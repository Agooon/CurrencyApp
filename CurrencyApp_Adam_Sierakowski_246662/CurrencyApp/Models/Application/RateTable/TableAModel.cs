using System.Collections.Generic;

namespace CurrencyApp.Models
{
    public class TableAModel
    {
        public string Table { get; set; }
        public string No { get; set; }
        public string EffectiveDate { get; set; }
        public List<RateModel> Rates { get; set; }
    }
}
