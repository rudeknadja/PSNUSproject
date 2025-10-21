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
        public double CurrentValue { get; private set; }

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
            if (value < LowLimit || value > HighLimit)
            {
                Console.WriteLine($"[ALARM] {TagName} out of range: {value} {Units}");
            }
        }
    }
}