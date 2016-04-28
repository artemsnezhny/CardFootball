using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace CardFootballW8
{
    //Assets/Cards/10_of_clubs.png
    //var uri = new Uri(parent.BaseUri, path);
    //        BitmapImage result = new BitmapImage();
    //        result.UriSource = uri;
    //        return result;
    public sealed class ImageConverter : IValueConverter
    {
        const string path = @"ms-appx:///Assets/Cards/";

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                Card card = (Card)value;
                string cardPath = path + card.Name.ToLower() + "_" + "of" + "_" + card.Suit.ToString().ToLower() + ".png";
                var uri = new Uri(cardPath);

                return new BitmapImage(uri);
            }
            catch
            {
                return new BitmapImage();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
