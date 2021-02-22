using System;
using System.Globalization;
using System.Linq;

namespace CurrencyApp.Backend
{
    public static class UtilFunctions
    {
        public static bool IsValidDate(string value, string dateFormat)
        {
            return DateTime.TryParseExact(value, dateFormat, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out _);
        }
        public static void PreviousDay(ref string value, string dateFormat)
        {
            DateTime newDate;
            DateTime.TryParseExact(value, dateFormat, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out newDate);
            value = newDate.AddDays(-1).ToString(dateFormat);
        }
        public static bool IsSorted(int[] arr)
        {
            for (int i = 0; i < arr.Length - 1; i++)
            {
                if (arr[i] > arr[i + 1])
                    return false;
            }
            return true;
        }

        public static string IsStringOfItemTable(string table)
        {
            if (string.IsNullOrWhiteSpace(table))
            {
                return "Podano pusty ciąg znaków";
            }
            string[] rows = table.Split("\r\n");

            string[] words = rows[0].Split("\t");

            if (words.Count() != 4)
                return "Nie zgadza się liczba wymaganych nazw kolumn.</br>Powinno być <b>4</b> a jest <b class='text-danger'>" + words[0].Count() + "</b>";
            int counter = 2;
            foreach (var row in rows.Skip(1))
            {
                words = row.Split("\t");
                if (words.Count() != 4)
                    return "Nie zgadza się liczba wymaganych argumentach w linijce <b>" + counter + " </b>.</br>Powinno być <b>4</b> a jest <b class='text-danger'>" + words[0].Count() + "</b>";

                // Checking the date
                try
                {
                    string.IsNullOrWhiteSpace(DateTime.Parse(words[1]).ToString("yyyy-MM-dd"));
                }
                catch
                {
                    return "Niepoprawny format daty w linijce <b>" + counter + "</b> </br> Najlepiej użyć formatu <b>DD-MM-RRRR, DD.MM.RRRR, RRRR.MM.DD, RRRR-MM-DD</b>";
                }
                try
                {
                    decimal.Parse(words[2]).ToString();
                }
                catch
                {
                    return "Niepoprawny format ceny w linijce <b>" + counter + "</b>";
                }
                // Checking the price
                if (decimal.Parse(words[2]).ToString() != words[2])
                    return "Niepoprawny format ceny w linijce <b>" + counter + "</b>";

                // Checking the currency
                if (words[3].Length != 3)
                    return "Niepoprawny format waluty w linijce <b>" + counter + "</b>";

                counter++;
            }

            return "SUCCESS";
        }
    }
}
