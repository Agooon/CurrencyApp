using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyApp.Models.Application.ItemTable
{
    public class SingleRateModel
    {
        public string Table { get; set; }
        public string Currency { get; set; }
        public string Code { get; set; }
        public Rate[] Rates { get; set; }
    }

    public class Rate
    {
        public string No { get; set; }
        public string EffectiveDate { get; set; }
        public float Mid { get; set; }
    }

}
