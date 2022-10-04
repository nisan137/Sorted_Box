using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;

namespace Sorted_Box
{
    public class Manager
    {
        /// <summary>
        /// I created a BST inside of a BST
        /// the first BST is to be able to find the box by the bottom.
        /// and the inside BST checks if the box contains a specific height.
        /// O(log n)
        /// </summary>
        public SortedDictionary<double, SortedDictionary<double, Box>> tree = new SortedDictionary<double, SortedDictionary<double, Box>>();

        /// <summary>
        /// The structure stores the reference of boxes,
        /// for the purpose of keeping and arranging on:
        /// the last date the box entered the inventory, or was purchased.
        /// 
        /// The (linkedListDate)
        /// The structure stores the reference of boxes,
        /// for the purpose of keeping and arranging on:
        /// the last date the box entered the inventory, or was purchased.
        /// I maintain the data structure
        /// in that when a box is purchased \ goes into stock,
        /// with the help of a function (BuyBox),
        /// I return it to the end of the list,
        /// and boxes that are about to expire are pushed to the beginning of the list.
        /// For every purchase that is made,
        /// if the box has not been sold for X time,
        /// and when X= MAX_DATE the box is removed from the stock,
        /// or if there is still stock of its type,
        /// then the quantity only decreases.
        /// The date is updated to the current date either way.
        /// </summary>
        public LinkedList<Box> linkedListDate = new LinkedList<Box>();
        public const double PERCENTAGE = 1.25;

        /// <summary>
        /// if the tree contains the current bottom, 
        /// so just add the new box inside of the relevant correct bottom dictionary.
        /// else creat a new node and push the data inside, add the .
        /// O(log(N))
        /// </summary>
        /// <param name="b"><see cref="Box"/></param>
        public void AddBox(Box b) 
        {
            if (b == null)
                throw new ArgumentNullException("The box have no values");

            if (tree.ContainsKey(b.Bottom))
            {
                if (tree[b.Bottom].ContainsKey(b.Height))
                {
                    tree[b.Bottom][b.Height].Amount += b.Amount;
                    linkedListDate.Remove(tree[b.Bottom][b.Height].NodePointer); // remove from the linked list of my data inventory.
                    tree[b.Bottom][b.Height].Date = DATE.today; // Resets it to the current date
                    tree[b.Bottom][b.Height].NodePointer = linkedListDate.AddLast(tree[b.Bottom][b.Height]); // add the removed box to last place in the linked list.
                }
                else
                {
                    tree[b.Bottom].Add(b.Height, b);  // add new box to the iner tree
                    b.NodePointer = linkedListDate.AddLast(b); // add the new box to last place in the linked list of my data inventory.
                }
            }
            else
            {
                tree.Add(b.Bottom, new SortedDictionary<double, Box>()); // add bottom and SortedDictionary object to the tree
                tree[b.Bottom].Add(b.Height, b); // add new box to the iner tree
                b.NodePointer = linkedListDate.AddLast(b); // add the new box to last place in the linked list of my data inventory.
            }
        }
        /// <summary>
        /// Remove the boxes nodes that they date passed the max days.
        /// O(1)
        /// </summary>
        public void RemoveOldBox() 
        {
            while (true)
            {
                if (linkedListDate.Count <= 0) break; 
                Box b = linkedListDate.First.Value;
                if (DATE.today - b.Date >= Box.maxDays) // chacks if the date today
                {
                    tree[b.Bottom].Remove(b.Height);   // remove the boxes from the tree.
                    linkedListDate.RemoveFirst();     // remove the Node from the linkedList.
                }
                else break;
            }
        }
        /// <summary>
        /// Find the match Boxes to the customer, and put them in the Dictionary,
        /// to be able manage the requested boxes from the customer.
        /// </summary>
        /// <param name="bottom">The base of the box</param>
        /// <param name="height">The height of the box</param>
        /// <param name="amount">The amount of the boxes</param>
        /// <returns></returns>
        /// <exception cref="AmountOutOfRangeException"></exception>
        public Dictionary<Box, int> FindMatchBox(double bottom, double height, int amount = 1)
        {
            Dictionary<Box, int> Data = new Dictionary<Box, int>();
            if (bottom <= 0 || height <= 0)
            {
                throw new AmountOutOfRangeException("The size can not be 0 or less");
            }
            foreach (var x in tree.Keys) // run on the base size of the tree
            {
                if (x < bottom) continue; // if the requested base is small then our base, continue the loop.
                if (x <= (bottom * PERCENTAGE) && amount > 0) 
                {
                    foreach (var y in tree[x].Keys)
                    {
                        if (y < height) continue; // if the requested height is small then our height, continue the loop.
                        if (y <= (height * PERCENTAGE) && amount > 0)
                        {
                            int count = 0;
                            for (int i = 0; i < tree[x][y].Amount && amount > 0; i++)
                            {
                                amount--; // discount the amount of the specific box. the amount of boxes that the customer ask for.
                                count++; // how many boxes to buy
                            }
                            Data.Add(tree[x][y], count); // add the specific box and the count (how many boxes to buy).
                        }
                        else break;
                    }
                }
                else break;
            }
            return Data;
        }
        /// <summary>
        /// The function removes the box from the list of dates.
        /// If this is the last box that was sold, 
        /// then it is also removed from the tree, and message is printed,
        /// otherwise the purchased quantity is subtracted from it,
        /// the date of the box is updated,
        /// it is added to the end of the list of dates,
        /// and the values of the sold box are printed.
        /// </summary>
        /// <param name="Data">// Key.Amount = count = how many boxes to buy</param>
        /// <returns>string</returns>
        public string BuyBox(Dictionary<Box, int> Data) 
        {
            string s = "";
            foreach (var item in Data)
            {
                linkedListDate.Remove(item.Key.NodePointer); // Deletes the box from the list of dates. the location is here because it happens anyway (if and else)
                if (item.Key.Amount - item.Value <= 0) // The number of boxes minus the number of boxes that can be bought <= 0
                {
                    tree[item.Key.Bottom].Remove(item.Key.Height);// Approach the tree[base] and removes the box from the tree. 
                    s += $"Box: ({item.Key.Bottom} ,{item.Key.Height}) - The last one is sold. removing from stock\n";
                }
                else
                {
                    item.Key.NodePointer = linkedListDate.AddLast(item.Key);
                    item.Key.Amount -= item.Value;// discount the amount from the box.amount.
                    item.Key.Date = DATE.today; // update the last purchas date of the box to the current day.
                    s += $"Sold: {item.Key}";
                }
            }
            return s;
        }
        /// <summary>
        /// runs on both trees and does different actions on every junction.
        /// </summary>
        /// <param name="action"></param>
        public void ActionOnAllBoxes(Action<Box> action)
        {
            foreach (var innerTree in tree)
            {
                foreach (var item in innerTree.Value)
                {
                    action?.Invoke(item.Value);
                }
            }
        }
    }
}