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
        private double trenchDumpValue = -0.5;
        private double trenchPumpValue;
        private bool trenchPumpClosedAboveDump;

        public int TotalWidth { get { return totalWidth; } set { totalWidth = value; } }
        public int DumpWidth { get { return dumpWidth; } set { dumpWidth = value; } }
        public int PumpWidth { get { return pumpWidth; } set { pumpWidth = value; } }
        public double TrenchDumpValue { get { return trenchDumpValue; } set { trenchDumpValue = value; } }
        public double TrenchPumpValue { get { return trenchPumpValue; } set { trenchPumpValue = value; } }
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
                double trenchBottom = 99999999999;

                double dumpHighestOpen = 0, dumpHighestHigh = 0, dumpLowestLow = 99999999999, dumpLowestClose = 99999999999;
                int dumpHighestOpenIndex = 0, dumpHighestHighIndex = 0, dumpLowestLowIndex = 0, dumpLowestCloseIndex = 0;

                double pumpHighestOpen = 0, pumpHighestHigh = 0, pumpLowestLow = 99999999999, pumpLowestClose = 99999999999;
                int pumpHighestOpenIndex = 0, pumpHighestHighIndex = 0, pumpLowestLowIndex = 0, pumpLowestCloseIndex = 0;

                List<double> dumpLineChart = new List<double>();
                List<double> pumpLineChart = new List<double>();

                if (candles.Count > TrenchSettings.Min1.DumpWidth + TrenchSettings.Min1.PumpWidth)
                {
                    for (int j = 0; j < TrenchSettings.Min1.DumpWidth; j++)
                    {
                        int key = i + j;

                        if (key < candles.Count - 1) //if we're not at the latest candle
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
                        firstHalf = firstHalf / (dumpLineChart.Count / 2);

                        for (int j = dumpLineChart.Count - 1; j > dumpLineChart.Count / 2; j--)
                        {
                            secondHalf += dumpLineChart[j];
                        }
                        secondHalf = secondHalf / (dumpLineChart.Count / 2);


                        if (firstHalf > secondHalf) //check to see if the average of the first half is more than the average of the second half - more advanced downtrend check
                        {
                            var change = ((dumpLowestClose - dumpHighestOpen) / Math.Abs(dumpHighestOpen)) * 100;

                            if (change <= TrenchSettings.Min1.TrenchDumpValue)
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

                                    if (key < candles.Count) //if we're not at the latest candle
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
                                    if (c.Close > trenchHead)
                                    {
                                        trenchHead = c.Close;
                                    }
                                }

                                if (trenchHead > trenchTail) //if the trench closed above the dump
                                {
                                    var trench = dump.Concat(pump);
                                    DateTime dt = new DateTime();
                                    foreach (var c in trench)
                                    {
                                        if (c.Close > c.Open) //check to see if the candle is positive
                                        {
                                            if (trenchBottom > c.Close)
                                            {
                                                trenchBottom = c.Close;
                                                dt = c.DateTime;
                                            }

                                        }
                                    }

                                    var combined = dump.Concat(pump).ToList();

                                    Trade t = new Trade(trenchBottom, dt, combined);

                                    return t;
                                }
                            }
                        }
                    }
                }
            }

            return null;
        }


        //private static Trade Scan15Min(List<OHLC> candles)
        //{
        //    Trade t = new Trade();
        //    return t;
        //}
        //private static Trade Scan1Hour(List<OHLC> candles)
        //{
        //    Trade t = new Trade();
        //    return t;
        //}
        //private static Trade Scan4Hour(List<OHLC> candles)
        //{
        //    Trade t = new Trade();
        //    return t;
        //}
        //private static Trade Scan12Hour(List<OHLC> candles)
        //{
        //    Trade t = new Trade();
        //    return t;
        //}
        //private static Trade Scan1Day(List<OHLC> candles)
        //{
        //    Trade t = new Trade();
        //    return t;
        //}
        //private static Trade Scan3Day(List<OHLC> candles)
        //{
        //    Trade t = new Trade();
        //    return t;
        //}

        public static Trade ScanForTrench(List<OHLC> candles, int timeframe)
        {
            switch (timeframe)
            {
                case 1:
                    return Scan1Min(candles);
                //case 15:
                //    t = Scan15Min(candles);
                //    return t;
                //case 60:
                //    t = Scan1Hour(candles);
                //    return t;
                //case 4:
                //    t = Scan4Hour(candles);
                //    return t;
                //case 12:
                //    t = Scan12Hour(candles);
                //    return t;
                //case 24:
                //    t = Scan1Day(candles);
                //    return t;
                //case 3:
                //    t = Scan3Day(candles);
                //    return t;
                default:
                    return null;
            }
        }
    }
}
