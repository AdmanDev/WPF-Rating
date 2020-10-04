using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace Admandev.Rating
{
    internal partial class Symbol : UserControl
    {
        //Delegate
        public delegate void OnValueChangedDelegate(Symbol symbol, double value);

        //Events
        //On value changed event
        public event OnValueChangedDelegate OnValueChanged;

        //Properties
        //Symbol rating (value between 0 and 1)
        public double Value 
        {
            get => this.SymbolTr.ScaleX;
            set
            {
                SetValue(value);
                OnValueChanged?.Invoke(this, value);
            }
        }

        //Symbol container
        private Rating Container { get; set; }

        //Constructors
        public Symbol(Rating container)
        {
            InitializeComponent();

            Container = container;
            SetValue(0);

            this.SetSymbolType(container.SymbolType);
        }

        //Update value with cursor moving
        private void TheSymbol_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (Container.ReadOnly)
            {
                return;
            }

            double cursX = e.GetPosition(this.TheSymbol).X;
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
            this.SymbolTr.ScaleX = value;
        }

        public void SetSelectedSymbolColor(Brush brush)
        {
            this.symbolBackground.Background = brush;
        }

        public void SetNormalSymbolColor(Brush brush)
        {
            this.NormalSymbolBackground.Background = brush;
        }

        public void SetSymbolBorderSize(double value)
        {
            this.TheSymbol.StrokeThickness = value;
        }

        internal void SetSymbolType(Symbols symbolType)
        {
            switch (symbolType)
            {
                case Symbols.Star:
                    this.TheSymbol.Data = SymbolsDefinition.STAR;
                    break;

                case Symbols.Heart:
                    this.TheSymbol.Data = SymbolsDefinition.HEART;
                    break;

                default:
                    this.TheSymbol.Data = SymbolsDefinition.STAR;
                    break;
            }
        }
    }
}
