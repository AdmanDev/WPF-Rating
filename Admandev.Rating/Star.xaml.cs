using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace Admandev.Rating
{
    internal partial class Star : UserControl
    {
        //Delegate
        public delegate void OnValueChangedDelegate(Star star, double value);

        //Events
        //On value changed event
        public event OnValueChangedDelegate OnValueChanged;

        //Properties
        //Star rating (value between 0 and 1)
        public double Value 
        {
            get => this.StarTr.ScaleX;
            set
            {
                SetValue(value);
                OnValueChanged?.Invoke(this, value);
            }
        }

        //Star container
        private Rating Container { get; set; }

        //Constructors
        public Star(Rating container)
        {
            InitializeComponent();

            Container = container;
            SetValue(0);
        }

        //Update value with cursor moving
        private void TheStar_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (Container.ReadOnly)
            {
                return;
            }

            double cursX = e.GetPosition(this.TheStar).X;
            if(cursX > this.ActualWidth)
            {
                cursX = this.ActualWidth;
            }

            double value = cursX / this.ActualWidth;
            Value = RatingModeEffect(value);
        }

        //Apply rating mode on value
        private double RatingModeEffect(double value)
        {
            switch (Container.RatingMode)
            {
                case RatingMode.Standard:
                    return Math.Ceiling(value);
                    break;
                case RatingMode.Half:
                    return Math.Round((value + 0.2) * 2, MidpointRounding.AwayFromZero) / 2;
                    break;
                default:
                    return value;
                    break;
            }
        }

        //Set value without calling event
        internal void SetValue(double value)
        {
            this.StarTr.ScaleX = value;
        }

        public void SetSelectedStarColor(Brush brush)
        {
            this.starBackground.Background = brush;
        }

        public void SetNormalStarColor(Brush brush)
        {
            this.NormalStarBackground.Background = brush;
        }

        public void SetStarBorderSize(double value)
        {
            this.TheStar.StrokeThickness = value;
        }
    }
}
