using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardFootballW8
{
    public enum Suits
    {
        Spades, //Пики
        Clubs, //Трефы
        Hearts, //Черви
        Diamonds, //Бубны
        None
    }

    public class Card
    {
        private string name;
        public string Name
        {
            get { return name; }
            private set
            {
                name = value;
            }
        }
        public int Weight { get; private set; }
        public Suits Suit { get; private set; }

        public Card(string name, int weight, Suits suit)
        {
            this.Name = name;
            this.Weight = weight;
            this.Suit = suit;
        }

        public Card()
        { }
    }

    public class Joker : Card
    {
        const string NAME = "Joker";
        const int WEIGHT = 15;
        public Joker(Suits suit)
            : base(NAME, WEIGHT, suit)
        { }
    }

    public class Ace : Card
    {
        const string NAME = "Ace";
        const int WEIGHT = 14;
        public Ace(Suits suit)
            : base(NAME, WEIGHT, suit)
        { }
    }

    public class King : Card
    {
        const string NAME = "King";
        const int WEIGHT = 13;
        public King(Suits suit)
            : base(NAME, WEIGHT, suit)
        { }
    }

    public class Queen : Card
    {
        const string NAME = "Queen";
        const int WEIGHT = 12;
        public Queen(Suits suit)
            : base(NAME, WEIGHT, suit)
        { }
    }

    public class Jack : Card
    {
        const string NAME = "Jack";
        const int WEIGHT = 11;
        public Jack(Suits suit)
            : base(NAME, WEIGHT, suit)
        { }
    }

    public class N10 : Card
    {
        const string NAME = "10";
        const int WEIGHT = 10;
        public N10(Suits suit)
            : base(NAME, WEIGHT, suit)
        { }
    }

    public class N9 : Card
    {
        const string NAME = "9";
        const int WEIGHT = 9;
        public N9(Suits suit)
            : base(NAME, WEIGHT, suit)
        { }
    }

    public class N8 : Card
    {
        const string NAME = "8";
        const int WEIGHT = 8;
        public N8(Suits suit)
            : base(NAME, WEIGHT, suit)
        { }
    }

    public class N7 : Card
    {
        const string NAME = "7";
        const int WEIGHT = 7;
        public N7(Suits suit)
            : base(NAME, WEIGHT, suit)
        { }
    }

    public class N6 : Card
    {
        const string NAME = "6";
        const int WEIGHT = 6;
        public N6(Suits suit)
            : base(NAME, WEIGHT, suit)
        { }
    }

    public class N5 : Card
    {
        const string NAME = "5";
        const int WEIGHT = 5;
        public N5(Suits suit)
            : base(NAME, WEIGHT, suit)
        { }
    }

    public class N4 : Card
    {
        const string NAME = "4";
        const int WEIGHT = 4;
        public N4(Suits suit)
            : base(NAME, WEIGHT, suit)
        { }
    }

    public class N3 : Card
    {
        const string NAME = "3";
        const int WEIGHT = 3;
        public N3(Suits suit)
            : base(NAME, WEIGHT, suit)
        { }
    }

    public class N2 : Card
    {
        const string NAME = "2";
        const int WEIGHT = 2;
        public N2(Suits suit)
            : base(NAME, WEIGHT, suit)
        { }
    }

    public class DeckOfCardRus
    {
        private List<Card> deck;
        public IEnumerable<Card> Deck { get { return deck; } }

        public DeckOfCardRus()
        {
            this.deck = new List<Card>();

            deck.Add(new Ace(Suits.Clubs));
            deck.Add(new Ace(Suits.Diamonds));
            deck.Add(new Ace(Suits.Hearts));
            deck.Add(new Ace(Suits.Spades));

            deck.Add(new King(Suits.Clubs));
            deck.Add(new King(Suits.Diamonds));
            deck.Add(new King(Suits.Hearts));
            deck.Add(new King(Suits.Spades));

            deck.Add(new Queen(Suits.Clubs));
            deck.Add(new Queen(Suits.Diamonds));
            deck.Add(new Queen(Suits.Hearts));
            deck.Add(new Queen(Suits.Spades));

            deck.Add(new Jack(Suits.Clubs));
            deck.Add(new Jack(Suits.Diamonds));
            deck.Add(new Jack(Suits.Hearts));
            deck.Add(new Jack(Suits.Spades));

            deck.Add(new N10(Suits.Clubs));
            deck.Add(new N10(Suits.Diamonds));
            deck.Add(new N10(Suits.Hearts));
            deck.Add(new N10(Suits.Spades));

            deck.Add(new N9(Suits.Clubs));
            deck.Add(new N9(Suits.Diamonds));
            deck.Add(new N9(Suits.Hearts));
            deck.Add(new N9(Suits.Spades));

            deck.Add(new N8(Suits.Clubs));
            deck.Add(new N8(Suits.Diamonds));
            deck.Add(new N8(Suits.Hearts));
            deck.Add(new N8(Suits.Spades));

            deck.Add(new N7(Suits.Clubs));
            deck.Add(new N7(Suits.Diamonds));
            deck.Add(new N7(Suits.Hearts));
            deck.Add(new N7(Suits.Spades));

            deck.Add(new N6(Suits.Clubs));
            deck.Add(new N6(Suits.Diamonds));
            deck.Add(new N6(Suits.Hearts));
            deck.Add(new N6(Suits.Spades));
        }

        public void Shuffle()
        {
            Random rng = new Random();
            int n = deck.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Card value = deck[k];
                deck[k] = deck[n];
                deck[n] = value;
            }
        }
    }
}
