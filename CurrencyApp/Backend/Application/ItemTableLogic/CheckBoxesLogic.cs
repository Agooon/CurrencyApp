using CurrencyApp.Models.Application.ItemTable;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyApp.Backend.Application.ItemTableLogic
{
    public static class CheckBoxesLogic
    {
        public static void SetCheckboxes(HttpRequest Request, CheckBoxesViewModel checkVM)
        {
            // Geting checkboxes from cookies
            if (Request.Cookies[checkVM.coockieName] != null)
            {
                var checkBoxes = JsonConvert.DeserializeObject<bool[]>(Request.Cookies["CheckboxesCookie"]);
                checkVM.NameCheck = checkBoxes[0];
                checkVM.DateCheck = checkBoxes[1];
                checkVM.PriceCheck = checkBoxes[2];
                checkVM.CurrencyFCheck = checkBoxes[3];
                checkVM.RateCheck = checkBoxes[4];
                checkVM.PriceConCheck = checkBoxes[5];
                checkVM.DateTableCheck = checkBoxes[6];
                checkVM.CurrencyTCheck = checkBoxes[7];
            }
        }
        // Save to cookies
        public static void SaveCheckBoxes(HttpResponse Response, CheckBoxesViewModel checkVM)
        {
            bool[] checkBoxes = new bool[8];
            checkBoxes[0] = checkVM.NameCheck;
            checkBoxes[1] = checkVM.DateCheck;
            checkBoxes[2] = checkVM.PriceCheck;
            checkBoxes[3] = checkVM.CurrencyFCheck;
            checkBoxes[4] = checkVM.RateCheck;
            checkBoxes[5] = checkVM.PriceConCheck;
            checkBoxes[6] = checkVM.DateTableCheck;
            checkBoxes[7] = checkVM.CurrencyTCheck;
            // Setting the values of checkboxes, async
            CookieOptions options = new CookieOptions()
            {
                Expires = DateTimeOffset.MaxValue
            };
            string itemsString = JsonConvert.SerializeObject(checkBoxes);
            Response.Cookies.Append(checkVM.coockieName, itemsString, options);
        }
    }
}
