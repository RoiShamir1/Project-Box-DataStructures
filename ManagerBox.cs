using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBox
{
    public class ManagerBox
    {
        private SortedDictionary<double, SortedDictionary<double, Box>> _widths = new SortedDictionary<double, SortedDictionary<double, Box>>();

        Configuration configuration = new Configuration();

        private SortedList<DateTime, Box> _date = new SortedList<DateTime, Box>();

        public SortedDictionary<double, SortedDictionary<double, Box>> Widths
        {
            get { return _widths; }
        }

        public SortedList<DateTime, Box> Date
        {
            get { return _date; }
        }

        public override string ToString()
        {
            foreach (var pair in Widths)
            {
                foreach (KeyValuePair<double, Box> box in pair.Value)
                {
                    Console.WriteLine(box.ToString());
                }
            }
            return "";
        }

        public void Print()
        {
            foreach (var pair in Widths)
            {
                foreach (KeyValuePair<double, Box> box in pair.Value)
                {
                    Console.WriteLine(box.ToString());
                    Console.WriteLine();
                }
            }
        }

        public void RemoveBox()
        {
            foreach (var box in Date)
            {

                if (box.Value.ExpiryDate <= DateTime.Now)
                {
                    Widths[box.Value.Width].Remove(box.Value.Height);
                    //Date.Remove(box.Value.ExpiryDate);
                }
                else if (box.Value.ExpiryDate > DateTime.Now)
                {
                    break;
                }

            }
            //foreach (var boxs in date)
            //{
            //    Console.WriteLine(boxs);
            //}
        }

        public Box SegestBox(double width, double height)
        {
            foreach (var w in Widths.Keys.Where(w => w <= configuration.Deviation * width))
            {
                foreach( var h in Widths[w].Keys.Where(h => h <= configuration.Deviation * height))
                {
                    return Widths[w][h];
                }
            }
            return null;
        }

        public Box FindBoxes(double width, double height, int amount)
        {
            foreach (var w in Widths.Keys.Where(w => w <= configuration.Deviation * width))
            {
                foreach (var h in Widths[w].Keys.Where(h => h <= configuration.Deviation * height))
                {
                    return Widths[w][h];
                }
            }
            return null;
        }

        public void BigSell(double w, double h, int a)
        {
            if (Widths.ContainsKey(w))
            {
                if (Widths[w].ContainsKey(h))
                {
                    if (Widths[w][h].Amount > a)
                    {
                        Console.WriteLine("Press 1 to purchase and 2 for cancel");
                        int confirm = int.Parse(Console.ReadLine());
                        if (confirm == 1)
                        {
                            Widths[w][h].Amount -= a;
                            if (Widths[w][h].ExpiryDate.Day - DateTime.Now.Day < 5)
                            {
                                Widths[w][h].ExpiryDate.AddDays(5);
                            }
                            Console.WriteLine("Boxs purchased");
                            if (Widths[w][h].Amount == 0)
                            {
                                Console.WriteLine("You bought the last box!");
                                Widths[w].Remove(h);
                            }
                            else if (Widths[w][h].Amount < configuration.MinAmount)
                            {
                                Console.WriteLine("Warning low amount!!");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Purchased cancel!");
                        }
                    }
                    else
                    {
                        var box = FindBoxes(w, h, a);
                        if (box != null)
                        {
                            if (Widths[w][h].Amount + box.Amount > a)
                            {
                                Console.WriteLine($"We have another box to sagest: {box}");
                                Console.WriteLine("press 1 for yes and 2 to cancel");
                                int confirm = int.Parse((Console.ReadLine()));
                                if (confirm == 1)
                                {
                                    int left = a - Widths[w][h].Amount;
                                    if (Widths[box.Width][box.Height].ExpiryDate.Day - DateTime.Now.Day < 5)
                                    {
                                        Widths[box.Width][box.Height].ExpiryDate.AddDays(5);
                                    }
                                    Widths[box.Width][box.Height].Amount = 0;
                                    Widths[box.Width].Remove(box.Height);
                                    box.Amount -= left;
                                }
                            }
                            else
                            {
                                Console.WriteLine("we dont have enough amount");
                            }
                        }
                        Console.WriteLine("boxes not found");
                    }
                }
                else
                {
                    Console.WriteLine("Boxes not found!");
                }
            }
            else
            {
                Console.WriteLine("Boxes not found!");
            }
        }

        public void StartManager()
        {

            int selection = 3;

            while (selection != 0)
            {
                Console.WriteLine("For admin entery press 1\nFor buying boxs press 2\nFor exit press 0");
                selection = int.Parse(Console.ReadLine());
                if (selection == 1)
                {
                    try
                    {
                        Console.WriteLine("What is the width of the box you want to add?");
                        double widthbox = double.Parse(Console.ReadLine());
                        Console.WriteLine("What is the height?");
                        double heightbox = double.Parse(Console.ReadLine());
                        Console.WriteLine("What is the amount?");
                        int amountbox = int.Parse(Console.ReadLine());
                        Console.WriteLine("What is the expiry date?");
                        //DateTime expirydate = DateTime.Parse(Console.ReadLine());
                        DateTime expirydate = Convert.ToDateTime(Console.ReadLine());
                        if (Widths.ContainsKey(widthbox))
                        {
                            if (Widths[widthbox].ContainsKey(heightbox))
                            {
                                Widths[widthbox][heightbox].Amount += amountbox;
                                if (Widths[widthbox][heightbox].Amount > configuration.MaxAmount)
                                {
                                    Widths[widthbox][heightbox].Amount = configuration.MaxAmount;
                                }
                            }
                            else
                            {
                                Widths[widthbox].Add(heightbox, new Box(widthbox, heightbox, amountbox, expirydate));
                                Date.Add(Widths[widthbox][heightbox].ExpiryDate, new Box(widthbox, heightbox, amountbox, expirydate));
                                //Widths[widthbox][heightbox] = new Box(widthbox,heightbox,amountbox);
                            }
                        }
                        else
                        {
                            Widths.Add(widthbox, new SortedDictionary<double, Box>());
                            Widths[widthbox].Add(heightbox, new Box(widthbox, heightbox, amountbox, expirydate));
                            Date.Add(Widths[widthbox][heightbox].ExpiryDate, new Box(widthbox, heightbox, amountbox, expirydate));
                            //Widths[widthbox] = new SortedDictionary<double, Box>();
                        }
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine("Please insert a valid input! date in format 2000/11/11");
                        Console.WriteLine();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Please insert a valid input date in format 2000/11/11!");
                        Console.WriteLine();
                    }
                    RemoveBox();
                    Print();
                }
                if (selection == 2)
                {
                    Console.WriteLine("Do you want a single or numerous boxes?");
                    Console.WriteLine("Press 1 for single and 2 for numerous");
                    int quantity = int.Parse(Console.ReadLine());
                    if (quantity == 1)
                    {
                        Console.WriteLine("What is the width of the box you want?");
                        double widthbox = double.Parse(Console.ReadLine());
                        Console.WriteLine("What is the height?");
                        double heightbox = double.Parse(Console.ReadLine());

                        if (Widths.ContainsKey(widthbox))
                        {
                            if (Widths[widthbox].ContainsKey(heightbox))
                            {
                                Console.WriteLine($"Are you sure you want to buy a box {widthbox},{heightbox}");
                                Console.WriteLine("Press 1 to purchase and 2 for cancel");
                                int confirm = int.Parse(Console.ReadLine());
                                if (confirm == 1)
                                {
                                    Console.WriteLine("Box purchased!");
                                    Widths[widthbox][heightbox].Amount -= 1;
                                    if (Widths[widthbox][heightbox].ExpiryDate.Day - DateTime.Now.Day < 5)
                                    {
                                        Widths[widthbox][heightbox].ExpiryDate.AddDays(5);
                                    }
                                    
                                    //Widths[widthbox][heightbox].ExpiryDate.AddDays(10);
                                    if (Widths[widthbox][heightbox].Amount == 0)
                                    {
                                        Console.WriteLine("You bought the last box!");
                                        Widths[widthbox].Remove(heightbox);
                                    }
                                    else if (Widths[widthbox][heightbox].Amount < configuration.MinAmount)
                                    {
                                        Console.WriteLine("Warning low amount!!");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Purchased cancel!");
                                }
                            }
                            else
                            {
                                Console.WriteLine($"There is no fit box");
                                var segest = SegestBox(widthbox, heightbox);
                                if (segest != null)
                                {
                                    Console.WriteLine($"Would you like another box: {segest.ToString()}?");
                                    Console.WriteLine("Press 1 for yes and 2 for cancel");
                                    int insert = int.Parse(Console.ReadLine());
                                    if (insert == 1)
                                    {
                                        Widths[segest.Width][segest.Height].Amount -= 1;
                                        if (Widths[segest.Width][segest.Height].ExpiryDate.Day - DateTime.Now.Day < 5)
                                        {
                                            Widths[segest.Width][segest.Height].ExpiryDate.AddDays(5);
                                        }
                                        if (Widths[segest.Width][segest.Height].Amount == 0)
                                        {
                                            Console.WriteLine("You bought the last box!");
                                            Widths[segest.Width].Remove(segest.Height);
                                        }
                                        else if (Widths[segest.Width][segest.Height].Amount < configuration.MinAmount)
                                        {
                                            Console.WriteLine("Warning low amount!!");
                                        }
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("There is no suitable box");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine($"There is no fit box");
                            var segest = SegestBox(widthbox, heightbox);
                            if (segest != null)
                            {
                                Console.WriteLine($"Would you like another box: {segest.ToString()}?");
                                Console.WriteLine("Press 1 for yes and 2 for cancel");
                                int insert = int.Parse(Console.ReadLine());
                                if (insert == 1)
                                {
                                    Widths[segest.Width][segest.Height].Amount -= 1;
                                    if (Widths[segest.Width][segest.Height].ExpiryDate.Day - DateTime.Now.Day < 5)
                                    {
                                        Widths[segest.Width][segest.Height].ExpiryDate.AddDays(5);
                                    }
                                    if (Widths[segest.Width][segest.Height].Amount == 0)
                                    {
                                        Console.WriteLine("You bought the last box!");
                                        Widths[segest.Width].Remove(segest.Height);
                                    }
                                    else if (Widths[segest.Width][segest.Height].Amount < configuration.MinAmount)
                                    {
                                        Console.WriteLine("Warning low amount!!");
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("There is no suitable box");
                            }
                        }
                    }
                    else if (quantity == 2)
                    {
                        Console.WriteLine("What is the width of the boxes you want?");
                        double w = double.Parse(Console.ReadLine());
                        Console.WriteLine("What is the width of the boxes you want?");
                        double h = double.Parse(Console.ReadLine());
                        Console.WriteLine("How many boxes you want?");
                        int q = int.Parse(Console.ReadLine());
                        BigSell(w,h, q);
                    }
                }
            }
        }
    } 
}
