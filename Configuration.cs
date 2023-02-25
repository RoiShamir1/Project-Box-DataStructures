using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBox
{
    public class Configuration
    {
        private const int _MaxAmount = 50;
        private const int _MinAmount = 10;
        private const int _MaxSplit = 2;
        private const double _Deviation = 1.25;

        public int MaxAmount
        {
            get { return _MaxAmount; }
        }

        public int MinAmount
        {
            get { return _MinAmount; }
        }

        public int MaxSplit
        {
            get { return _MaxSplit; }
        }

        public double Deviation
        {
            get { return _Deviation; }
        }
    }
}
