using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DataConcentrator
{
    public abstract class Tag
    {
        public string TagName { get; set; }
        public string Description { get; set; }
        public string IOAddress { get; set; }
        public string TypeName => GetType().Name;

        public virtual void AddAlarm(Alarm alarm) { } //AnalogInput

        public virtual void RemoveAlarm(Alarm alarm) { } //AnalogInput


        public Tag() { }

        protected Tag(string tagName, string description, string ioAddress)
        {
            TagName = tagName;
            Description = description;
            IOAddress = ioAddress;
        }

        public abstract double ReadValue();
        public abstract void WriteValue(double value);

        public override string ToString()
        {
            return $"{TagName} ({Description}) @ {IOAddress}";
        }
    }
}