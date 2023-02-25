using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBox
{
    public class Box
    {
        private double _width;
        private double _height;
        private int _amount;
        private DateTime _ExpiryDate;

        public Box(double width, double height, int amount, DateTime expiryDate)
        {
            _width = width;
            _height = height;
            _amount = amount;
            _ExpiryDate = expiryDate;
        }

        public double Width
        {
            get { return _width; }
            set
            {
                if (value > 0)
                {
                    _width = value;
                }
            }
        }

        public double Height
        {
            get { return _height; }
            set
            {
                if (value > 0)
                {
                    _height = value;
                }
            }
        }

        public int Amount
        {
            get { return _amount; }
            set
            {
                if (value >= 0 && value < 50)
                {
                    _amount = value;
                }
            }
        }

        public DateTime ExpiryDate
        {
            get
            {
                return _ExpiryDate;
            }

            set
            {
                if (value >= DateTime.Now)
                {
                    _ExpiryDate = value;
                }
            }
        }

        public override string ToString()
        {
            return $"The width is {_width} the height is {_height} the amount is {_amount} and the expiry date is {ExpiryDate}";
        }
    }
}
