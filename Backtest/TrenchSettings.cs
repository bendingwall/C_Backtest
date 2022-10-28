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
        private int dumpWidth = 15;
        private int pumpWidth = 30;
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
            List<OHLC> pump = new List<OHLC>();

            for (int i = 0; i < candles.Count; i++) //head is always going to be i
            {
                dump = new List<OHLC>();
                pump = new List<OHLC>();
                double trenchTail = 0;
                double trenchHead = 0;
                double trenchBottom = 0;

                double dumpHighestOpen = 0, dumpHighestHigh = 0, dumpLowestLow = 0, dumpLowestClose = 0;
                int dumpHighestOpenIndex = 0, dumpHighestHighIndex = 0, dumpLowestLowIndex = 0, dumpLowestCloseIndex = 0;

                double pumpHighestOpen = 0, pumpHighestHigh = 0, pumpLowestLow = 0, pumpLowestClose = 0;
                int pumpHighestOpenIndex = 0, pumpHighestHighIndex = 0, pumpLowestLowIndex = 0, pumpLowestCloseIndex = 0;

                List<double> dumpLineChart = new List<double>();
                List<double> pumpLineChart = new List<double>();

                for (int j = 0; j < TrenchSettings.Min1.DumpWidth; j++)
                {
                    int key = i + j;

                    if (key <= candles.Count) //if were not at the latest candle
                    {
                        if (candles[key].Open > dumpHighestOpen)
                        {
                            dumpHighestOpen = candles[key].Open;
                            dumpHighestOpenIndex = key;
                        }
                        if (candles[key].High > dumpHighestHigh)
                        {
                            dumpHighestHigh = candles[key].High;
                            dumpHighestHighIndex = key;
                        }
                        if (candles[key].Low < dumpLowestLow)
                        {
                            dumpLowestLow = candles[key].Low;
                            dumpLowestLowIndex = key;
                        }
                        if (candles[key].Close < dumpLowestClose)
                        {
                            dumpLowestClose = candles[key].Close;
                            dumpLowestCloseIndex = key;
                        }

                        double line = (candles[key].Open + candles[key].High + candles[key].Low + candles[key].Close) / 4;
                        dump.Add(candles[key]);
                        dumpLineChart.Add(line);
                    }
                    else
                    {
                        break;
                    }
                }

                if (dumpLowestCloseIndex > dumpHighestOpenIndex) //first basic check to see if we're downtrending
                {
                    double firstHalf = 0;
                    double secondHalf = 0;

                    for (int j = 0; j < dumpLineChart.Count / 2; j++)
                    {
                        firstHalf += dumpLineChart[j];
                    }
                    firstHalf = dumpLineChart.Count / 2;

                    for (int j = dumpLineChart.Count; j > dumpLineChart.Count / 2; j--)
                    {
                        secondHalf += dumpLineChart[j];
                    }
                    secondHalf = dumpLineChart.Count / 2;


                    if (firstHalf > secondHalf) //check to see if the average of the first half is more than the average of the second half - more advanced downtrend check
                    {
                        var change = ((dumpLowestClose - dumpHighestOpen) / Math.Abs(dumpHighestOpen)) * 100;

                        if (change > TrenchSettings.Min1.TrenchDumpValue)
                        {
                            foreach (var c in dump)
                            {
                                if (c.Open > trenchTail)
                                {
                                    trenchTail = c.Open;
                                }
                            }

                            for (int j = 0; j < TrenchSettings.Min1.PumpWidth; j++)
                            {
                                int key = i + dump.Count + j;

                                if (key <= candles.Count) //if were not at the latest candle
                                {
                                    if (candles[key].Open > pumpHighestOpen)
                                    {
                                        pumpHighestOpen = candles[key].Open;
                                        pumpHighestOpenIndex = key;
                                    }
                                    if (candles[key].High > pumpHighestHigh)
                                    {
                                        pumpHighestHigh = candles[key].High;
                                        pumpHighestHighIndex = key;
                                    }
                                    if (candles[key].Low < pumpLowestLow)
                                    {
                                        pumpLowestLow = candles[key].Low;
                                        pumpLowestLowIndex = key;
                                    }
                                    if (candles[key].Close < pumpLowestClose)
                                    {
                                        pumpLowestClose = candles[key].Close;
                                        pumpLowestCloseIndex = key;
                                    }

                                    double line = (candles[key].Open + candles[key].High + candles[key].Low + candles[key].Close) / 4;
                                    pump.Add(candles[key]);
                                    pumpLineChart.Add(line);
                                }
                                else
                                {
                                    break;
                                }
                            }

                            foreach (var c in pump)
                            {
                                if (c.Open > trenchHead)
                                {
                                    trenchHead = c.Open;
                                }
                            }

                            if (trenchHead > trenchTail)
                            {
                                var trench = dump.Concat(pump);

                                foreach (var c in trench)
                                {
                                    if (trenchBottom > c.Open)
                                    {
                                        trenchBottom = c.Open;
                                    }
                                }

                                //create trade
                            }
                        }
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
