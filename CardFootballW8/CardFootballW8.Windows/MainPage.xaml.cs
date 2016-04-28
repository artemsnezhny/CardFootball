using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CardFootballW8
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public Player player1 { get; set; }
        public Player player2 { get; set; }
        public int course { get; set; }
        public Game game { get; set; }
        public
        static DeckOfCardRus deck;

        Player CurrentPlayer;
        Player OtherPlayer;

        const string def1 = "def1";
        const string def2 = "def2";
        const string def3 = "def3";

        const double AI_DELAY_SEC = 1.5;

        public readonly Card NONE = new Card("None", 0, Suits.None);
        const int WIN_SCORE = 3;

        Card curCard;

        static bool AI;
        const string AI_NAME = "AI";
        const string DefaultPlayer1NameRU = "Игрок 1";
        const string DefaultPlayer2NameRU = "Игрок 2";
        const string DefaultPlayer1NameEN = "Player 1";
        const string DefaultPlayer2NameEN = "Player 2";

        bool noCards = false;

        public MainPage()
        {
            this.InitializeComponent();
            StartPop();
        }

        public void StartPop()
        {
            var bounds = Window.Current.Bounds;
            StartPopup.Width = bounds.Width;
            PupupHeaderStackPanel.Width = bounds.Width;
            StartPopup.IsOpen = true;

            if (bounds.Width == 1920 && bounds.Height == 1080)
            {
                P1DeckCountTransformControl.Margin = new Thickness(-250, 30, 0, 0);
                P2DeckCountTransformControl.Margin = new Thickness(0, 0, -250, 30);
            }

            if (bounds.Width == 2560 && bounds.Height == 1440)
            {
                P1DeckCountTransformControl.Margin = new Thickness(-320, 30, 0, 0);
                P2DeckCountTransformControl.Margin = new Thickness(0, 0, -320, 30);
            }

            if ((bounds.Width == 1024 && bounds.Height == 768)
                || (bounds.Width == 1440 && bounds.Height == 1080))
            {
                ImageBrush imageBrush = new ImageBrush();
                imageBrush.ImageSource = new BitmapImage(new Uri(@"ms-appx:///Assets/background75.jpg"));
                MainGrid.Background = imageBrush;
                p1Name.FontSize = 45;
                p2Name.FontSize = 45;
                Player1TransformControl.Margin = new Thickness(-25, 0, 0, 0);
                Player2TransformControl2.Margin = new Thickness(25, 0, 0, 0);
            }
        }

        public void Start(string player1Name, string player2Name)
        {
            curCard = NONE;
            var region = Windows.System.UserProfile.GlobalizationPreferences.Languages.FirstOrDefault();
            string p1Name = "Игрок 1";
            string p2Name = "Игрок 2";
            if (region == "en-US")
            {
                p1Name = player1Name == "" ? DefaultPlayer1NameEN : player1Name;
                p2Name = player2Name == "" ? DefaultPlayer2NameEN : player2Name;


            }
            else
                if (region == "ru")
                {
                    p1Name = player1Name == "" ? DefaultPlayer1NameRU : player1Name;
                    p2Name = player2Name == "" ? DefaultPlayer2NameRU : player2Name;
                }
            
            player1 = new Player(p1Name);
            if (!AI)
                player2 = new Player(p2Name);
            else
                player2 = new Player(AI_NAME);

            Random rnd = new Random();
            course = rnd.Next(0, 2);

            game = new Game(player1, player2, WIN_SCORE);

            deck = new DeckOfCardRus();
            deck.Shuffle();


            for (int i = 0; i < deck.Deck.Count() / 2; i++)
            {
                game.Player1.AddCardInDeck(deck.Deck.ElementAt(i));
                game.Player2.AddCardInDeck(deck.Deck.ElementAt(i + 18));
            }

            game.Player1.PField.Defender1 = game.GetAndRemoveFirstCard(game.Player1);
            game.Player1.PField.Defender2 = game.GetAndRemoveFirstCard(game.Player1);
            game.Player1.PField.Defender3 = game.GetAndRemoveFirstCard(game.Player1);
            game.Player1.PField.Goalkeeper = game.GetAndRemoveFirstCard(game.Player1);

            game.Player2.PField.Defender1 = game.GetAndRemoveFirstCard(game.Player2);
            game.Player2.PField.Defender2 = game.GetAndRemoveFirstCard(game.Player2);
            game.Player2.PField.Defender3 = game.GetAndRemoveFirstCard(game.Player2);
            game.Player2.PField.Goalkeeper = game.GetAndRemoveFirstCard(game.Player2);

            CurrentPlayer = (course == 0) ? game.Player1 : game.Player2;
            OtherPlayer = (course == 0) ? game.Player2 : game.Player1;
            this.DataContext = game;

            ShowCourse();

            NextCardVisibility();

            noCards = false;
            if (CurrentPlayer.Name == AI_NAME && AI)
                AI_Attack();
        }

        private void card_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Image image = sender as Image;
            string name = image.Name;
            switch (name)
            {
                case "p1g":
                    {
                        if (CurrentPlayer.Name != game.Player1.Name && OtherPlayer.PField.Defender1 == NONE
                            && OtherPlayer.PField.Defender2 == NONE && OtherPlayer.PField.Defender3 == NONE)
                        {
                            AttackGoalkeeper();
                        }
                        if (CurrentPlayer.Name == game.Player1.Name && CurrentPlayer.DeckCount == 0
                            && CurrentPlayer.PField.Defender1 == NONE && CurrentPlayer.PField.Defender2 == NONE
                            && CurrentPlayer.PField.Defender3 == NONE)
                        {
                            CurrentPlayer.AddCardInDeck(CurrentPlayer.PField.Goalkeeper);
                            CurrentPlayer.PField.Goalkeeper = NONE;
                        }
                        break;
                    }
                case "p1d1":
                    {
                        if (CurrentPlayer.Name != game.Player1.Name)
                        {
                            AttackDef(def1);
                        }
                        if (CurrentPlayer.Name == game.Player1.Name && CurrentPlayer.DeckCount == 0)
                        {
                            CurrentPlayer.AddCardInDeck(CurrentPlayer.PField.Defender1);
                            CurrentPlayer.PField.Defender1 = NONE;
                        }
                        break;
                    }
                case "p1d2":
                    {
                        if (CurrentPlayer.Name != game.Player1.Name)
                        {
                            AttackDef(def2);
                        }
                        if (CurrentPlayer.Name == game.Player1.Name && CurrentPlayer.DeckCount == 0)
                        {
                            CurrentPlayer.AddCardInDeck(CurrentPlayer.PField.Defender2);
                            CurrentPlayer.PField.Defender2 = NONE;
                        }
                        break;
                    }
                case "p1d3":
                    {
                        if (CurrentPlayer.Name != game.Player1.Name)
                        {
                            AttackDef(def3);
                        }
                        if (CurrentPlayer.Name == game.Player1.Name && CurrentPlayer.DeckCount == 0)
                        {
                            CurrentPlayer.AddCardInDeck(CurrentPlayer.PField.Defender3);
                            CurrentPlayer.PField.Defender3 = NONE;
                        }
                        break;
                    }

                case "p2g":
                    {
                        if (CurrentPlayer.Name != game.Player2.Name && OtherPlayer.PField.Defender1 == NONE
                            && OtherPlayer.PField.Defender2 == NONE && OtherPlayer.PField.Defender3 == NONE)
                        {
                            AttackGoalkeeper();
                        }
                        if (CurrentPlayer.Name == game.Player2.Name && CurrentPlayer.DeckCount == 0
                            && CurrentPlayer.PField.Defender1 == NONE && CurrentPlayer.PField.Defender2 == NONE
                            && CurrentPlayer.PField.Defender3 == NONE)
                        {
                            CurrentPlayer.AddCardInDeck(CurrentPlayer.PField.Goalkeeper);
                            CurrentPlayer.PField.Goalkeeper = NONE;
                        }
                        break;
                    }
                case "p2d1":
                    {
                        if (CurrentPlayer.Name != game.Player2.Name)
                        {
                            AttackDef(def1);
                        }
                        if (CurrentPlayer.Name == game.Player2.Name && CurrentPlayer.DeckCount == 0)
                        {
                            CurrentPlayer.AddCardInDeck(CurrentPlayer.PField.Defender1);
                            CurrentPlayer.PField.Defender1 = NONE;
                        }
                        break;
                    }
                case "p2d2":
                    {
                        if (CurrentPlayer.Name != game.Player2.Name)
                        {
                            AttackDef(def2);
                        }
                        if (CurrentPlayer.Name == game.Player2.Name && CurrentPlayer.DeckCount == 0)
                        {
                            CurrentPlayer.AddCardInDeck(CurrentPlayer.PField.Defender2);
                            CurrentPlayer.PField.Defender2 = NONE;
                        }
                        break;
                    }
                case "p2d3":
                    {
                        if (CurrentPlayer.Name != game.Player2.Name)
                        {
                            AttackDef(def3);
                        }
                        if (CurrentPlayer.Name == game.Player2.Name && CurrentPlayer.DeckCount == 0)
                        {
                            CurrentPlayer.AddCardInDeck(CurrentPlayer.PField.Defender3);
                            CurrentPlayer.PField.Defender3 = NONE;
                        }
                        break;
                    }
                default:
                    break;
            }
        }

        private void AttackGoalkeeper()
        {
            curCard = CurrentPlayer.FirstDeckCard;
            if ((curCard.Weight > OtherPlayer.PField.Goalkeeper.Weight)
                 || ((curCard is N6) && (OtherPlayer.PField.Goalkeeper is Ace)))
            {
                CurrentPlayer.AddCardInDeck(OtherPlayer.PField.Goalkeeper);
                OtherPlayer.PField.Goalkeeper = NONE;
                CurrentPlayer.RemoveFirstDeckCard();
                CurrentPlayer.AddCardInDeck(curCard);
                if (course == 0) game.Score.Player1++;
                else game.Score.Player2++;

                if (OtherPlayer.DeckCount == 0)
                {
                    PlayerHasNoCards(OtherPlayer.Name);
                }

                RefreshFields();
            }
        }

        private void AttackDef(string defName)
        {
            Card OtherPlayerDefender = NONE;
            curCard = CurrentPlayer.FirstDeckCard;
            switch (defName)
            {
                case def1:
                    {
                        OtherPlayerDefender = OtherPlayer.PField.Defender1;
                        break;
                    }
                case def2:
                    {
                        OtherPlayerDefender = OtherPlayer.PField.Defender2;
                        break;
                    }
                case def3:
                    {
                        OtherPlayerDefender = OtherPlayer.PField.Defender3;
                        break;
                    }
            }
            if (OtherPlayerDefender != NONE && CurrentPlayer.DeckCount != 0)
            {
                if ((curCard.Weight > OtherPlayerDefender.Weight)
                    || ((curCard is N6) && (OtherPlayerDefender is Ace)))
                {
                    CurrentPlayer.AddCardInDeck(OtherPlayerDefender);
                    switch (defName)
                    {
                        case def1:
                            {
                                OtherPlayer.PField.Defender1 = NONE;
                                break;
                            }
                        case def2:
                            {
                                OtherPlayer.PField.Defender2 = NONE;
                                break;
                            }
                        case def3:
                            {
                                OtherPlayer.PField.Defender3 = NONE;
                                break;
                            }
                    }
                    CurrentPlayer.AddCardInDeck(curCard);
                    CurrentPlayer.RemoveFirstDeckCard();
                }
            }
        }

        private void ShowCourse()
        {
            if (course == 0)
            {
                p1Name.Foreground = new SolidColorBrush(Colors.Black);
                p2Name.Foreground = new SolidColorBrush(Colors.White);
            }
            else
            {
                p2Name.Foreground = new SolidColorBrush(Colors.Black);
                p1Name.Foreground = new SolidColorBrush(Colors.White);
            }
        }

        private void next_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Image image = sender as Image;
            switch (image.Name)
            {
                case "p1next":
                    {
                        if (CurrentPlayer.Name == game.Player1.Name)
                        {
                            OtherPlayer.AddCardInDeck(game.GetAndRemoveFirstCard(CurrentPlayer));
                            course = 1;

                            if (OtherPlayer.DeckCount == 0 && OtherPlayer.PField.Defender1 == NONE
                                && OtherPlayer.PField.Defender2 == NONE && OtherPlayer.PField.Defender3 == NONE
                                && OtherPlayer.PField.Goalkeeper == NONE)
                            {
                                PlayerHasNoCards(OtherPlayer.Name);
                            }

                            ShowCourse();
                            RefreshFields();
                        }
                        break;
                    }
                case "p2next":
                    {
                        if (CurrentPlayer.Name == game.Player2.Name)
                        {
                            OtherPlayer.AddCardInDeck(game.GetAndRemoveFirstCard(CurrentPlayer));
                            course = 0;

                            if (OtherPlayer.DeckCount == 0 && OtherPlayer.PField.Defender1 == NONE
                                && OtherPlayer.PField.Defender2 == NONE && OtherPlayer.PField.Defender3 == NONE
                                && OtherPlayer.PField.Goalkeeper == NONE)
                            {
                                PlayerHasNoCards(OtherPlayer.Name);
                            }

                            ShowCourse();
                            RefreshFields();
                        }
                        break;
                    }
            }

            CurrentPlayer = (course == 0) ? game.Player1 : game.Player2;
            OtherPlayer = (course == 0) ? game.Player2 : game.Player1;

            NextCardVisibility();

            if (CurrentPlayer.Name == AI_NAME && AI)
                AI_Attack();
        }

        private void PlayerHasNoCards(string playerName)
        {
            this.noCards = true;

            MessageDialog msgDialog = new MessageDialog("У игрока " + playerName + " больше нет карт.");
            UICommand newGameBtn = new UICommand("Новая игра");
            newGameBtn.Invoked = NewGameBtnClick;
            msgDialog.Commands.Add(newGameBtn);

            this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => msgDialog.ShowAsync());
        }

        private void NewGameBtnClick(IUICommand command)
        {
            StartPop();
        }

        private void NextCardVisibility()
        {
            if (course == 0)
            {
                p2next.Visibility = Visibility.Collapsed;
                p2nextCollapsed.Visibility = Visibility.Visible;

                p1next.Visibility = Visibility.Visible;
                p1nextCollapsed.Visibility = Visibility.Collapsed;
            }
            else
            {
                p1next.Visibility = Visibility.Collapsed;
                p1nextCollapsed.Visibility = Visibility.Visible;

                p2next.Visibility = Visibility.Visible;
                p2nextCollapsed.Visibility = Visibility.Collapsed;
            }
        }

        private async void AI_Attack()
        {
            bool exit = false;
            while (!exit && !noCards)
            {
                await Task.Delay(TimeSpan.FromSeconds(AI_DELAY_SEC));
                if (CurrentPlayer.DeckCount == 0)
                {
                    //----

                    if (CurrentPlayer.PField.Defender1 != NONE || CurrentPlayer.PField.Defender2 != NONE
                            || CurrentPlayer.PField.Defender3 != NONE)
                    {
                        int defN = 0; //0???
                        if ((CurrentPlayer.Name == AI_NAME) && AI)
                        {
                            if ((CurrentPlayer.PField.Defender1.Weight >= CurrentPlayer.PField.Defender2.Weight)
                                && (CurrentPlayer.PField.Defender1.Weight >= CurrentPlayer.PField.Defender3.Weight))
                                defN = 1;
                            else
                                if ((CurrentPlayer.PField.Defender2.Weight >= CurrentPlayer.PField.Defender1.Weight)
                                && (CurrentPlayer.PField.Defender2.Weight >= CurrentPlayer.PField.Defender3.Weight))
                                    defN = 2;
                                else
                                    if ((CurrentPlayer.PField.Defender3.Weight >= CurrentPlayer.PField.Defender1.Weight)
                                && (CurrentPlayer.PField.Defender3.Weight >= CurrentPlayer.PField.Defender2.Weight))
                                        defN = 3;
                        }
                        switch (defN)
                        {
                            case 1:
                                {
                                    curCard = CurrentPlayer.PField.Defender1;
                                    CurrentPlayer.PField.Defender1 = NONE;
                                    CurrentPlayer.AddCardInDeck(curCard);
                                    break;
                                }
                            case 2:
                                {
                                    curCard = CurrentPlayer.PField.Defender2;
                                    CurrentPlayer.PField.Defender2 = NONE;
                                    CurrentPlayer.AddCardInDeck(curCard);
                                    break;
                                }
                            case 3:
                                {
                                    curCard = CurrentPlayer.PField.Defender3;
                                    CurrentPlayer.PField.Defender3 = NONE;
                                    CurrentPlayer.AddCardInDeck(curCard);
                                    break;
                                }
                        }

                    }
                    else
                    {
                        if (CurrentPlayer.PField.Goalkeeper != NONE)
                        {
                            curCard = CurrentPlayer.PField.Goalkeeper;
                            CurrentPlayer.PField.Goalkeeper = NONE;
                            CurrentPlayer.AddCardInDeck(curCard);
                        }
                    }

                    //----
                }
                else
                {
                    if (OtherPlayer.PField.Defender1 != NONE || OtherPlayer.PField.Defender2 != NONE || OtherPlayer.PField.Defender3 != NONE)
                    {
                        int defNumber = 0;
                        if ((CurrentPlayer.Name == AI_NAME) && AI)
                        {
                            defNumber = GetAttackDefenderNum(CurrentPlayer.FirstDeckCard.Weight, OtherPlayer.PField.Defender1.Weight,
                                OtherPlayer.PField.Defender2.Weight, OtherPlayer.PField.Defender3.Weight);
                        }
                        switch (defNumber)
                        {
                            case 1:
                                AttackDef(def1);
                                break;
                            case 2:
                                AttackDef(def2);
                                break;
                            case 3:
                                AttackDef(def3);
                                break;
                            case 0:
                                next_Tapped(p2next, null);
                                exit = true;
                                break;
                        }
                    }
                    else
                    {
                        if ((CurrentPlayer.FirstDeckCard.Weight > OtherPlayer.PField.Goalkeeper.Weight)
                            || (CurrentPlayer.FirstDeckCard is N6 && OtherPlayer.PField.Goalkeeper is Ace))
                            AttackGoalkeeper();
                        else
                        {
                            next_Tapped(p2next, null);
                            exit = true;
                        }
                    }
                }
            }
        }

        private int GetAttackDefenderNum(int curCardWeight, int def1Weight, int def2Weight, int def3Weight)
        {
            int result = 0;
            int[] results = new int[4];
            results[0] = int.MaxValue;
            results[1] = (def1Weight == NONE.Weight) ? int.MinValue : (curCardWeight - def1Weight);
            results[2] = (def2Weight == NONE.Weight) ? int.MinValue : (curCardWeight - def2Weight);
            results[3] = (def3Weight == NONE.Weight) ? int.MinValue : (curCardWeight - def3Weight);
            for (int i = 0; i < results.Length; i++)
                if (results[i] <= 0) results[i] = int.MaxValue;
            result = results.Min();
            //проверка на Ace и 6 ПЛОХАЯ
            if (curCardWeight == 6 && def1Weight == 14)
                return 1;
            if (curCardWeight == 6 && def2Weight == 14)
                return 2;
            if (curCardWeight == 6 && def3Weight == 14)
                return 3;
            if (result > 0)
            {
                for (int i = 0; i < results.Length; i++)
                {
                    if (results[i] == result)
                        return i;
                }
                return 0;
            }
            else
                return 0;
        }

        private void RefreshFields()
        {
            if (OtherPlayer.PField.Goalkeeper == NONE)
            {
                if (OtherPlayer.DeckCount != 0)
                    OtherPlayer.PField.Goalkeeper = game.GetAndRemoveFirstCard(OtherPlayer);

                if (OtherPlayer.DeckCount != 0)
                    OtherPlayer.PField.Defender1 = game.GetAndRemoveFirstCard(OtherPlayer);
                if (OtherPlayer.DeckCount != 0)
                    OtherPlayer.PField.Defender2 = game.GetAndRemoveFirstCard(OtherPlayer);
                if (OtherPlayer.DeckCount != 0)
                    OtherPlayer.PField.Defender3 = game.GetAndRemoveFirstCard(OtherPlayer);
            }
            //CurrentPlayer
            if (CurrentPlayer.PField.Defender1 == NONE && CurrentPlayer.DeckCount != 0)
                CurrentPlayer.PField.Defender1 = game.GetAndRemoveFirstCard(CurrentPlayer);
            if (CurrentPlayer.PField.Defender2 == NONE && CurrentPlayer.DeckCount != 0)
                CurrentPlayer.PField.Defender2 = game.GetAndRemoveFirstCard(CurrentPlayer);
            if (CurrentPlayer.PField.Defender3 == NONE && CurrentPlayer.DeckCount != 0)
                CurrentPlayer.PField.Defender3 = game.GetAndRemoveFirstCard(CurrentPlayer);

            if (OtherPlayer.PField.Defender1 == NONE && OtherPlayer.DeckCount != 0)
                OtherPlayer.PField.Defender1 = game.GetAndRemoveFirstCard(OtherPlayer);
            if (OtherPlayer.PField.Defender2 == NONE && OtherPlayer.DeckCount != 0)
                OtherPlayer.PField.Defender2 = game.GetAndRemoveFirstCard(OtherPlayer);
            if (OtherPlayer.PField.Defender3 == NONE && OtherPlayer.DeckCount != 0)
                OtherPlayer.PField.Defender3 = game.GetAndRemoveFirstCard(OtherPlayer);
        }

        private void IsAICheckBox_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)IsAICheckBox.IsChecked)
            {
                Player2NameTextBox.Text = AI_NAME;
                Player2NameTextBox.IsEnabled = false;
                AI = true;
            }
            else
            {
                Player2NameTextBox.Text = "";
                Player2NameTextBox.IsEnabled = true;
                AI = false;
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            StartPopup.IsOpen = false;
            Start(Player1NameTextBox.Text, Player2NameTextBox.Text);
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            StartPop();
            BottomAppBar.IsOpen = false;
        }

        private void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            CardFootballW8.RulesSettingsFlyout helpSF = new CardFootballW8.RulesSettingsFlyout();
            // When the settings flyout is opened from the app bar instead of from
            // the setting charm, use the ShowIndependent() method.
            helpSF.ShowIndependent();
            BottomAppBar.IsOpen = false;
        }
    }


}
