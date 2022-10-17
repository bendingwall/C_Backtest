using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backtest
{
    internal static class TrenchSettings
    {
        private static int totalWidth = 100;
        private static int flatWidth;
        private static int dumpWidth;
        private static int pumpWidth;
        private static float trenchDumpValue;
        private static float trenchPumpValue;
        private static bool trenchPumpClosedAboveDump;

        public static int TotalWidth { get { return totalWidth; } set { totalWidth = value; } }
    }
}
