using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backtest
{
    internal static class Interactables
    {
        private static bool _play = true;
        private static bool _pause = false;
        private static bool _refresh = false;
        private static bool _sound = true;
        private static int _speed = 1000;

        public static bool Play { get { return _play; } set { _play = value; } }
        public static bool Pause { get { return _pause; } set { _pause = value; } }
        public static bool Refresh { get { return _refresh; } set { _refresh = value; } }
        public static bool Sound { get { return _sound; } set { _sound = value; } }
        public static int Speed { get { return _speed; } set { _speed = value; } }
    }
}
