using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataConcentrator
{
    public class DataConcentrator
    {
        private readonly Dictionary<string, Tag> _tags = new Dictionary<string, Tag>();

        // Dodavanje novog taga
        public void AddTag(Tag tag)
        {
            if (_tags.ContainsKey(tag.TagName))
            {
                Console.WriteLine($"[WARNING] Tag '{tag.TagName}' already exists!");
                return;
            }

            _tags[tag.TagName] = tag;
            Console.WriteLine($"[INFO] Added tag: {tag.TagName}");
        }

        // remove
        public void RemoveTag(string tagName)
        {
            if (_tags.Remove(tagName))
                Console.WriteLine($"[INFO] Removed tag: {tagName}");
            else
                Console.WriteLine($"[WARNING] Tag '{tagName}' not found!");
        }

        // read tag
        public double ReadValue(string tagName)
        {
            if (_tags.TryGetValue(tagName, out var tag))
            {
                return tag.ReadValue();
            }

            Console.WriteLine($"[WARNING] Tag '{tagName}' not found!");
            return double.NaN;
        }

        // upis vrednosti u izlazne tagove
        public void WriteValue(string tagName, double value)
        {
            if (_tags.TryGetValue(tagName, out var tag))
            {
                if (tag is AnalogInput ai)
                {
                    ai.WriteValue(value);
                }
                else if (tag is AnalogOutput ao)
                {
                    ao.WriteValue(value);
                }
                else if (tag is DigitalOutput doTag)
                {
                    doTag.WriteValue(value);
                }
                else
                {
                    Console.WriteLine($"[INFO] Cannot write to tag type: {tag.GetType().Name}");
                }
            }
            else
            {
                Console.WriteLine($"[WARNING] Tag '{tagName}' not found!");
            }
        }

        // prikaz tagova
        public void ListTags()
        {
            Console.WriteLine("\n--- TAGS IN SYSTEM ---");
            foreach (var tag in _tags.Values)
            {
                Console.WriteLine(tag);
            }
            Console.WriteLine("----------------------\n");
        }

        // Metoda za proveru svih alarma (npr. kad se koristi u petlji)
        public void CheckAllAlarms()
        {
            foreach (var tag in _tags.Values.OfType<AnalogInput>())
            {
                foreach (var alarm in tag.Alarms)
                {
                    alarm.CheckActivation(tag.ReadValue());
                }
            }
        }
    }
}

