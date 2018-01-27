using System;
using System.Collections.Generic;
using QuikSharp.DataStructures;

namespace MarketServerTest.SecurityTables
{
    class SecuritiesTable
    {
        public string Name { get; set; }
        public string Id { get; set; } 
        public List<SecurityInfo> Securities { get; set; }

        public SecuritiesTable()
        {
            Name = string.Empty;
            Id = Guid.NewGuid().ToString("N"); //генерирует уникальный идентификатор таблицы
            Securities = new List<SecurityInfo>();
        }

        public SecuritiesTable(string name, List<SecurityInfo> securities)
        {
            Name = name;
            Id = Guid.NewGuid().ToString("N");
            Securities = securities;
        }
    }
}
