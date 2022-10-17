using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backtest
{
    internal class Trades
    {
        private DateTime date;
        private double open;
        private double stoploss;
        private double takeprofit;
        private bool won;

        public DateTime Date { get { return date; } set { date = value; } }
        public double Open { get { return open; } set { open = value; } }
        public double StopLoss { get { return stoploss; } set { stoploss = value; } }
        public double TakeProfit { get { return takeprofit; } set { takeprofit = value; } } 
        public bool Won { get { return won; } set { won = value; } }

    }
    internal class Results
    {
        private double pnl;
        private double percentagewins;
        private double roi;
        private double dd;
        private double maxdd;
        private double avgdd;
        private int wins;
        private List<Trades> trades = new List<Trades>();

        public double PNL { get { return pnl; } set { pnl = value; } }
        public double PercentageWins { get { return percentagewins; } set { percentagewins = value; } }
        public double ROI { get { return roi; } set { roi = value; } }
        public double DD { get { return dd; } set { dd = value; } }
        public double MaxDD { get { return maxdd; } set { maxdd = value; } }
        public double AvgDD { get { return avgdd; } set { avgdd = value; } }
        public int Wins { get { return wins; } set { wins = value; } }
        public List<Trades> Trades { get { return trades; } }

        public void AddTrade(Trades t)
        {
            trades.Add(t);
        }
    }
}
