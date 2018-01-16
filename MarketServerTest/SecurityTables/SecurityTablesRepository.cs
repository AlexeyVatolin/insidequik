using System.Collections.Generic;
using System.Xml.Linq;

namespace MarketServerTest.SecurityTables
{
    static class SecurityTablesRepository
    {
        private static List<SecuritiesTable> securitiesTables;

        static SecurityTablesRepository()
        {
            securitiesTables = new List<SecuritiesTable>();
        }
        public static void Add(SecuritiesTable table)
        {
            securitiesTables.Add(table);
            Save();
        }

        private static void Save()
        {
            var root = new XDocument();
            foreach (var table in securitiesTables)
            {
                root.Add(table.ToXml());
            }
            root.Save("SecuritiesTables.xml");
        }

        private static void Read()
        {
            
        }

        public static XElement ToXml(this SecuritiesTable securitiesTable)
        {
            var table = new XElement("table", new XAttribute("name", securitiesTable.Name));
            foreach (var security in securitiesTable.Securities)
            {
                table.Add(new XElement("secutiry",
                    new XElement("ClassCode", security.ClassCode),
                    new XElement("SecCode", security.SecCode),
                    new XElement("Name", security.Name),
                    new XElement("ShortName", security.ShortName)));
            }
            return table;
        }
    }
}
