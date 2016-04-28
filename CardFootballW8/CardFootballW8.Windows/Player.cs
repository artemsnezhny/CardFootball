using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardFootballW8
{
    public class Player : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public Field PField { get; set; }
        public Card FirstDeckCard
        {
            get { return deck.FirstOrDefault(); }
        }
        public int DeckCount
        {
            get { return deck.Count(); }
        }
        private List<Card> deck;

        public Player(string name)
        {
            this.Name = name;
            PField = new Field();
            deck = new List<Card>();
        }

        public void AddCardInDeck(Card newCard)
        {
            this.deck.Add(newCard);
            InvokePropertyChanged("DeckCount");
            if (DeckCount == 1)
                InvokePropertyChanged("FirstDeckCard");
        }

        public void RemoveFirstDeckCard()
        {
            this.deck.Remove(deck.First());
            InvokePropertyChanged("FirstDeckCard");
            InvokePropertyChanged("DeckCount");
        }

        private void InvokePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}