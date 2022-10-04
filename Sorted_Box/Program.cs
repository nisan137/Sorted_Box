using System;
using System.Collections.Generic;

namespace Sorted_Box
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Manager manager = new Manager();
            DATE.AddDayEvent += (o, e) => manager.RemoveOldBox(); // activet the func that removes all the boxes that expaired.

            Console.WriteLine($"Hello, welcome to the store, how can I help you today?");
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine();
                Console.WriteLine($"Please select [1-5]\tToday: {DATE.today}\n" +
                $"1. Buy box\n" +
                $"2. Add a new box to the stock\n" +
                $"3. (manager) Add day\n" +
                $"4. (manager) Show stock\n");
                int chosenReq = DoBuyerRequest();
                Console.ForegroundColor = ConsoleColor.Cyan;
                try
                {
                    switch (chosenReq)
                    {
                        case 1:
                            Console.WriteLine("Please enter the base size and height size and amount of the box/es that you would like to buy");
                            Console.WriteLine("Base size: ");
                            double baseSize = double.Parse(Console.ReadLine());
                            Console.WriteLine("Height size: ");
                            double heightSize = double.Parse(Console.ReadLine());
                            Console.WriteLine("Amount: ");
                            int amountReq = int.Parse(Console.ReadLine());
                            Dictionary<Box, int> dic = manager.FindMatchBox(baseSize, heightSize, amountReq);

                            foreach (var item in dic)
                            {
                                Console.WriteLine(item);
                            }
                            if (dic.Count > 0)
                            {
                                while (true)
                                {
                                    Console.WriteLine("Would you like to continue the purchase?");
                                    Console.WriteLine("y / n");
                                    string cusAns = Console.ReadLine().ToLower();
                                    if (cusAns == "y")
                                    {
                                        Console.WriteLine(manager.BuyBox(dic));
                                        break;
                                    }
                                    else if (cusAns == "n")
                                    {
                                        Console.WriteLine("Thanks, have a nice day");
                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid input, please try again");
                                    }
                                }
                            }
                            else
                                Console.WriteLine("I don't have that size of box");
                            break;
                        case 2:
                            Console.WriteLine("Please enter the base size and height size and amount of the box that you would like to add");
                            Console.WriteLine("Base size: ");
                            double baseReq = double.Parse(Console.ReadLine());
                            Console.WriteLine("Height size: ");
                            double heightReq = double.Parse(Console.ReadLine());
                            Console.WriteLine("Amount: ");
                            int amountRequ = int.Parse(Console.ReadLine());
                            manager.AddBox(new Box(baseReq, heightReq, amountRequ));
                            break;
                        case 3:
                            DATE.AddDay();
                            Console.WriteLine("\n1 day is added");
                            break;
                        case 4:
                            manager.ActionOnAllBoxes(Console.WriteLine);
                            break;
                        default:
                            break;
                    }
                }
                catch (AmountOutOfRangeException e)
                {
                    Console.WriteLine(e.Message);
                }
                catch(FormatException)
                {
                    Console.WriteLine("Start Over again");
                }
            }
        }

        public static int DoBuyerRequest()
        {
            int ChosenSelection;
            while (!int.TryParse(Console.ReadLine(), out ChosenSelection) || ChosenSelection < 1 || ChosenSelection > 5)
            {
                Console.WriteLine("Invalid selection, Please select number between [1 - 5] ");
            }
            return ChosenSelection;
        }
    }
}
