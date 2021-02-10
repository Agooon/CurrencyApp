namespace CurrencyApp.Models.Application.ItemTable
{
    public class CheckBoxesViewModel
    {
        public readonly string coockieName = "CheckboxesCookie";
        public bool NameCheck { get; set; } = false; // 1

        public bool DateCheck { get; set; } = false;// 2

        public bool PriceCheck { get; set; } = false;// 3

        public bool CurrencyFCheck { get; set; } = false;// 4

        public bool RateCheck { get; set; } = false;// 5

        public bool PriceConCheck { get; set; } = false;// 6

        public bool DateTableCheck { get; set; } = false;// 7

        public bool CurrencyTCheck { get; set; } = false;// 8

    }
}
