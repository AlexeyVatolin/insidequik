using System.Collections.Generic;
using QuikSharp.DataStructures;

namespace MarketServerTest.SecurityTables
{
    class SecuritiesTable
    {
        public string Name { get; set; }
        public List<SecurityInfo> Securities { get; set; }

        public SecuritiesTable(string name, List<SecurityInfo> securities)
        {
            Name = name;
            Securities = securities;
        }
    }
}
