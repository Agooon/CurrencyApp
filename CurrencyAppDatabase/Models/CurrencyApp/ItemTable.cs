using CurrencyAppDatabase.Models.CurrencyApp.Connections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurrencyAppDatabase.Models.CurrencyApp
{
    public class ItemTable
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName ="VARCHAR(256)")]
        public string Name { get; set; }

        public virtual ICollection<Item> Items { get; set; }
        public virtual ICollection<UserTable> Users { get; set; }
    }
}
