using MarketServerTest.Helpers;
using MarketServerTest.SignalR;
using QuikSharp.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MarketServerTest.ViewModels
{
    public class SendOrderViewModel : SendOrderClientBase
    {
        public string securityCode { get; set; } = "SBER";
        public string classCode { get; set; } = "TQBR";
        public bool operationBuy { get; set; } = true;
        //public bool operationSell{ get; set; } = false;
        //public Operation operation { get; set; } 
        public decimal price { get; set; } = 260;
        public decimal price2 { get; set; } = 240;
        public int quantity { get; set; } = 5;
        public bool marketPrice { get; set; }

        public ICommand SendOrderToServer
        {
            get
            {
                return new RelayCommand(async (obj) => await SendOrder(new Common.Models.ClientOrder
                {
                    SecurityCode = securityCode,
                    ClassCode = classCode,
                    Operation = operationBuy?Operation.Buy:Operation.Sell,
                    Price = price,
                    Quantity = quantity,
                    MarketPrice = marketPrice
                }));

            }
        }

        public ICommand SendStopOrderToServer
        {
            get
            {
                return new RelayCommand(async (obj) => await SendStopOrder(new Common.Models.ClientStopOrder
                {
                    SecurityCode = securityCode,
                    Operation = operationBuy ? Operation.Buy : Operation.Sell,
                    Price = price,
                    Price2=price2,
                    Quantity = quantity
                }));

            }
        }
    }
}
