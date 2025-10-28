using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace DataConcentrator
{
    public class ConfigurationManager
    {
        private string _filePath;

        public ConfigurationManager(string filePath = "Configuration.xml")
        {
            _filePath = filePath;
        }

        // snimanje svih tagova u XML fajl
        public void SaveConfiguration(List<Tag> tags)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Tag>));
                using (FileStream fs = new FileStream(_filePath, FileMode.Create))
                {
                    serializer.Serialize(fs, tags);
                }
                Console.WriteLine($"Configuration saved to {_filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving configuration: " + ex.Message);
            }
        }

        // upis u hml
        public List<Tag> LoadConfiguration()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    Console.WriteLine("No configuration file found. Returning empty list.");
                    return new List<Tag>();
                }

                XmlSerializer serializer = new XmlSerializer(typeof(List<Tag>));
                using (FileStream fs = new FileStream(_filePath, FileMode.Open))
                {
                    return (List<Tag>)serializer.Deserialize(fs);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading configuration: " + ex.Message);
                return new List<Tag>();
            }
        }
    }
}
