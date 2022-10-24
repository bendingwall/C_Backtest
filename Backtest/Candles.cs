using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScottPlot;

namespace Backtest
{
    internal static class Candles
    {
        private static List<OHLC> _candles3d = new List<OHLC>();
        private static List<OHLC> _candles1d = new List<OHLC>();
        private static List<OHLC> _candles12h = new List<OHLC>();
        private static List<OHLC> _candles4h = new List<OHLC>();
        private static List<OHLC> _candles1h = new List<OHLC>();
        private static List<OHLC> _candles15m = new List<OHLC>();
        private static List<OHLC> _candles1m = new List<OHLC>();

        private static OHLC[] _3d;
        private static OHLC[] _1d;
        private static OHLC[] _12h;
        private static OHLC[] _4h;
        private static OHLC[] _1h;
        private static OHLC[] _15m;
        private static OHLC[] _1m;

        public static OHLC[] Candles3d { get { return _3d; } }
        public static OHLC[] Candles1d { get { return _1d; } }
        public static OHLC[] Candles12h { get { return _12h; } }
        public static OHLC[] Candles4h { get { return _4h; } }
        public static OHLC[] Candles1h { get { return _1h; } }
        public static OHLC[] Candles15m { get { return _15m; } }
        public static OHLC[] Candles1m { get { return _1m; } }


        public static void ConvertToArray()
        {
            _3d = _candles3d.ToArray();
            _1d = _candles1d.ToArray();
            _12h = _candles12h.ToArray();
            _4h = _candles4h.ToArray();
            _1h = _candles1h.ToArray();
            _15m = _candles15m.ToArray();
            _1m = _candles1m.ToArray();
        }
        public static void AddCandle(OHLC c, int timeframe)
        {
            switch (timeframe)
            {
                case 1:
                    _candles1m.Add(c);
                    break;
                case 15:
                    _candles15m.Add(c);
                    break;
                case 60:
                    _candles1h.Add(c);
                    break;
                case 4:
                    _candles4h.Add(c);
                    break;
                case 12:
                    _candles12h.Add(c);
                    break;
                case 24:
                    _candles1d.Add(c);
                    break;
                case 3:
                    _candles3d.Add(c);
                    break;
                default:
                    break;
            }
        }
    }

    internal class ReadFile
    {
        public ReadFile()
        {
            string[] files = { "BTCUSDT-1m-futures.csv", "BTCUSDT-15m-futures.csv", "BTCUSDT-1h-futures.csv", "BTCUSDT-4h-futures.csv", "BTCUSDT-12h-futures.csv", "BTCUSDT-1d-futures.csv", "BTCUSDT-3d-futures.csv", };

            foreach (var file in files)
            {
                using (var reader = new StreamReader(Path.Combine(@"C:\Users\ADMIN\source\repos\Backtest\Backtest\Data\BTCUSDT\", file)))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');
                        TimeSpan time = new TimeSpan();
                        DateTime DT = new DateTime();

                        if (values[0] != "timestamp")
                        {
                            if(file == "BTCUSDT-1m-futures.csv")
                            {
                                DT = DateTime.ParseExact(values[0], "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                                time = TimeSpan.FromMinutes(1);
                            }
                            else if (file == "BTCUSDT-15m-futures.csv")
                            {
                                DT = DateTime.ParseExact(values[0], "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                                time = TimeSpan.FromMinutes(15);
                            }
                            else if (file == "BTCUSDT-1h-futures.csv")
                            {
                                DT = DateTime.ParseExact(values[0], "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                                time = TimeSpan.FromHours(1);
                            }
                            else if (file == "BTCUSDT-4h-futures.csv")
                            {
                                DT = DateTime.ParseExact(values[0], "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                                time = TimeSpan.FromHours(4);
                            }
                            else if (file == "BTCUSDT-12h-futures.csv")
                            {
                                DT = DateTime.ParseExact(values[0], "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                                time = TimeSpan.FromHours(12);
                            }
                            else if (file == "BTCUSDT-1d-futures.csv")
                            {
                                DT = DateTime.ParseExact(values[0], "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                                time = TimeSpan.FromDays(1);
                            }
                            else if (file == "BTCUSDT-3d-futures.csv")
                            {
                                DT = DateTime.ParseExact(values[0], "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                                time = TimeSpan.FromDays(3);
                            }

                            var open = float.Parse(values[1], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                            var high = float.Parse(values[2], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                            var low = float.Parse(values[3], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                            var close = float.Parse(values[4], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                            var volume = 0.0;


                            OHLC candle = new OHLC(open, high, low, close, DT, time, volume);

                            if (file == "BTCUSDT-1m-futures.csv")
                                Candles.AddCandle(candle, 1);
                            else if (file == "BTCUSDT-15m-futures.csv")
                                Candles.AddCandle(candle, 15);
                            else if (file == "BTCUSDT-1h-futures.csv")
                                Candles.AddCandle(candle, 60);
                            else if (file == "BTCUSDT-4h-futures.csv")
                                Candles.AddCandle(candle, 4);
                            else if (file == "BTCUSDT-12h-futures.csv")
                                Candles.AddCandle(candle, 12);
                            else if (file == "BTCUSDT-1d-futures.csv")
                                Candles.AddCandle(candle, 24);
                            else if (file == "BTCUSDT-3d-futures.csv") //just for clarity
                                Candles.AddCandle(candle, 3);
                        }
                    }
                }
            }

            Candles.ConvertToArray();
        }
    }
}
