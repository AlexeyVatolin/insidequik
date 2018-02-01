using QuikSharp.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Common.Models
{
    public class ClientOrder
    {
        public string SecurityCode { get; set; }
        public string ClassCode { get; set; }
        public Operation Operation { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }    
        public bool MarketPrice { get; set; }

    }
}
