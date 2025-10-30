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
        private readonly string _filePath;

        public ConfigurationManager(string filePath = "C:\\Users\\Nadja\\Desktop\\Configuration.xml")
        {
            _filePath = filePath;
        }

        // Snima sve tagove (uključujući njihove alarme) u XML fajl.
        public void SaveConfiguration(List<Tag> tags)
        {
            try
            {
                // Dodaj sve tipove koji mogu biti u konfiguraciji
                var knownTypes = new Type[]
                {
                    typeof(AnalogInput),
                    typeof(DigitalInput),
                    typeof(AnalogOutput),
                    typeof(DigitalOutput),
                    typeof(Alarm)
                };

                var serializer = new XmlSerializer(typeof(List<Tag>), knownTypes);

                // Snimi fajl
                using (var fs = new FileStream(_filePath, FileMode.Create))
                {
                    serializer.Serialize(fs, tags);
                }

                Console.WriteLine($"[INFO] Configuration saved to {_filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Error saving configuration: {ex.Message}");
            }
        }

        // Učitava konfiguraciju (tagove + alarme) iz XML fajla.
        public List<Tag> LoadConfiguration()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    Console.WriteLine("[INFO] No configuration file found. Returning empty list.");
                    return new List<Tag>();
                }

                var knownTypes = new Type[]
                {
                    typeof(AnalogInput),
                    typeof(DigitalInput),
                    typeof(AnalogOutput),
                    typeof(DigitalOutput),
                    typeof(Alarm)
                };

                var serializer = new XmlSerializer(typeof(List<Tag>), knownTypes);

                using (var fs = new FileStream(_filePath, FileMode.Open))
                {
                    return (List<Tag>)serializer.Deserialize(fs);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Error loading configuration: {ex.Message}");
                return new List<Tag>();
            }
        }
    }
}
