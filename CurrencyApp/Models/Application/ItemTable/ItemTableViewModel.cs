using CurrencyApp.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyApp.Models.Application
{
    public class ItemTableViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ItemViewModel> Items { get; set; }
        public virtual ICollection<UserData> Users { get; set; }
    }
}
