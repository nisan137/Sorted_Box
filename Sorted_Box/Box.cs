using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sorted_Box
{
    public class Box 
    {
        /// <summary>
        /// Represents the box base
        /// </summary>
        private double _bottom;
        /// <summary>
        /// Represents the box height
        /// </summary>
        private double _height;
        /// <summary>
        /// Represents the box amount
        /// </summary>
        private int _amount;
        /// <summary>
        /// Represents the max days that box can stay in the store.
        /// </summary>
        public const int maxDays = 5;
        /// <summary>
        /// Represents the max amount of boxes that the customer can buy.
        /// </summary>
        public const int maxAmount = 50;

        /// <summary>
        ///  Represents a pointer to a box. Improves the running time significantly from O(n) to O(1)
        /// </summary>
        public LinkedListNode<Box> NodePointer { get; set; }

        /// <summary>
        /// The last time A purchase has been made or the adding date.
        /// </summary>
        public int Date{ get; set; }
        public double Bottom
        {
            get { return _bottom; }
            set
            {
                if (value > 0) // Does not allow entering a value that is zero or less for the size of a box
                    _bottom = value;
                else
                    throw new InvalidOperationException();
            }
        }
        public double Height
        {
            get { return _height; }
            set
            {
                if (value > 0)// Does not allow entering a value that is zero or less for the size of a box
                    _height = value;
                else
                    throw new InvalidOperationException();
            }
        }
        public int Amount
        {
            get { return _amount; }
            set
            {
                if (value > maxAmount)// Saves the boxes that the customer requested, which exceeded the upper limit of the number of boxes that can be sold
                {
                    var amountReturned = value - maxAmount;
                    _amount = maxAmount;
                    throw new AmountOutOfRangeException($"The max amount is: 50\nReturned to the deliver: {amountReturned} boxes");
                }
                _amount = value;
            }
        }
        public Box(double bottom, double height, int amount)
        {
            Bottom = bottom;
            Height = height;
            Amount = amount;
            Date = DATE.today;
        }
        public override string ToString()
        {
            return $"bottom: {Bottom}.  height: {Height}.  amount: {Amount}.  Date: {Date}.";
        }
    }
}
