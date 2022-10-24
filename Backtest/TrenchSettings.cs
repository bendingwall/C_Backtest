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
        public static int DumpWidth { get { return dumpWidth; } set { dumpWidth = value; } }
        public static int PumpWidth { get { return pumpWidth; } set { pumpWidth = value; } }
        public static float TrenchDumpValue { get { return trenchDumpValue; } set { trenchDumpValue = value; } }
        public static float TrenchPumpValue { get { return trenchPumpValue; } set { trenchPumpValue = value; } }
        public static bool TrenchPumpClosedAboveDump { get { return trenchPumpClosedAboveDump; } set { trenchPumpClosedAboveDump = value; } }


        //public static Trade ScanForTrench()
        //{

        //}
    }
}
