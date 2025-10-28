using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataConcentrator
{
    public class DigitalOutput : Tag
    {
        public bool InitialValue { get; set; }
        public bool CurrentValue { get; private set; }

        public DigitalOutput(string tagName, string description, string ioAddress, bool initialValue)
            : base(tagName, description, ioAddress)
        {
            InitialValue = initialValue;
            CurrentValue = initialValue;
        }

        public override double ReadValue()
        {
            return CurrentValue ? 1.0 : 0.0;
        }

        public override void WriteValue(double value)
        {
            CurrentValue = value != 0;
            //bool newValue = value != 0;
            //CurrentValue = newValue;
            //Console.WriteLine($"[INFO] Digital Output '{TagName}' set to {(newValue ? "ON" : "OFF")}");
        }
    }
}
