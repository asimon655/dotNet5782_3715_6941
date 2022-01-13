using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace PL
{
    public class DELClient : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (((BO.CustomerList)value).InTheWay == 0 && ((BO.CustomerList)value).ParcelDeliveredAndNotGot == 0 ? Visibility.Visible : Visibility.Hidden);
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
    public class DELStat : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (((BO.StationList)value).BusyPorts == 0 ? Visibility.Visible : Visibility.Hidden);
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
    public class DELDRN : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((BO.DroneStatuses)value == BO.DroneStatuses.Free ? Visibility.Visible : Visibility.Hidden);
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
    public class DELAV : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((BO.ParcelStatus)value == BO.ParcelStatus.Declared ? Visibility.Visible : Visibility.Hidden);
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
    public class HalfScale : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double x;
            if (double.TryParse(value.ToString(), out x))
            {
                return x / 2;
            }

            return 0;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
    public class BatteryCRT : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((double)value<30 || (double)value >=60 )
                return new SolidColorBrush(Color.FromRgb((byte)(255 - (int)((double)value * 255 / 100)), (byte)((int)((double)value * 255 / 100)), 0));
            if((double)value < 60)
                return new SolidColorBrush(Color.FromRgb((byte)(255 - (int)(255-(double)value * 255 / 100)+102), (byte)(255-(int)((double)value * 255 / 100)+102), 0));
            return null;

        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
    public class NULLEMP : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return "";
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
    public class Disappearer : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (value is null ? new GridLength(0, GridUnitType.Star) : new GridLength(1, GridUnitType.Star));
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }


}
