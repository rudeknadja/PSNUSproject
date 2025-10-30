﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PLCSimulator
{
    /// <summary>
    /// PLC Simulator
    /// 
    /// 4 x ANALOG INPUT : ADDR001 - ADDR004
    /// 4 x ANALOG OUTPUT: ADDR005 - ADDR008
    /// 1 x DIGITAL INPUT: ADDR009
    /// 1 x DIGITAL OUTPUT: ADDR010
    /// </summary>
    public class PLCSimulatorManager
    {
        private Dictionary<string, double> addressValues;
        private object locker = new object();
        private Thread t1;
        private Thread t2;

        public PLCSimulatorManager()
        {
            addressValues = new Dictionary<string, double>();

            // AI
            addressValues.Add("ADDR001", 0);
            addressValues.Add("ADDR002", 0);
            addressValues.Add("ADDR003", 0);
            addressValues.Add("ADDR004", 0);

            // AO
            addressValues.Add("ADDR005", 0);
            addressValues.Add("ADDR006", 0);
            addressValues.Add("ADDR007", 0);
            addressValues.Add("ADDR008", 0);

            // DI
            // TODO: dodati jos nekoliko adresa za DI (recimo po 4 za svaku vrstu tagova)
            addressValues.Add("ADDR009", 0);
            addressValues.Add("ADDR019", 0);
            addressValues.Add("ADDR029", 0);
            addressValues.Add("ADDR039", 0);

            // DO
            // TODO: dodati jos nekoliko adresa za DI (recimo po 4 za svaku vrstu tagova)
            addressValues.Add("ADDR010", 0);
            addressValues.Add("ADDR011", 0);
            addressValues.Add("ADDR012", 0);
            addressValues.Add("ADDR013", 0);
        }

        public void StartPLCSimulator()
        {
            t1 = new Thread(GeneratingAnalogInputs);
            t1.Start();

            t2 = new Thread(GeneratingDigitalInputs);
            t2.Start();
        }

        private void GeneratingAnalogInputs()
        {
            while (true)
            {
                Thread.Sleep(100);

                lock (locker)
                {
                    addressValues["ADDR001"] = 100 * Math.Sin((double)DateTime.Now.Second / 60 * Math.PI); //SINE
                    addressValues["ADDR002"] = 100 * DateTime.Now.Second / 60; //RAMP
                    addressValues["ADDR003"] = 50 * Math.Cos((double)DateTime.Now.Second / 60 * Math.PI); //COS
                    addressValues["ADDR004"] = RandomNumberBetween(0, 50);  //rand
                }
                // Ispis cisto da vidimo da li PLC Simulator radi
                Console.WriteLine($"ADDR001 = {addressValues["ADDR001"]:F2}");

            }
        }

        private void GeneratingDigitalInputs()
        {
            while (true)
            {
                Thread.Sleep(1000);

                lock (locker)
                {
                    if (addressValues["ADDR009"] == 0)
                    {
                        addressValues["ADDR009"] = 1;
                    }
                    else
                    {
                        addressValues["ADDR009"] = 0;
                    }
                    // TODO: add other digital inputs
                }
                // Ispis cisto da vidimo da li PLC Simulator radi
                Console.WriteLine($"ADDR009 = {addressValues["ADDR009"]:F2}");

            }
        }

        public double GetAnalogValue(string address)
        {

            if (addressValues.ContainsKey(address))
            {
                return addressValues[address];
            }
            else
            {
                return -1;
            }
        }

        public void SetAnalogValue(string address, double value)
        {
            if (addressValues.ContainsKey(address))
            {
                addressValues[address] = value;
            }
        }

        public void SetDigitalValue(string address, double value)
        {
            if (addressValues.ContainsKey(address))
            {
                addressValues[address] = value;
            }
        }

        private static double RandomNumberBetween(double minValue, double maxValue)
        {
            Random random = new Random();
            var next = random.NextDouble();

            return minValue + (next * (maxValue - minValue));
        }

        public void Abort()
        {
            t1.Abort();
            t2.Abort();
        }
    }
}
