using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardFootballW8
{
    public class Game
    {
        const int GAME_END_SCORE = 3;

        public Player Player1 { get; set; }
        public Player Player2 { get; private set; }
        public Score Score { get; private set; }
        private readonly int gameEndScore;
        public int GameEndScore { get { return gameEndScore; } }

        public Game(Player p1, Player p2)
        {
            this.Player1 = p1;
            this.Player2 = p2;
            this.Score = new Score();
            this.gameEndScore = GAME_END_SCORE;
        }

        public Game(Player p1, Player p2, int gameEndScore)
            : this(p1, p2)
        {
            this.gameEndScore = gameEndScore;
        }

        public Card GetAndRemoveFirstCard(Player player)
        {
            Card card = player.FirstDeckCard;
            player.RemoveFirstDeckCard();
            return card;
        }
    }
}
