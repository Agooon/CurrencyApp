using CurrencyAppDatabase.DataAccess;
using CurrencyAppDatabase.Models.CurrencyApp;
using CurrencyAppDatabase.Models.CurrencyApp.Connections;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyAppDatabase.DatabaseOperations
{
    public static class TableOperations
    {
        // Returning table to add items to it / assign a user
        public async static Task<ItemTable> CreateItemTable(CurrencyContext context, string name = "Default", ICollection<Item> items = null)
        {
            ItemTable newTable = new ItemTable
            {
                Items = items,
                Name = name
            };
            await context.ItemTables.AddAsync(newTable);
            await context.SaveChangesAsync();
            return newTable;
        }
        public async static Task<bool> CreateItemTable(CurrencyContext context, ItemTable newTable)
        {
            await context.ItemTables.AddAsync(newTable);
            await context.SaveChangesAsync();
            return true;
        }
        public async static Task<bool> DeleteItemTable(CurrencyContext context, ItemTable newTable)
        {
            context.ItemTables.Remove(newTable);
            await context.SaveChangesAsync();
            return true;
        }
        public async static Task<bool> AddItemToItemTable(CurrencyContext context, ItemTable table, Item item)
        {
            item.ItemTableId = table.Id;
            item.Position = table.Items.Count;
            await context.Items.AddAsync(item);
            await context.SaveChangesAsync();
            return true;
        }
        public async static Task<bool> DeleteItemFromItemTable(CurrencyContext context, Item item)
        {
            // There is no RemoveAsync
            context.Items.Remove(item);
            await context.SaveChangesAsync();
            return true;
        }
        public async static Task<bool> AddUserToItemTable(CurrencyContext context, ItemTable table, AppUser user)
        {
            UserTable newConnnectionUT = new UserTable()
            {
                Table= table,
                User = user
            };
            await context.AddAsync(newConnnectionUT);
            await context.SaveChangesAsync();
            return true;
        }

        public static ICollection<Item> ItemSort(ICollection<Item> items, string sortOption)
        {
            switch (sortOption)
            {
                case "name":
                    items = items.OrderBy(x => x.Name).ToList();
                    break;
                case "name_desc":
                    items = items.OrderByDescending(x => x.Name).ToList();
                    break;
                case "date":
                    items = items.OrderBy(x => x.Date).ToList();
                    break;
                case "date_desc":
                    items = items.OrderByDescending(x => x.Date).ToList();
                    break;
                case "price":
                    items = items.OrderBy(x => x.Price).ToList();
                    break;
                case "price_desc":
                    items = items.OrderByDescending(x => x.Price).ToList();
                    break;
                case "currency":
                    items = items.OrderBy(x => x.CurrencyFrom).ToList();
                    break;
                case "currency_desc":
                    items = items.OrderByDescending(x => x.CurrencyFrom).ToList();
                    break;
                case "rate":
                    items = items.OrderBy(x => x.Rate).ToList();
                    break;
                case "rate_desc":
                    items = items.OrderByDescending(x => x.Rate).ToList();
                    break;
                default:
                    break;
            }
            return items;
        }


        public static List<Item> GenerateTestItemList()
        {
            return new List<Item>()
            {
                new Item("xp  69",
                    DateTime.ParseExact("2020-11-19","yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo,DateTimeStyles.None),
                    "USD", (decimal)251.99, (decimal)3.7,
                    DateTime.ParseExact("2020-11-19","yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo,DateTimeStyles.None),
                    0),
                new Item(":O   1",
                    DateTime.ParseExact("2020-11-19","yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo,DateTimeStyles.None),
                    "USD", (decimal)251.99, (decimal)3.7,
                    DateTime.ParseExact("2020-11-19","yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo,DateTimeStyles.None),
                    1),
                new Item("xD   2",
                    DateTime.ParseExact("2020-11-19","yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo,DateTimeStyles.None),
                    "EUR", (decimal)22.99, (decimal)4.1,
                    DateTime.ParseExact("2020-11-19","yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo,DateTimeStyles.None),
                    2),
                new Item("xD   3",
                    DateTime.ParseExact("2020-11-18","yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo,DateTimeStyles.None),
                    "EUR", (decimal)22.99, (decimal)4.09,
                    DateTime.ParseExact("2020-11-18","yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo,DateTimeStyles.None),
                    3),
                new Item("Item 4",
                    DateTime.ParseExact("2020-11-15","yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo,DateTimeStyles.None),
                    "AUD", (decimal)100.0, (decimal)2.75,
                    DateTime.ParseExact("2020-11-13","yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo,DateTimeStyles.None),
                    4),
                    new Item("Item 5",
                    DateTime.ParseExact("2020-11-14","yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo,DateTimeStyles.None),
                    "AUD", (decimal)100.0, (decimal)2.75,
                    DateTime.ParseExact("2020-11-13","yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo,DateTimeStyles.None),
                    5),
                new Item("Item 6",
                    DateTime.ParseExact("2020-11-13","yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo,DateTimeStyles.None),
                    "AUD", (decimal)100.0, (decimal)2.75,
                    DateTime.ParseExact("2020-11-13","yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo,DateTimeStyles.None),
                    6),
                new Item("Item 7",
                    DateTime.ParseExact("2020-10-01","yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo,DateTimeStyles.None),
                    "AUD", (decimal)100.0, (decimal)2.83,
                    DateTime.ParseExact("2020-10-01","yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo,DateTimeStyles.None)
                    ,7),
                new Item("xp  69111111",
                    DateTime.ParseExact("2020-11-19","yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo,DateTimeStyles.None),
                    "USD", (decimal)251.99, (decimal)3.7,
                    DateTime.ParseExact("2020-11-19","yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo,DateTimeStyles.None),
                    8),
                new Item(":O   1222222222222",
                    DateTime.ParseExact("2020-11-19","yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo,DateTimeStyles.None),
                    "USD", (decimal)251.99, (decimal)3.7,
                    DateTime.ParseExact("2020-11-19","yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo,DateTimeStyles.None),
                    9),
                new Item("xD   2333333333333",
                    DateTime.ParseExact("2020-11-19","yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo,DateTimeStyles.None),
                    "EUR", (decimal)22.99, (decimal)4.1,
                    DateTime.ParseExact("2020-11-19","yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo,DateTimeStyles.None),
                    10),
            };
        }
    }
}
