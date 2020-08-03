using System;
using System.Windows;
using System.Windows.Forms;

namespace TestApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Rating_RateEvent(double rate, double percentRate)
        {
            this.Title = rate.ToString() + "  -  " + percentRate * 100 + "%";
        }
    }
}
