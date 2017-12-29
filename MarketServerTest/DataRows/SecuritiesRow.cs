using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketServerTest.DataRows
{
    public class SecuritiesRow
    {
        public string ClassCode { get; set; }
        public string SecCode { get; set; }
        public string Name { get; set; }
        public double LastPrice { get; set; }
        public double ChangePercent { get; set; }
        public double Flow { get; set; }
    }
}
