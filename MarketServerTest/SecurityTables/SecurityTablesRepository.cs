using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using MarketServerTest.Models;
using QuikSharp.DataStructures;

namespace MarketServerTest.SecurityTables
{
    static class SecurityTablesRepository
    {
        private static List<SecuritiesTable> securitiesTables;
        private static string fileName = "SecuritiesTables.xml";

        static SecurityTablesRepository()
        {
            securitiesTables = new List<SecuritiesTable>();
            Read();
        }
        public static void Add(SecuritiesTable table)
        {
            securitiesTables.Add(table);
            Save();
        }

        public static List<TradeTableModel> GetSecuritiesTablesInfos()
        {
            var result = new List<TradeTableModel>();
            foreach (var table in securitiesTables)
            {
                result.Add(new TradeTableModel
                {
                    Id = table.Id,
                    Name = table.Name
                });
            }
            return result;
        }

        public static bool IsAllowedName(string name)
        {
            return !securitiesTables.Exists(item => item.Name == name);
        }

        public static List<SecurityInfo> GetSecuritiesById(string id)
        {
            return securitiesTables.FirstOrDefault(item => item.Id == id)?.Securities;
        }

        public static void DeleteById(string id)
        {
            securitiesTables.Remove(securitiesTables.First(item => item.Id == id));
            Save();
        }

        private static void Save()
        {
            var root = new XDocument();
            var tables = new XElement("tables");
            root.Add(tables);
            foreach (var table in securitiesTables)
            {
                tables.Add(table.ToXml());
            }
            root.Save(fileName);
        }

        private static void Read()
        {
            if (File.Exists(fileName))
            {
                XDocument doc = XDocument.Load(fileName);
                if (doc.Root != null && doc.Root.HasElements)
                {
                    foreach (XElement element in doc.Root.Elements())
                    {
                        var securitiesTable = new SecuritiesTable();
                        securitiesTable.Name = element.Attribute("name").Value;
                        securitiesTable.Id = element.Attribute("id").Value;
                        foreach (var secutiry in element.Elements())
                        {
                            securitiesTable.Securities.Add(new SecurityInfo()
                            {
                                ClassCode = secutiry.Element("ClassCode").Value,
                                SecCode = secutiry.Element("SecCode").Value,
                                Name = secutiry.Element("Name").Value,
                                ShortName = secutiry.Element("ShortName").Value
                            });
                        }
                        securitiesTables.Add(securitiesTable);
                    }
                }
            }
        }

        private static XElement ToXml(this SecuritiesTable securitiesTable)
        {
            var table = new XElement("table", 
                new XAttribute("name", securitiesTable.Name), 
                new XAttribute("id", securitiesTable.Id)); 
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
