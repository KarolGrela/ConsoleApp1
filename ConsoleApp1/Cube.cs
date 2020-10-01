using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    // a class used by CurencyReport class to store nameof currency and rate
    class Cube
    {
        // fields
        private string currency;    // abreviation of currency name
        private double rate;         // rate of currency compared to euro

        // constructor - setter
        public Cube(string c, double r)
        {
            currency = c;
            rate = r;
        }

        // properties - getters
        public string Currency
        {
            get => currency;
        }
        public double Rate
        {
            get => rate;
        }
    }
}
