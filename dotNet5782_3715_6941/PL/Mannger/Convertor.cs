﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace PL
{
    public  class DELAV : IValueConverter
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

	  return (double) value / 2; 
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
	 return new SolidColorBrush(Color.FromRgb((byte)(255-(int)((double)value*255/100)), (byte)((int)((double)value * 255 / 100)), 0));
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


}
