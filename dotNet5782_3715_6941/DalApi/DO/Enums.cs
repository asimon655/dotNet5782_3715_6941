﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
/*
 
    _/_/_/  _/_/_/    _/    _/  _/      _/
     _/    _/    _/  _/    _/  _/_/  _/_/
    _/    _/    _/  _/_/_/_/  _/  _/  _/
   _/    _/    _/  _/    _/  _/      _/
_/_/_/  _/_/_/    _/    _/  _/      _/


 
 
       _/_/              _/
   _/    _/    _/_/_/      _/_/_/  _/_/      _/_/    _/_/_/
  _/_/_/_/  _/_/      _/  _/    _/    _/  _/    _/  _/    _/
 _/    _/      _/_/  _/  _/    _/    _/  _/    _/  _/    _/
_/    _/  _/_/_/    _/  _/    _/    _/    _/_/    _/    _/


 
 
 */
namespace DO
{
    public enum Priorities
    {
        [Description("Regular delivery ")]
        Regular,
        [Description("fast delivery")]
        Fast,
        [Description("Emergency delivery ")]
        Emergency
,
    }
    public enum WeightCategories
    {
        [Description("easy weight ")]
        Easy,
        [Description("medium weight ")]
        Medium,
        [Description("heavy weight  ")]
        Heavy,
    };
}
