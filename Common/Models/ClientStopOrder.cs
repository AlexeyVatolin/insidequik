using QuikSharp.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class ClientStopOrder
    {
        public string SecurityCode { get; set; }
        public Operation Operation { get; set; }
        public decimal Price { get; set; }
        public decimal Price2 { get; set; }
        public int Quantity { get; set; }
    }
}
