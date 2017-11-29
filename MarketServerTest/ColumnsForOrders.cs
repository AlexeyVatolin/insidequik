using QuikSharp.DataStructures.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketServerTest
{
    internal class ColumnsForOrders
    {
        public string Company { get; set; }
        public string ClassCode { get; set; }
        public string Operation { get; set; }
        public string Quantity { get; set; }
        public string Price { get; set; }
        public string Time { get; set; }
        public string Balance { get; set; }
        public string Value { get; set; }
        public string State { get; set; }

        public ColumnsForOrders(Order item)
        { 
            Company = item.SecCode;
            ClassCode = item.ClassCode;
            Operation = item.Operation.ToString();
            Quantity = item.Quantity.ToString();
            Price = item.Price.ToString();
            Time = item.Datetime.hour.ToString() + ":" + item.Datetime.min.ToString() + ":" + item.Datetime.sec.ToString();
            Balance = item.Balance.ToString();
            Value = item.Value.ToString();
            State = item.State.ToString();
        }
    }
}
