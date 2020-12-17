using CurrenycAppDatabase.Models.CurrencyApp;

namespace CurrencyAppDatabase.Models.CurrencyApp.Connections
{
    public class UserTable
    {
        public int UserId { get; set; }
        public AppUser User { get; set; }
        public int TableId { get; set; }
        public ItemTable Table { get; set; }
    }
}
