using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataConcentrator
{
    public abstract class Tag
    {
        public string TagName { get; set; }
        public string Description { get; set; }
        public string IOAddress { get; set; }

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