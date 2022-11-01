using ScottPlot;
using System.Diagnostics;

namespace Backtest
{
    public partial class Form1 : Form
    {
        ReadFile rf = new ReadFile();
        List<Trade> min1Trades = new List<Trade>();

        System.Windows.Forms.Timer _1MinTimer = new System.Windows.Forms.Timer();
        System.Windows.Forms.Timer _15MinTimer = new System.Windows.Forms.Timer();
        System.Windows.Forms.Timer _1HourTimer = new System.Windows.Forms.Timer();
        System.Windows.Forms.Timer _4HourTimer = new System.Windows.Forms.Timer();
        System.Windows.Forms.Timer _12HourTimer = new System.Windows.Forms.Timer();
        System.Windows.Forms.Timer _1DayTimer = new System.Windows.Forms.Timer();
        System.Windows.Forms.Timer _3DayTimer = new System.Windows.Forms.Timer();

        int _1MinPosition = 0, _1MinInitNum = 0, _1MinResetTradesCounter = 0;
        int _15MinPosition = 0, _15MinInitNum = 0;
        int _1HourPosition = 0, _1HourInitNum = 0;
        int _4HourPosition = 0, _4HourInitNum = 0;
        int _12HourPosition = 0, _12HourInitNum = 0;
        int _1DayPosition = 0, _1DayInitNum = 0;
        int _3DayPosition = 0, _3DayInitNum = 0;

        public Form1()
        {            
            InitializeComponent();

            _1MinInitNum = TrenchSettings.Min1.TotalWidth - 1;
            _15MinInitNum = TrenchSettings.Min15.TotalWidth - 1;
            _1HourInitNum = TrenchSettings.Hour1.TotalWidth - 1;
            _4HourInitNum = TrenchSettings.Hour4.TotalWidth - 1;
            _12HourInitNum = TrenchSettings.Hour12.TotalWidth - 1;
            _1DayInitNum = TrenchSettings.Day1.TotalWidth - 1;
            _3DayInitNum = TrenchSettings.Day3.TotalWidth - 1;
        }

        private void StepByStepCharts(object sender, EventArgs e, OHLC[] candles, int _Position, int timeframe, TrenchSetting trench, FormsPlot _Plot, int InitNum, System.Windows.Forms.Timer time)
        {
            if (Interactables.Play)
            {
                List<OHLC> PartitionChart = new List<OHLC>();

                if (InitNum > 0)
                {
                    for (int i = 0; i < trench.TotalWidth - InitNum; i++)
                    {
                        if (i >= candles.Length)
                        {
                            time.Stop();
                        }
                        else
                        {
                            PartitionChart.Add(candles[i]);
                        }
                    }
                    switch (timeframe)
                    {
                        case 1:
                            _1MinInitNum--;
                            break;
                        case 15:
                            _15MinInitNum--;
                            break;
                        case 60:
                            _1HourInitNum--;
                            break;
                        case 4:
                            _4HourInitNum--;
                            break;
                        case 12:
                            _12HourInitNum--;
                            break;
                        case 24:
                            _1DayInitNum--;
                            break;
                        case 3:
                            _3DayInitNum--;
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    for (int i = 0; i < trench.TotalWidth; i++)
                    {
                        if (_Position + i >= candles.Length)
                        {
                            time.Stop();
                        }
                        else
                        {
                            PartitionChart.Add(candles[_Position + i]);
                        }
                    }

                    switch (timeframe)
                    {
                        case 1:
                            if (_Position + TrenchSettings.Min1.TotalWidth <= candles.Length)
                            {
                                _1MinPosition++;

                                var Min1Scan = Trench.ScanForTrench(PartitionChart, 1);

                                if (Min1Scan != null)
                                {
                                    min1Trades.Clear();
                                    min1Trades.Add(Min1Scan);
                                }

                                if (min1Trades.Count > 0)
                                {
                                    _1MinResetTradesCounter++;
                                }
                            }
                            break;
                        case 15:
                            if (_Position + TrenchSettings.Min15.TotalWidth <= candles.Length)
                            {
                                _15MinPosition++;
                            }
                            break;
                        case 60:
                            if (_Position + TrenchSettings.Hour1.TotalWidth <= candles.Length)
                            {
                                _1HourPosition++;
                            }
                            break;
                        case 4:
                            if (_Position + TrenchSettings.Hour4.TotalWidth <= candles.Length)
                            {
                                _4HourPosition++;
                            }
                            break;
                        case 12:
                            if (_Position + TrenchSettings.Hour12.TotalWidth <= candles.Length)
                            {
                                _12HourPosition++;
                            }
                            break;
                        case 24:
                            if (_Position + TrenchSettings.Day1.TotalWidth <= candles.Length)
                            {
                                _1DayPosition++;
                            }
                            break;
                        case 3:
                            if (_Position + TrenchSettings.Day3.TotalWidth <= candles.Length)
                            {
                                _3DayPosition++;
                            }
                            break;
                        default:
                            break;
                    }
                }

                _Plot.Plot.Clear();
                _Plot.Plot.AddCandlesticks(PartitionChart.ToArray());

                foreach (var trade in min1Trades)
                {
                    _Plot.Plot.AddHorizontalLine(trade.TakeProfit, color: Color.Green);
                    _Plot.Plot.AddHorizontalLine(trade.OpenPrice, color: Color.Gray);
                    _Plot.Plot.AddHorizontalLine(trade.StopLoss, color: Color.Red);
                }

                _Plot.Plot.XAxis.DateTimeFormat(true);
                _Plot.Plot.AxisAuto();
                _Plot.Refresh();
            }
        }

        private void SetTimerSpeeds()
        {
            _1MinTimer.Interval = Interactables.Speed;
            _15MinTimer.Interval = Interactables.Speed;
            _1HourTimer.Interval = Interactables.Speed;
            _4HourTimer.Interval = Interactables.Speed;
            _12HourTimer.Interval = Interactables.Speed;
            _1DayTimer.Interval = Interactables.Speed;
            _3DayTimer.Interval = Interactables.Speed;
        }

        private void _3DayPlot_Load(object sender, EventArgs e)
        {
            _3DayTimer.Tick += new EventHandler((sender, e) => StepByStepCharts(sender, e, Candles.Candles3d, _3DayPosition, 3, TrenchSettings.Hour1, _3DayPlot, _3DayInitNum, _3DayTimer));
        }

        private void _1DayPlot_Load(object sender, EventArgs e)
        {
            _1DayTimer.Tick += new EventHandler((sender, e) => StepByStepCharts(sender, e, Candles.Candles1d, _1DayPosition, 24, TrenchSettings.Day1, _1DayPlot, _1DayInitNum, _1DayTimer));
        }

        private void _12HourPlot_Load(object sender, EventArgs e)
        {
            _12HourTimer.Tick += new EventHandler((sender, e) => StepByStepCharts(sender, e, Candles.Candles12h, _12HourPosition, 12, TrenchSettings.Hour12, _12HourPlot, _12HourInitNum, _12HourTimer));
        }

        private void _4HourPlot_Load(object sender, EventArgs e)
        {
            _4HourTimer.Tick += new EventHandler((sender, e) => StepByStepCharts(sender, e, Candles.Candles4h, _4HourPosition, 4, TrenchSettings.Hour4, _4HourPlot, _4HourInitNum, _4HourTimer));
        }

        private void _1HourPlot_Load(object sender, EventArgs e)
        {
            _1HourTimer.Tick += new EventHandler((sender, e) => StepByStepCharts(sender, e, Candles.Candles1h, _1HourPosition, 60, TrenchSettings.Hour1, _1HourPlot, _1HourInitNum, _1HourTimer));
        }

        private void _15MinPlot_Load(object sender, EventArgs e)
        {
            _15MinTimer.Tick += new EventHandler((sender, e) => StepByStepCharts(sender, e, Candles.Candles15m, _15MinPosition, 15, TrenchSettings.Min15, _15MinPlot, _15MinInitNum, _15MinTimer));
        }

        private void _1MinPlot_Load(object sender, EventArgs e)
        {
            _1MinTimer.Tick += new EventHandler((sender, e) => StepByStepCharts(sender, e, Candles.Candles1m, _1MinPosition, 1, TrenchSettings.Min1, _1MinPlot, _1MinInitNum, _1MinTimer));
        }


        private void PlayButton_Click(object sender, EventArgs e)
        {
            Interactables.Play = true;
            Interactables.Pause = false;
            Interactables.Refresh = false;

            SetTimerSpeeds();

            _1MinTimer.Start();
            _15MinTimer.Start();
            _1HourTimer.Start();
            _4HourTimer.Start();
            _12HourTimer.Start();
            _1DayTimer.Start();
            _3DayTimer.Start();
        }

        private void PauseButton_Click(object sender, EventArgs e)
        {
            Interactables.Play = false;
            Interactables.Pause = true;
            Interactables.Refresh = false;

            _1MinTimer.Stop();
            _15MinTimer.Stop();
            _1HourTimer.Stop();
            _4HourTimer.Stop();
            _12HourTimer.Stop();
            _1DayTimer.Stop();
            _3DayTimer.Stop();
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            Interactables.Play = false;
            Interactables.Pause = false;
            Interactables.Refresh = true;
        }

        private void PlaySpeedBar_Scroll(object sender, EventArgs e)
        {
            switch (PlaySpeedBar.Value)
            {
                case 1:
                    Interactables.Speed = 1000;
                    break;
                case 2:
                    Interactables.Speed = 750;
                    break;
                case 3:
                    Interactables.Speed = 500;
                    break;
                case 4:
                    Interactables.Speed = 400;
                    break;
                case 5:
                    Interactables.Speed = 300;
                    break;
                case 6:
                    Interactables.Speed = 200;
                    break;
                case 7:
                    Interactables.Speed = 100;
                    break;
                case 8:
                    Interactables.Speed = 75;
                    break;
                case 9:
                    Interactables.Speed = 50;
                    break;
                case 10:
                    Interactables.Speed = 1;
                    break;
                default:
                    break;
            }

            SetTimerSpeeds();
        }
    }
}