using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLCSimulator;
using System.Timers;


namespace DataConcentrator
{
    public class DataConcentrator
    {
        private readonly Dictionary<string, Tag> _tags = new Dictionary<string, Tag>();
        private readonly PLCSimulatorManager plcVar;
        private Timer scanTimer;


        public DataConcentrator()
        {
            plcVar = new PLCSimulatorManager();
            plcVar.StartPLCSimulator();
            // Console.WriteLine("[INFO] PLC Simulator started.");
        }

        // Dodavanje novog taga
        public void AddTag(Tag tag)
        {
            if (_tags.ContainsKey(tag.TagName))
            {
              //  Console.WriteLine($"[WARNING] Tag '{tag.TagName}' already exists!");
                return;
            }

            _tags[tag.TagName] = tag;
           // Console.WriteLine($"[INFO] Added tag: {tag.TagName}");
        }

        // remove
        public void RemoveTag(string tagName)
        {
            if (_tags.Remove(tagName))
              Console.WriteLine($"[INFO] Removed tag: {tagName}");
            else
               Console.WriteLine($"[WARNING] Tag '{tagName}' not found!");
        }

        // Citanje trenutne vrednosti taga
        public double ReadValue(string tagName)
        {
            if (_tags.TryGetValue(tagName, out var tag))
            {
                // ako je ulazni tag, čitamo iz PLC simulatora
                if (tag is AnalogInput ai)
                {
                    var val = plcVar.GetAnalogValue(ai.IOAddress);
                    ai.WriteValue(val); // update i proveri alarme
                    return val;
                }
                else if (tag is DigitalInput di)
                {
                    var val = plcVar.GetAnalogValue(di.IOAddress);
                    di.UpdateValue(val != 0);
                    return val;
                }
                else
                {
                    return tag.ReadValue();
                }
            }

          //  Console.WriteLine($"[WARNING] Tag '{tagName}' not found!");
            return double.NaN;
        }

        // Upis vrednosti u izlazne tagove
        public void WriteValue(string tagName, double value)
        {
            if (_tags.TryGetValue(tagName, out var tag))
            {
                if (tag is AnalogOutput ao)
                {
                    ao.WriteValue(value);
                    plcVar.SetAnalogValue(ao.IOAddress, value);
                }
                else if (tag is DigitalOutput doTag)
                {
                    doTag.WriteValue(value);
                    plcVar.SetDigitalValue(doTag.IOAddress, value);
                }
                else
                {
                   // Console.WriteLine($"[INFO] Cannot write to tag type: {tag.GetType().Name}");
                }
            }
            else
            {
              //  Console.WriteLine($"[WARNING] Tag '{tagName}' not found!");
            }
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
        public void StartScanning(int intervalMs = 1000)
        {
            scanTimer = new Timer(intervalMs);
            scanTimer.Elapsed += (s, e) => ScanTags();
            scanTimer.AutoReset = true;
            scanTimer.Start();
          //  Console.WriteLine($"[INFO] DataConcentrator scanning started (interval {intervalMs} ms).");
        }

        public void StopScanning()
        {
            scanTimer?.Stop();
          //  Console.WriteLine("[INFO] DataConcentrator scanning stopped.");
        }

        private void ScanTags()
        {
            foreach (var tag in _tags.Values)
            {
                if (tag is AnalogInput ai && ai.OnOffScan)
                {
                    double newValue = plcVar.GetAnalogValue(ai.IOAddress);
                    ai.WriteValue(newValue);        // koristi metodu koja ažurira vrednost i proverava alarme
                }
                else if (tag is DigitalInput di && di.OnOffScan)
                {
                    double newValue = plcVar.GetAnalogValue(di.IOAddress);
                    di.UpdateValue(newValue != 0);  // koristi metodu koja postavlja true/false
                }
            }
        }

        // get za tagove
        public List<Tag> GetAllTags()
        {
            return _tags.Values.ToList();
        }

        // get za alarme
        public List<Alarm> GetActiveAlarms()
        {
            List<Alarm> active = new List<Alarm>();

            foreach (var ai in _tags.Values.OfType<AnalogInput>())
            {
                foreach (var alarm in ai.Alarms)
                {
                    if (alarm.IsActive)
                        active.Add(alarm);
                }
            }

            return active;
        }
    }
}


