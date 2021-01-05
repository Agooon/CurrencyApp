using CurrencyApp.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyApp.Backend
{
    public static class StaticFunctions
    {
        public static bool IsValidDate(string value, string dateFormat)
        {
            return DateTime.TryParseExact(value, dateFormat, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out _);
        }
        public static void PreviousDay(ref string value, string dateFormat)
        {
            DateTime newDate;
            DateTime.TryParseExact(value, dateFormat, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out newDate);
            value = newDate.AddDays(-1).ToString(dateFormat);
        }
        public static bool IsSorted(int [] arr)
        {
            for(int i = 0; i < arr.Length-1; i++)
            {
                if (arr[i] > arr[i + 1])
                    return false;
            }
            return true;
        }
       
    }
}
