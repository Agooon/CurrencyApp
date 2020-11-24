namespace CurrenycAppDatabase.Models.CurrencyApp
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Date { get; set; }
        public int Position { get; set; }
        public decimal Price { get; set; }
        public string CurrencyFrom { get; set; }
        public decimal Rate { get; set; }
        public string DateTable { get; set; }
        public decimal ConvertedPrice { get; set; }
        public string CurrencyTo { get; set; }
    }
}
