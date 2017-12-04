using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuikSharp.DataStructures.Transaction;

namespace MarketServerTest
{
    internal class ColumnsForTrades
    {
        public string Time { get; set; }
        public string Company { get; set; }
        public string Quantity { get; set; }
        public string Value { get; set; }
        public string Date { get; set; }
        public string Opeartion { get; set; }

        public ColumnsForTrades(Trade trade, Order order)
        {
            var date = order.Datetime.hour.ToString("00") + ":" + order.Datetime.min.ToString("00")
                       + ":" + order.Datetime.sec.ToString("00") + "." + order.Datetime.ms;
            Company = trade.SecCode; //инструмент
            Value = trade.Value.ToString(CultureInfo.InvariantCulture); //объем в денежных единицах
            Quantity = trade.Quantity.ToString(); // предположительно объем бумаг
            Date = date; // дата сделки
            Time = trade.Period.ToString(); // время в милисекундах
            Opeartion = order.Operation.ToString(); // покупка/продажа
        }
    }
}