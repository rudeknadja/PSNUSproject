using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataConcentrator
{
    public enum AlarmType
    {
        High,   // max
        Low     // min
    }

    public class Alarm
    {
        public string Id { get; set; }
        public string TagName { get; set; }
        public double Limit { get; set; }
        public AlarmType Type { get; set; }
        public string Message { get; set; }
        public DateTime? TimeActivated { get; private set; }
        public bool IsActive { get; private set; }

        public Alarm(string id, string tagName, double limit, AlarmType type, string message)
        {
            Id = id;
            TagName = tagName;
            Limit = limit;
            Type = type;
            Message = message;
            IsActive = false;
        }

        // Provera da li je alarm aktivan
        public bool CheckActivation(double currentValue)
        {
            bool shouldActivate = false;

            if (Type == AlarmType.High && currentValue > Limit)
                shouldActivate = true;
            else if (Type == AlarmType.Low && currentValue < Limit)
                shouldActivate = true;

            if (shouldActivate && !IsActive)
            {
                Activate();
                return true; // aktivan
            }
            else if (!shouldActivate && IsActive)
            {
                Deactivate();
            }

            return false; // nije aktiviran
        }

        private void Activate()
        {
            IsActive = true;
            TimeActivated = DateTime.Now;
          //  Console.WriteLine($"[ALARM ACTIVATED] {TagName} - {Message} (limit: {Limit}) at {TimeActivated}");
        }

        private void Deactivate()
        {
            IsActive = false;
          //  Console.WriteLine($"[ALARM CLEARED] {TagName} - {Message}");
        }
    }
}

