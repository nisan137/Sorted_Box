using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Sorted_Box
{
    /// <summary>
    /// delegate to deal with getting the expired boxes out when a day change.
    /// </summary>
    /// <param name="today"></param>
    /// <param name="args"></param>
    public delegate void AddDayEventHandler(int today, EventArgs args);
  
    public struct DATE
    {
        public static event AddDayEventHandler AddDayEvent;
        public static int today = 0;
        /// <summary>
        /// Add day in the system.
        /// And when a day changes, 
        /// the function goes through the inventory in the store 
        /// and checks which products have expired
        /// </summary>
        public static void AddDay()
        {
            today++;
            AddDayEvent?.Invoke(today, EventArgs.Empty);
        }
    }

}
