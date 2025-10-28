using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataConcentrator
{
    public class AnalogOutput : Tag
    {
        public double InitialValue { get; set; }
        public double LowLimit { get; set; }
        public double HighLimit { get; set; }
        public string Units { get; set; }
        public double CurrentValue { get; private set; }

        public AnalogOutput(string tagName, string description, string ioAddress,
                            double initialValue, double lowLimit, double highLimit, string units)
            : base(tagName, description, ioAddress)
        {
            InitialValue = initialValue;
            LowLimit = lowLimit;
            HighLimit = highLimit;
            Units = units;
            CurrentValue = initialValue;
        }

        public override double ReadValue()
        {
            return CurrentValue;
        }

        //nista
        public override void WriteValue(double value)
        {
            if (value < LowLimit || value > HighLimit)
            {
               // Console.WriteLine($"[WARNING] {TagName}: Value {value} {Units} out of range ({LowLimit}-{HighLimit})");
            }
            CurrentValue = value;
        }
    }
}