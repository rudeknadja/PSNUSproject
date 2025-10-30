using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataConcentrator
{
    public class AnalogInput : Tag
    {
        public double LowLimit { get; set; }
        public double HighLimit { get; set; }
        public string Units { get; set; }
        public int ScanTime { get; set; } // u milisekundama
        public bool OnOffScan { get; set; }
        public double CurrentValue { get; set; }

        public List<Alarm> Alarms { get; private set; } = new List<Alarm>();

        //da li ima alarme, tj. da li je alarms true stavjen kada se kreirao analog input
        public bool HasAlarms => Alarms.Count > 0;

        public AnalogInput() : base() { }

        public AnalogInput(string tagName, string description, string ioAddress,
                           double lowLimit, double highLimit, string units, int scanTime, bool onOffScan)
            : base(tagName, description, ioAddress)
        {
            LowLimit = lowLimit;
            HighLimit = highLimit;
            Units = units;
            ScanTime = scanTime;
            OnOffScan = onOffScan;
        }

        public override double ReadValue()
        {
            // Citati iz PLC simulatora
            return CurrentValue;
        }

        public override void WriteValue(double value)
        {
            CurrentValue = value;

            // Proveri sve alarme koji su vezani za ovu veličinu
            foreach (var alarm in Alarms)
            {
                alarm.CheckActivation(value);
            }
        }

        public override void AddAlarm(Alarm alarm)
        {
            if (alarm.TagName != TagName)
            {
                // Console.WriteLine($"[WARNING] Alarm {alarm.Id} is linked to a different tag!");
                return;
            }

            Alarms.Add(alarm);
        }

        public void RemoveAlarm(string alarmId)
        {
            Alarms.RemoveAll(a => a.Id == alarmId);
        }
    }
}