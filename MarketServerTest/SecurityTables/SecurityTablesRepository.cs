using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using MarketServerTest.ViewModels;
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

        public static List<TradesTableViewModel> GetSecuritiesTablesInfos()
        {
            var result = new List<TradesTableViewModel>();
            foreach (var table in securitiesTables)
            {
                result.Add(new TradesTableViewModel(table.Id, table.Name));
            }
            return result;
        }

        public static List<SecurityInfo> GetSecuritiesById(string id)
        {
            return securitiesTables.FirstOrDefault(item => item.Id == id)?.Securities;
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
