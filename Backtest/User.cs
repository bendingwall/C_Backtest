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
        private static double percentagesStake;
        private static double commission;
        private static double compound;
        private static double compoundAfterN;

        public static double StartingBankroll { get { return startingBankroll; } set { startingBankroll = value; } }
        public static double Bankroll { get { return bankroll; } set { bankroll = value; } }
        public static double PercentageStake { get { return percentagesStake; } set { percentagesStake = value; } }
        public static double Commission { get { return commission; } set { commission = value; } }
        public static double Compound { get { return compound; } set { compound = value; } }
        public static double CompoundAfterN { get { return compoundAfterN; } set { compoundAfterN = value; } }
    }
}
