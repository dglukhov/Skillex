using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Day1_HW
{
    class Program
    {
        static void Main(string[] args)
        {

            #region XML. Чтение

            var file = File.ReadAllText(@"d:\homework.xml"); 

            var db = XDocument.Parse(file)
                              .Descendants("WEATHER")
                              .Descendants("REPORT")
                              .Descendants("TOWN")
                              .ToList();

            #endregion

            #region Способ 1 (простой, неоптимизированный)

            string output = string.Empty;
            XElement DataBase = new XElement("DATA");
            

            for (int j = -20; j <= 20; j++)
            {
                XElement City = new XElement("City");
                int count = 0;
                XElement Temperature = new XElement("Temperature") { Value = j.ToString() };
                for (int i = 0; i < db.Count; i++)
                {
                    var item = db[i];
                    if (Int32.Parse(item.Element("TEMPERATURE").Attribute("value").Value) == j)
                     count++;
                    
                }
                XElement Count = new XElement("Count") { Value = count.ToString() };
                City.Add(Temperature, Count);
                DataBase.Add(City);

            }
            DataBase.Save(@"d:\citytemp.xml");
            #endregion

            #region Способ 2 (сложный, оптимизированный)

            var temperatures = Enumerable.Range(-20, 41);
            var byTemp = db.GroupBy(xe => xe.Element("TEMPERATURE").Attribute("value").Value);
            var map = byTemp.ToDictionary(g => int.Parse(g.Key), g => g.Count());

            var dataBase2 = new XElement("DATA");

            foreach (var ex in temperatures.Select(t =>
            {
                var city = new XElement("City");
                city.Add(
                    new XElement("Temperature") { Value = t.ToString() },
                    new XElement("Count") { Value = (map.ContainsKey(t) ? map[t] : 0).ToString() });
                return city;
            }))
            {
                dataBase2.Add(ex);
            }

            dataBase2.Save(@"d:\citytemp2.xml");

            #endregion

        }
    }
}
