using ScottPlot;

namespace Backtest
{
    public partial class Form1 : Form
    {
        ReadFile rf = new ReadFile();
        public Form1()
        {
            InitializeComponent();
        }


        private void _3DayPlot_Load(object sender, EventArgs e)
        {
            _3DayPlot.Plot.AddCandlesticks(Candles.Candles3d);
            _3DayPlot.Plot.XAxis.DateTimeFormat(true);

            _3DayPlot.Refresh();
        }

        private void _1DayPlot_Load(object sender, EventArgs e)
        {
            _1DayPlot.Plot.AddCandlesticks(Candles.Candles1d);
            _1DayPlot.Plot.XAxis.DateTimeFormat(true);

            _1DayPlot.Refresh();
        }

        private void _12HourPlot_Load(object sender, EventArgs e)
        {
            _12HourPlot.Plot.AddCandlesticks(Candles.Candles12h);
            _12HourPlot.Plot.XAxis.DateTimeFormat(true);

            _12HourPlot.Refresh();
        }

        private void _4HourPlot_Load(object sender, EventArgs e)
        {
            _4HourPlot.Plot.AddCandlesticks(Candles.Candles4h);
            _4HourPlot.Plot.XAxis.DateTimeFormat(true);

            _4HourPlot.Refresh();
        }

        private void _1HourPlot_Load(object sender, EventArgs e)
        {
            _1HourPlot.Plot.AddCandlesticks(Candles.Candles1h);
            _1HourPlot.Plot.XAxis.DateTimeFormat(true);

            _1HourPlot.Refresh();
        }

        private void _15MinPlot_Load(object sender, EventArgs e)
        {
            _15MinPlot.Plot.AddCandlesticks(Candles.Candles15m);
            _15MinPlot.Plot.XAxis.DateTimeFormat(true);

            _15MinPlot.Refresh();
        }

        private void _1MinPlot_Load(object sender, EventArgs e)
        {
            _1MinPlot.Plot.AddCandlesticks(Candles.Candles1m);
            _1MinPlot.Plot.XAxis.DateTimeFormat(true);

            _1MinPlot.Refresh();
        }
    }
}