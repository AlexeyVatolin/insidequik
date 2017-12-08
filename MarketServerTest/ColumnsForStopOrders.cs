using QuikSharp.DataStructures;
using QuikSharp.DataStructures.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketServerTest
{
    class ColumnsForStopOrders
    {
        public string Company { get; set; }
        public string StopPrice { get; set; }
        public string StopLimPrice { get; set; }
        public string Quantity { get; set; }
        public string ActQuantity { get; set; }
        public string ExecQuantity { get; set; }
        public ColumnsForStopOrders(StopOrder stopOrder)
        {
            Company = stopOrder.SecCode;
            StopPrice = stopOrder.ConditionPrice.ToString();
            StopLimPrice = stopOrder.ConditionPrice2.ToString();
            Quantity = stopOrder.Quantity.ToString();
            ActQuantity = stopOrder.FilledQuantity.ToString();
           // ExecQuantity = stopOrder.
        }
    }
}
