using ScottPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backtest
{
    internal static class Results
    {
        private static Result all = new Result();
        private static Result min1 = new Result();
        private static Result min15 = new Result();
        private static Result hour1 = new Result();
        private static Result hour4 = new Result();
        private static Result hour12 = new Result();
        private static Result day1 = new Result();
        private static Result day3 = new Result();

        public static Result All { get { return all; } }
        public static Result Min1 {  get { return min1; } }
        public static Result Min15 { get { return min15; } }
        public static Result Hour1 { get { return hour1; } }
        public static Result Hour4 { get { return hour4; } }
        public static Result Hour12 { get { return hour12; } }
        public static Result Day1 { get { return day1; } }
        public static Result Day3 { get { return day3; } }

        private static void CalculateAll()
        {
            List<Result> allResults = new List<Result>();
            allResults.Add(min1);
            allResults.Add(min15);
            allResults.Add(hour1);
            allResults.Add(hour4);
            allResults.Add(hour12);
            allResults.Add(day1);
            allResults.Add(day3);
            all.CalculateAll(allResults);
        }

        private static void UpdateGUI()
        {
            
        }

        public static void Add1MinTrade(Trade t)
        {
            min1.AddTrade(t);
            CalculateAll();
        }
        public static void Add15MinTrade(Trade t)
        {
            min15.AddTrade(t);
            CalculateAll();
        }
        public static void Add1HourTrade(Trade t)
        {
            hour1.AddTrade(t);
            CalculateAll();
        }
        public static void Add4HourTrade(Trade t)
        {
            hour4.AddTrade(t);
            CalculateAll();
        }
        public static void Add12HourTrade(Trade t)
        {
            hour12.AddTrade(t);
            CalculateAll();
        }
        public static void Add1DayTrade(Trade t)
        {
            day1.AddTrade(t);
            CalculateAll();
        }
        public static void Add3DayTrade(Trade t)
        {
            day3.AddTrade(t);
            CalculateAll();
        }
    }

    internal class TradeState
    {
        private bool open;
        private bool filled;
        private bool closed;

        public bool Open { get { return open; } set { SetState(); open = value; } }
        public bool Filled { get { return filled; } set { SetState(); filled = value; } }
        public bool Closed { get { return closed; } set { SetState(); closed = value; } }

        public TradeState()
        {
            open = true;
        }

        private void SetState()
        {
            open = false;
            filled = false;
            closed = false;
        }
    }

    internal class Trade
    {
        private TradeState tradeState;
        private DateTime openDate;
        private DateTime closeDate;
        private double openPrice;
        private double amount;
        private double stopLoss;
        private double takeProfit;
        private bool won;
        private List<OHLC> candles;

        public Trade(double op, DateTime date, List<OHLC> c)
        {
            OpenPrice = op;
            TakeProfit = OpenPrice * User.PercentageWin;
            StopLoss = OpenPrice * User.PercentageLoss;
            Amount = (User.Bankroll / 100) * User.PercentageTrade;
            OpenDate = date;
            Candles = c;
            TradeState = new TradeState();
        }
        public TradeState TradeState { get { return tradeState; } set { tradeState = value; } }
        public DateTime OpenDate { get { return openDate; } set { openDate = value; } }
        public DateTime CloseDate { get { return closeDate; } set { closeDate = value; } }
        public double OpenPrice { get { return openPrice; } set { openPrice = value; } }
        public double Amount { get { return amount; } set { amount = value; } }
        public double StopLoss { get { return stopLoss; } set { stopLoss = value; } }
        public double TakeProfit { get { return takeProfit; } set { takeProfit = value; } } 
        public bool Won { get { return won; } set { won = value; } }
        public List<OHLC> Candles { get { return candles; } set { candles = value; } }
    }
    internal class Result
    {
        private double pnl;
        private double percentagewin;
        private double roi;
        private double dd;
        private double maxdd;
        private double avgdd;
        private int wins;
        private List<Trade> trades = new List<Trade>();

        public double PNL { get { return pnl; } set { pnl = value; } }
        public double PercentageWin { get { return percentagewin; } set { percentagewin = value; } }
        public double ROI { get { return roi; } set { roi = value; } }
        public double DD { get { return dd; } set { dd = value; } }
        public double MaxDD { get { return maxdd; } set { maxdd = value; } }
        public double AvgDD { get { return avgdd; } set { avgdd = value; } }
        public int Wins { get { return wins; } set { wins = value; } }
        public List<Trade> Trades { get { return trades; } }

        public void AddTrade(Trade t)
        {
            trades.Add(t);
            if (t.Won)
            {

            }
            Calculate();
        }

        public void CalculateAll(List<Result> allResults)
        {
            List<Trade> sortedByDateList = new List<Trade>();

            foreach (Result timeframe in allResults)
            {
                foreach (var trade in timeframe.trades)
                {
                    sortedByDateList.Add(trade);
                }
            }

            sortedByDateList.Sort((x, y) => y.OpenDate.CompareTo(x.OpenDate));

            foreach (var trade in sortedByDateList)
            {
                AddTrade(trade);
            }
        }

        public void Calculate()
        {
            double tmpPNL = 0;
            double highestPNL = 0;
            double currentDD = 0;
            double maxDD = 0;
            double lowestLastDD = 0;
            bool inDD = false;
            int wins = 0;
            List<double> allDDs = new List<double>();
            foreach (var trade in trades)
            {

                if (trade.Won)
                {
                    var profitPercentage = ((trade.TakeProfit - trade.OpenPrice) / trade.OpenPrice);
                    tmpPNL += (trade.Amount * profitPercentage);
                    wins++;
                }
                else
                {
                    var lossPercentage = ((trade.StopLoss - trade.OpenPrice) / trade.OpenPrice);
                    tmpPNL -= (trade.Amount * -lossPercentage);
                    inDD = true;
                }


                if (tmpPNL > highestPNL)
                {
                    highestPNL = tmpPNL;
                    inDD = false;
                    lowestLastDD = 0; //make it 0 so we dont spam add it to the list to work out average
                }
                else
                {
                    currentDD = tmpPNL - highestPNL;

                    if (currentDD < maxDD)
                    {
                        maxDD = currentDD;
                    }
                }

                if (inDD)
                {
                    if (tmpPNL - highestPNL < lowestLastDD - highestPNL)
                    {
                        lowestLastDD = tmpPNL - highestPNL;
                    }
                }
                else
                {
                    if (lowestLastDD != 0)
                    {
                        allDDs.Add(lowestLastDD);
                    }
                }
            }

            double dds = 0;
            foreach (var item in allDDs)
            {
                dds += item;
            }
            double avgDD = dds / allDDs.Count();

            double pw = (double)wins / trades.Count() * 100;
            PercentageWin = Math.Round(pw, 2);
            PNL = Math.Round(tmpPNL, 2);
            User.Bankroll = User.StartingBankroll + PNL;
            ROI = Math.Round(User.Bankroll / User.StartingBankroll * 100 - 100, 2);
            DD = currentDD;
            MaxDD = maxDD;
            AvgDD = avgDD;
            Wins = wins;
        }
    }
}
