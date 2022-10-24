using ScottPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backtest
{
    internal static class TrenchSettings
    {
        static TrenchSetting min1 = new TrenchSetting();
        static TrenchSetting min15 = new TrenchSetting();
        static TrenchSetting hour1 = new TrenchSetting();
        static TrenchSetting hour4 = new TrenchSetting();
        static TrenchSetting hour12 = new TrenchSetting();
        static TrenchSetting day1 = new TrenchSetting();
        static TrenchSetting day3 = new TrenchSetting();

        public static TrenchSetting Min1 { get { return min1; } set { min1 = value; } }
        public static TrenchSetting Min15 { get { return min15; } set { min15 = value; } }
        public static TrenchSetting Hour1 { get { return hour1; } set { hour1 = value; } }
        public static TrenchSetting Hour4 { get { return hour4; } set { hour4 = value; } }
        public static TrenchSetting Hour12 { get { return hour12; } set { hour12 = value; } }
        public static TrenchSetting Day1 { get { return day1; } set { day1 = value; } }
        public static TrenchSetting Day3 { get { return day3; } set { day3 = value; } }
    }
    internal class TrenchSetting
    {
        private int totalWidth = 100;
        private int flatWidth;
        private int dumpWidth = 15;
        private int pumpWidth;
        private float trenchDumpValue;
        private float trenchPumpValue;
        private bool trenchPumpClosedAboveDump;

        public int TotalWidth { get { return totalWidth; } set { totalWidth = value; } }
        public int DumpWidth { get { return dumpWidth; } set { dumpWidth = value; } }
        public int PumpWidth { get { return pumpWidth; } set { pumpWidth = value; } }
        public float TrenchDumpValue { get { return trenchDumpValue; } set { trenchDumpValue = value; } }
        public float TrenchPumpValue { get { return trenchPumpValue; } set { trenchPumpValue = value; } }
        public bool TrenchPumpClosedAboveDump { get { return trenchPumpClosedAboveDump; } set { trenchPumpClosedAboveDump = value; } }
    }

    internal static class Trench
    {
        private static Trade Scan1Min(List<OHLC> candles)
        {
            bool closedAboveDump = false;
            bool dumpValueAccepted = false;
            bool pumpValueAccepted = false;

            List<OHLC> dump = new List<OHLC>();
            List<OHLC> trench = new List<OHLC>();
            List<OHLC> pump = new List<OHLC>();

            for (int i = 0; i < candles.Count; i++) //head is always going to be i
            {
                double highestOpen = 0, highestHigh = 0, lowestLow = 0, lowestClose = 0;

                for (int j = 0; j < TrenchSettings.Min1.DumpWidth; j++)
                {
                    int key = i + j;

                    if (key > candles.Count)
                    {
                        key = candles.Count;
                    }

                    if (candles[key].Open > highestOpen)
                    {
                        highestOpen = candles[key].Open;
                    }
                    if (candles[key].High > highestHigh)
                    {
                        highestHigh = candles[key].High;
                    }
                    if (candles[key].Low < lowestLow)
                    {
                        lowestLow = candles[key].Low;
                    }
                    if (candles[key].Close < lowestClose)
                    {
                        lowestClose = candles[key].Close;
                    }


                }
            }

            closedAboveDump = ClosedAboveDump();
            dumpValueAccepted = DumpValueAccepted();
            pumpValueAccepted = PumpValueAccepted();

            if (closedAboveDump && dumpValueAccepted && pumpValueAccepted)
            {
                Trade t = new Trade();

                //create trade

                return t;
            }
            else
            {
                Trade t = new Trade();
                return t;
            }



            static bool ClosedAboveDump()
            {
                return false;
            }
            static bool DumpValueAccepted()
            {
                return false;
            }
            static bool PumpValueAccepted()
            {
                return false;
            }
        }


        private static Trade Scan15Min(List<OHLC> candles)
        {
            Trade t = new Trade();
            return t;
        }
        private static Trade Scan1Hour(List<OHLC> candles)
        {
            Trade t = new Trade();
            return t;
        }
        private static Trade Scan4Hour(List<OHLC> candles)
        {
            Trade t = new Trade();
            return t;
        }
        private static Trade Scan12Hour(List<OHLC> candles)
        {
            Trade t = new Trade();
            return t;
        }
        private static Trade Scan1Day(List<OHLC> candles)
        {
            Trade t = new Trade();
            return t;
        }
        private static Trade Scan3Day(List<OHLC> candles)
        {
            Trade t = new Trade();
            return t;
        }

        public static Trade ScanForTrench(List<OHLC> candles, int timeframe)
        {
            Trade t = new Trade();

            switch (timeframe)
            {
                case 1:
                    t = Scan1Min(candles);
                    break;
                case 15:
                    t = Scan15Min(candles);
                    break;
                case 60:
                    t = Scan1Hour(candles);
                    break;
                case 4:
                    t = Scan4Hour(candles);
                    break;
                case 12:
                    t = Scan12Hour(candles);
                    break;
                case 24:
                    t = Scan1Day(candles);
                    break;
                case 3:
                    t = Scan3Day(candles);
                    break;
                default:
                    break;
            }

            return t;
        }
    }
}
