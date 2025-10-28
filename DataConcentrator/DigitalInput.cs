using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataConcentrator
{
    public class DigitalInput : Tag
    {
        public int ScanTime { get; set; }
        public bool OnOffScan { get; set; }
        public bool CurrentValue { get; private set; }

        public DigitalInput(string tagName, string description, string ioAddress,
                            int scanTime, bool onOffScan)
            : base(tagName, description, ioAddress)
        {
            ScanTime = scanTime;
            OnOffScan = onOffScan;
        }

        public override double ReadValue()
        {
            return CurrentValue ? 1.0 : 0.0;
        }

        public override void WriteValue(double value)
        {
            //nista
            // Dolazi iz PLC-a
           // Console.WriteLine($"[INFO] Cannot write to Digital Input '{TagName}'. Value is read-only.");
        }

        public void UpdateValue(bool newValue)
        {
            CurrentValue = newValue;
        }
    }
}