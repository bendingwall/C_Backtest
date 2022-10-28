using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backtest
{
    internal class User
    {
        private static double startingBankroll;
        private static double bankroll;
        private static double percentageLoss;
        private static double percentageWin;
        private static double commission;
        private static double compound;
        private static double compoundAfterN;

        public static double StartingBankroll { get { return startingBankroll; } set { startingBankroll = value; } }
        public static double Bankroll { get { return bankroll; } set { bankroll = value; } }
        public static double PercentageLoss { get { return percentageLoss; } set { CalcualtePercentageLoss(value); } }
        public static double PercentageWin { get { return percentageWin; } set { CalcualtePercentageWin(value); } }
        public static double Commission { get { return commission; } set { commission = value; } }
        public static double Compound { get { return compound; } set { compound = value; } }
        public static double CompoundAfterN { get { return compoundAfterN; } set { compoundAfterN = value; } }


        private static double CalcualtePercentageLoss(double v)
        {
            double tmp = 100 - v;
            string s = "0." + tmp.ToString();
            double value = double.Parse(s, System.Globalization.CultureInfo.InvariantCulture);
            return value;
        }

        private static double CalcualtePercentageWin(double v)
        {
            string s = v.ToString();
            s = s.Replace(".", "");
            s = "1." + s;
            double value = double.Parse(s, System.Globalization.CultureInfo.InvariantCulture);
            return value;
        }
    }
}
