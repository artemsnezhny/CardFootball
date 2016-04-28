using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardFootballW8
{
    public class Field : INotifyPropertyChanged
    {
        private Card goalkeeper;
        private Card defender1;
        private Card defender2;
        private Card defender3;

        public Card Goalkeeper
        {
            get { return goalkeeper; }
            set
            {
                goalkeeper = value;
                InvokePropertyChanged("Goalkeeper");
            }
        }
        public Card Defender1
        {
            get { return defender1; }
            set
            {
                defender1 = value;
                InvokePropertyChanged("Defender1");
            }
        }
        public Card Defender2
        {
            get { return defender2; }
            set
            {
                defender2 = value;
                InvokePropertyChanged("Defender2");
            }
        }
        public Card Defender3
        {
            get { return defender3; }
            set
            {
                defender3 = value;
                InvokePropertyChanged("Defender3");
            }
        }

        public Field()
        {
            this.Goalkeeper = new Card();
            this.Defender1 = new Card();
            this.Defender2 = new Card();
            this.Defender3 = new Card();
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
