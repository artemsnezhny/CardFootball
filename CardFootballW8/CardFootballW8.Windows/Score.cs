using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardFootballW8
{
    public class Score : INotifyPropertyChanged
    {
        private int player1;
        public int Player1
        {
            get { return player1; }
            set 
            {
                this.player1 = value;
                InvokePropertyChanged("Player1");
            }
        }

        private int player2;
        public int Player2
        {
            get { return player2; }
            set
            {
                this.player2 = value;
                InvokePropertyChanged("Player2");
            }
        }

        public Score()
        {
            Player1 = 0;
            Player2 = 0;
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
