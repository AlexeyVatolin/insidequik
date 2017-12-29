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
        public string Operation { get; set; }
        public string StopPrice { get; set; }
        public string StopLimPrice { get; set; }
        public string Quantity { get; set; }
        public string ActQuantity { get; set; }
        public string ExecQuantity { get; set; }
        public string State { get; set; }
        public ColumnsForStopOrders(StopOrder stopOrder)
        {
            Company = stopOrder.SecCode;
            Operation = stopOrder.Operation.ToString();
            StopPrice = stopOrder.ConditionPrice.ToString();
            StopLimPrice = stopOrder.Price.ToString();
            Quantity = stopOrder.Quantity.ToString();
            ActQuantity = (stopOrder.Quantity - stopOrder.FilledQuantity).ToString();
            ExecQuantity = stopOrder.FilledQuantity.ToString();
            State = stopOrder.State.ToString();
        }
    }
}
