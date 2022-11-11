using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backtest
{
    internal class User
    {
        private static double startingBankroll = 10000;
        private static double bankroll = 10000;
        private static double percentageTrade = 25;
        private static double percentageLeverage = 20;
        private static double percentageLoss = CalcualtePercentageLoss(0.75);
        private static double percentageWin = CalcualtePercentageWin(0.75);
        private static double commission;
        private static double compound;
        private static double compoundAfterN;

        public static double StartingBankroll { get { return startingBankroll; } set { startingBankroll = value; } }
        public static double Bankroll { get { return bankroll; } set { bankroll = value; } }
        public static double PercentageTrade { get { return percentageTrade; } set { percentageTrade = value; } }
        public static double PercentageLeverage { get { return percentageLeverage; } set { percentageLeverage = value; } }
        public static double PercentageLoss { get { return percentageLoss; } set { CalcualtePercentageLoss(value); } }
        public static double PercentageWin { get { return percentageWin; } set { CalcualtePercentageWin(value); } }
        public static double Commission { get { return commission; } set { commission = value; } }
        public static double Compound { get { return compound; } set { compound = value; } }
        public static double CompoundAfterN { get { return compoundAfterN; } set { compoundAfterN = value; } }


        private static double CalcualtePercentageLoss(double v)
        {
            return 1 - (v / 100);
        }

        private static double CalcualtePercentageWin(double v)
        {
            return 1 + (v / 100);
        }
    }
}
