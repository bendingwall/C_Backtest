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
        private int totalWidth = 0;
        private int trenchWidth = 0;
        private int dumpWidth = 15;
        private int pumpWidth = 30;
        private int buffer = 205;
        private double trenchDumpValue = -3;
        private double trenchPumpValue;
        private bool trenchPumpClosedAboveDump;

        public TrenchSetting()
        {
            TotalWidth = DumpWidth + PumpWidth + Buffer;
            trenchWidth = DumpWidth + PumpWidth;
        }

        public int TotalWidth { get { return totalWidth; } set { totalWidth = value; } }
        public int DumpWidth { get { return dumpWidth; } set { dumpWidth = value; } }
        public int PumpWidth { get { return pumpWidth; } set { pumpWidth = value; } }
        public int TrenchWidth { get { return trenchWidth; } set { trenchWidth = value; } }
        public int Buffer { get { return buffer; } set { buffer = value; } }
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

            List<List<OHLC>> dumps = new List<List<OHLC>>();
            List<OHLC> pump = new List<OHLC>();

            for (int i = 0; i < candles.Count; i++) //head is always going to be i
            {
                dumps = new List<List<OHLC>>();
                pump = new List<OHLC>();
                double trenchTail = 0;
                double trenchHead = 0;
                double trenchBottom = 99999999999;

                double dumpHighestOpen = 0, dumpHighestHigh = 0, dumpLowestLow = 99999999999, dumpLowestClose = 99999999999;
                int dumpHighestOpenIndex = 0, dumpHighestHighIndex = 0, dumpLowestLowIndex = 0, dumpLowestCloseIndex = 0;

                double pumpHighestOpen = 0, pumpHighestHigh = 0, pumpLowestLow = 99999999999, pumpLowestClose = 99999999999;
                int pumpHighestOpenIndex = 0, pumpHighestHighIndex = 0, pumpLowestLowIndex = 0, pumpLowestCloseIndex = 0;

                List<List<double>> dumpLineChart = new List<List<double>>();
                List<double> pumpLineChart = new List<double>();

                for (int j = TrenchSettings.Min1.DumpWidth; j > 0; j--)
                {
                    List<double> TMP_dumpLineChart = new List<double>();
                    List<OHLC> TMP_dumps = new List<OHLC>();

                    for (int k = 0; k < j; k++)
                    {
                        int key = i + k;

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
                            TMP_dumps.Add(candles[key]);
                            TMP_dumpLineChart.Add(line);
                        }
                        else
                        {
                            break;
                        }
                    }
                    dumps.Add(TMP_dumps);
                    dumpLineChart.Add(TMP_dumpLineChart);
                }
                for (int j = 0; j < dumps.Count; j++) //loop over all dump amounts
                {
                    if (dumpLowestCloseIndex > dumpHighestOpenIndex) //first basic check to see if we're downtrending
                    {
                        double firstHalf = 0;
                        double secondHalf = 0;


                        for (int k = 0; k < dumpLineChart[j].Count / 2; k++)
                        {
                            firstHalf += dumpLineChart[j][k];
                        }
                        firstHalf = firstHalf / (dumpLineChart[j].Count / 2);

                        for (int k = dumpLineChart[j].Count - 1; k > dumpLineChart[j].Count / 2; k--)
                        {
                            secondHalf += dumpLineChart[j][k];
                        }
                        secondHalf = secondHalf / (dumpLineChart[j].Count / 2);


                        if (firstHalf > secondHalf) //check to see if the average of the first half is more than the average of the second half - more advanced downtrend check
                        {
                            var change = ((dumpLowestClose - dumpHighestOpen) / Math.Abs(dumpHighestOpen)) * 100;

                            if (change <= TrenchSettings.Min1.TrenchDumpValue)
                            {
                                foreach (var c in dumps[j])
                                {
                                    if (c.Open > trenchTail)
                                    {
                                        trenchTail = c.Open;
                                    }
                                }

                                for (int k = 0; k < TrenchSettings.Min1.PumpWidth; k++)
                                {
                                    int key = i + dumps[j].Count + k;

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
                                    var trench = dumps[j].Concat(pump);
                                    DateTime dt = new DateTime();
                                    dt = pump[pump.Count - 1].DateTime;
                                    foreach (var c in trench)
                                    {
                                        if (c.Close > c.Open) //check to see if the candle is positive
                                        {
                                            if (trenchBottom > c.Close)
                                            {
                                                trenchBottom = c.Close;
                                            }
                                        }
                                    }

                                    var combined = dumps[j].Concat(pump).ToList();

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
