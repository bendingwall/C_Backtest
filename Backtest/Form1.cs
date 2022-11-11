using ScottPlot;
using System.Diagnostics;

namespace Backtest
{
    public partial class Form1 : Form
    {
        ReadFile rf = new ReadFile();
        List<Trade> min1Trades = new List<Trade>();
        Trade lastTrade = new Trade(0.0, DateTime.Now, null);

        System.Windows.Forms.Timer _1MinTimer = new System.Windows.Forms.Timer();
        System.Windows.Forms.Timer _15MinTimer = new System.Windows.Forms.Timer();
        System.Windows.Forms.Timer _1HourTimer = new System.Windows.Forms.Timer();
        System.Windows.Forms.Timer _4HourTimer = new System.Windows.Forms.Timer();
        System.Windows.Forms.Timer _12HourTimer = new System.Windows.Forms.Timer();
        System.Windows.Forms.Timer _1DayTimer = new System.Windows.Forms.Timer();
        System.Windows.Forms.Timer _3DayTimer = new System.Windows.Forms.Timer();

        System.Windows.Forms.Timer ch = new System.Windows.Forms.Timer();

        int _1MinPosition = 0, _1MinInitNum = 0, _1MinResetTradesCounter = 0, _1MinBreak = 0;
        int _15MinPosition = 0, _15MinInitNum = 0;
        int _1HourPosition = 0, _1HourInitNum = 0;
        int _4HourPosition = 0, _4HourInitNum = 0;
        int _12HourPosition = 0, _12HourInitNum = 0;
        int _1DayPosition = 0, _1DayInitNum = 0;
        int _3DayPosition = 0, _3DayInitNum = 0;

        bool min1InTrade = false;

        ScottPlot.Plottable.Crosshair Crosshair;

        public Form1()
        {            
            InitializeComponent();
            ch.Start();
            ch.Interval = 1;
            ch.Tick += new EventHandler((sender, e) => UpdateCursor(null, null, _1MinPlot));
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
                switch (timeframe)
                {
                    case 1:
                        if (_Position + TrenchSettings.Min1.TrenchWidth <= candles.Length)
                        {
                            if (PartitionChart.Count >= TrenchSettings.Min1.TrenchWidth)
                            {
                                List<OHLC> tmpParChart = new List<OHLC>();
                                for (int i = 0; i < TrenchSettings.Min1.TrenchWidth; i++)
                                {
                                    tmpParChart.Add(PartitionChart[PartitionChart.Count - i - 1]);
                                }
                                tmpParChart.Reverse();

                                var Min1Scan = Trench.ScanForTrench(tmpParChart, 1);

                                if (Min1Scan != null)
                                {
                                    if (_1MinBreak <= 0)
                                    {
                                        if (!min1InTrade && lastTrade.OpenPrice != Min1Scan.OpenPrice)
                                        {
                                            if (PartitionChart[PartitionChart.Count - 1].Low > Min1Scan.OpenPrice)
                                            {
                                                min1Trades.Clear();
                                                min1Trades.Add(Min1Scan);
                                                _1MinResetTradesCounter = 0;
                                                min1InTrade = true;
                                                _1MinBreak = TrenchSettings.Min1.TrenchWidth;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        _1MinBreak--;
                                    }
                                }

                            }
                        }
                        break;
                    case 15:

                        break;
                    case 60:

                        break;
                    case 4:

                        break;
                    case 12:

                        break;
                    case 24:

                        break;
                    case 3:

                        break;
                    default:
                        break;
                }

                _Plot.Plot.Clear();
                _Plot.Plot.AddCandlesticks(PartitionChart.ToArray());


                foreach (var trade in min1Trades)
                {
                    _Plot.Plot.AddHorizontalLine(trade.TakeProfit, color: Color.Green, label: trade.TakeProfit.ToString());

                    if (trade.TradeState.Filled)
                    {
                        _Plot.Plot.AddHorizontalLine(trade.OpenPrice, color: Color.Blue, label: trade.OpenPrice.ToString());
                    }
                    else
                    {
                        _Plot.Plot.AddHorizontalLine(trade.OpenPrice, color: Color.Gray, label: trade.OpenPrice.ToString());
                    }

                    _Plot.Plot.AddHorizontalLine(trade.StopLoss, color: Color.Red, label: trade.StopLoss.ToString());

                    if (trade.TradeState.Filled)
                    {
                        if (PartitionChart[PartitionChart.Count - 1].High > trade.TakeProfit) //trade won
                        {
                            trade.Won = true;
                            trade.TradeState.Closed = true;
                            trade.CloseDate = PartitionChart[PartitionChart.Count - 1].DateTime;
                            Results.Min1.AddTrade(trade);
                            min1Trades.Clear();
                            min1InTrade = false;
                            lastTrade = trade;
                            UpdateResultsGUI(1);
                            break;
                        }
                        else if (PartitionChart[PartitionChart.Count - 1].Low < trade.StopLoss) //trade lost
                        {
                            trade.Won = false;
                            trade.TradeState.Closed = true;
                            trade.CloseDate = PartitionChart[PartitionChart.Count - 1].DateTime;
                            Results.Min1.AddTrade(trade);
                            min1Trades.Clear();
                            min1InTrade = false;
                            lastTrade = trade;
                            UpdateResultsGUI(1);
                            break;
                        }
                        else if (PartitionChart[PartitionChart.Count - 1].High > trade.TakeProfit && PartitionChart[PartitionChart.Count - 1].Low < trade.StopLoss) //need to do an extra scan on the 1 min for this day to determine if the trade was a win/loss, for now just set as loss
                        {
                            trade.Won = false;
                            trade.TradeState.Closed = true;
                            trade.CloseDate = PartitionChart[PartitionChart.Count - 1].DateTime;
                            Results.Min1.AddTrade(trade);
                            min1Trades.Clear();
                            min1InTrade = false;
                            lastTrade = trade;
                            UpdateResultsGUI(1);
                            break;
                        }
                    }
                    else if (trade.TradeState.Open)
                    {
                        if (PartitionChart[PartitionChart.Count - 1].Low < trade.OpenPrice)
                        {
                            trade.TradeState.Filled = true;
                            _1MinResetTradesCounter = 0;
                        }
                    }
                }

                _1MinResetTradesCounter++;
                if (_1MinResetTradesCounter >= TrenchSettings.Min1.TrenchWidth)
                {
                    if (min1Trades.Count > 0)
                    {
                        if (!min1Trades[0].TradeState.Filled)
                        {
                            min1Trades.Clear();
                            min1InTrade = false;
                            _1MinResetTradesCounter = 0;
                        }
                    }
                }

                _Plot.Plot.XAxis.DateTimeFormat(true);
                _Plot.Plot.AxisAuto();
                _Plot.Update();
            }
        }

        private void UpdateCursor(object sender, MouseEventArgs e, FormsPlot _Plot)
        {
            _Plot.Plot.Remove(Crosshair);
            Crosshair = _Plot.Plot.AddCrosshair(0, 0);
            MouseMoved(null, null, _Plot);
        }

        private void MouseMoved(object sender, MouseEventArgs e, FormsPlot _Plot)
        {
            (double coordinateX, double coordinateY) = _Plot.GetMouseCoordinates();

            Crosshair.X = coordinateX;
            Crosshair.Y = coordinateY;

            _Plot.Refresh(lowQuality: true, skipIfCurrentlyRendering: true);
        }

        private void MouseEnter(object sender, EventArgs e)
        {
            Crosshair.IsVisible = true;
        }

        private void MouseLeave(object sender, EventArgs e, FormsPlot _Plot)
        {
            Crosshair.IsVisible = false;
            _Plot.Refresh();
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
            //_3DayTimer.Tick += new EventHandler((sender, e) => StepByStepCharts(sender, e, Candles.Candles3d, _3DayPosition, 3, TrenchSettings.Hour1, _3DayPlot, _3DayInitNum, _3DayTimer));
        }

        private void _1DayPlot_Load(object sender, EventArgs e)
        {
            //_1DayTimer.Tick += new EventHandler((sender, e) => StepByStepCharts(sender, e, Candles.Candles1d, _1DayPosition, 24, TrenchSettings.Day1, _1DayPlot, _1DayInitNum, _1DayTimer));
        }

        private void _12HourPlot_Load(object sender, EventArgs e)
        {
            //_12HourTimer.Tick += new EventHandler((sender, e) => StepByStepCharts(sender, e, Candles.Candles12h, _12HourPosition, 12, TrenchSettings.Hour12, _12HourPlot, _12HourInitNum, _12HourTimer));
        }

        private void _4HourPlot_Load(object sender, EventArgs e)
        {
            //_4HourTimer.Tick += new EventHandler((sender, e) => StepByStepCharts(sender, e, Candles.Candles4h, _4HourPosition, 4, TrenchSettings.Hour4, _4HourPlot, _4HourInitNum, _4HourTimer));
        }

        private void _1HourPlot_Load(object sender, EventArgs e)
        {
            //_1HourTimer.Tick += new EventHandler((sender, e) => StepByStepCharts(sender, e, Candles.Candles1h, _1HourPosition, 60, TrenchSettings.Hour1, _1HourPlot, _1HourInitNum, _1HourTimer));
        }

        private void _15MinPlot_Load(object sender, EventArgs e)
        {
            //_15MinTimer.Tick += new EventHandler((sender, e) => StepByStepCharts(sender, e, Candles.Candles15m, _15MinPosition, 15, TrenchSettings.Min15, _15MinPlot, _15MinInitNum, _15MinTimer));
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

        private void UpdateResultsGUI(int timeframe)
        {
            Min1PNL.Text = Results.Min1.PNL.ToString();

            switch (timeframe)
            {
                case 1:
                    Min1PNL.Text = Math.Round(Results.Min1.PNL, 2).ToString();
                    Min1Trades.Text = Results.Min1.Trades.Count().ToString();
                    Min1Wins.Text = Results.Min1.Wins.ToString();
                    Min1WinPer.Text = Math.Round(Results.Min1.PercentageWin, 2).ToString() + "%";
                    Min1ROI.Text = Math.Round(Results.Min1.ROI, 2).ToString() + "%";
                    Min1DD.Text = Math.Round(Results.Min1.DD, 2).ToString();
                    Min1MaxDD.Text = Math.Round(Results.Min1.MaxDD, 2).ToString();
                    Min1AvgDD.Text = Math.Round(Results.Min1.AvgDD, 2).ToString();
                    UpdateResultsGUI(9999999);
                    break;
                case 15:

                    break;
                case 60:

                    break;
                case 4:

                    break;
                case 12:

                    break;
                case 24:

                    break;
                case 3:

                    break;
                case 9999999:
                    AllPNL.Text = Math.Round(Results.All.PNL, 2).ToString();
                    AllTrades.Text = Results.All.Trades.Count().ToString();
                    AllWins.Text = Results.All.Wins.ToString();
                    AllWinPer.Text = Math.Round(Results.All.PercentageWin, 2).ToString() + "%";
                    AllROI.Text = Math.Round(Results.All.ROI, 2).ToString() + "%";
                    AllDD.Text = Math.Round(Results.All.DD, 2).ToString();
                    AllMaxDD.Text = Math.Round(Results.All.MaxDD, 2).ToString();
                    AllAvgDD.Text = Math.Round(Results.All.AvgDD, 2).ToString();
                    break;
                default:
                    break;
            }
        }
    }
}