using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Admandev.Rating
{
    public partial class Rating : UserControl
    {
        //Variables
        private readonly List<Symbol> Symbols;

        //Delegate
        public delegate void RateDelegate(double rate, double percentRate);

        //Event
        public event RateDelegate RateEvent;

        #region Properties

        public static readonly DependencyProperty normalSymbolColorProp = DependencyProperty.Register("NormalSymbolColor",
                                                                  typeof(Brush),
                                                                  typeof(Rating),
                                                                  new PropertyMetadata(
                                                                      new SolidColorBrush(Colors.Transparent),
                                                                      NormalSymbolColorChanged));

        public Brush NormalSymbolColor
        {
            get => (Brush)GetValue(normalSymbolColorProp);
            set => SetValue(normalSymbolColorProp, value);
        }

        public static readonly DependencyProperty selectedSymbolColorProp = DependencyProperty.Register("SelectedSymbolColor",
                                                                  typeof(Brush),
                                                                  typeof(Rating),
                                                                  new PropertyMetadata(
                                                                      new SolidColorBrush(Colors.Red),
                                                                      SelectedSymbolColorChanged));

        public Brush SelectedSymbolColor 
        { 
            get => (Brush)GetValue(selectedSymbolColorProp);
            set => SetValue(selectedSymbolColorProp, value);
        }

        public static readonly DependencyProperty SymbolCountProp = DependencyProperty.Register("SymbolsCount",
                                                                typeof(int),
                                                                typeof(Rating),
                                                                new PropertyMetadata(5, SymbolsCountChanged));
        public int SymbolsCount 
        {
            get => (int)GetValue(SymbolCountProp);
            set => SetValue(SymbolCountProp, value);
        }

        public static readonly DependencyProperty SymbolSpacingProp = DependencyProperty.Register("SymbolSpacing",
                                                                    typeof(double),
                                                                    typeof(Rating),
                                                                    new PropertyMetadata(5d, SymbolSpacingChanged));
        public double SymbolSpacing 
        {
            get => (double)GetValue(SymbolSpacingProp);
            set => SetValue(SymbolSpacingProp, value);
        }

        public static readonly DependencyProperty SymbolSizeProp = DependencyProperty.Register("SymbolSize",
                                                                 typeof(double),
                                                                 typeof(Rating),
                                                                 new PropertyMetadata(40d, SymbolSizeChanged));
        public double SymbolSize 
        {
            get => (double)GetValue(SymbolSizeProp); 
            set => SetValue(SymbolSizeProp, value);
        }

        public static readonly DependencyProperty SymbolBorderSizeProp = DependencyProperty.Register(
                                                                       "SymbolBorderSize",
                                                                       typeof(double),
                                                                       typeof(Rating),
                                                                       new PropertyMetadata(0.5d, SymbolBorderSizeChanged));

        public double SymbolBorderSize 
        {
            get => (double)GetValue(SymbolBorderSizeProp);
            set => SetValue(SymbolBorderSizeProp, value); 
        }

        public static readonly DependencyProperty readOnlyProp = DependencyProperty.Register("ReadOnly",
                                                                 typeof(bool),
                                                                 typeof(Rating),
                                                                 new PropertyMetadata(false));
        public bool ReadOnly 
        { 
            get => (bool)GetValue(readOnlyProp); 
            set => SetValue(readOnlyProp, value);
        }

        public static readonly DependencyProperty lockAfterRatingProp = DependencyProperty.Register("LockAfterRating",
                                                         typeof(bool),
                                                         typeof(Rating),
                                                         new PropertyMetadata(true));
        public bool LockAfterRating
        {
            get => (bool)GetValue(lockAfterRatingProp);
            set => SetValue(lockAfterRatingProp, value);
        }

        public static readonly DependencyProperty ratingModeProp = DependencyProperty.Register("RatingMode",
                                                                   typeof(RatingMode),
                                                                   typeof(Rating),
                                                                   new PropertyMetadata(RatingMode.Standard));
        public RatingMode RatingMode 
        { 
            get => (RatingMode)GetValue(ratingModeProp);
            set => SetValue(ratingModeProp, value);
        }

        public static readonly DependencyProperty rateProp = DependencyProperty.Register("Rate",
                                                             typeof(double),
                                                             typeof(Rating),
                                                             new PropertyMetadata(0d, RateChanged));
        public double Rate 
        {
            get => (double)GetValue(rateProp);
            set => SetValue(rateProp, value);
        }

        public double PercentRate
        {
            get
            {
                return Rate / Symbols.Count;
            }
        }

        public static readonly DependencyProperty showToolTipProp = DependencyProperty.Register("ShowToolTip",
                                                                    typeof(bool),
                                                                    typeof(Rating),
                                                                    new PropertyMetadata(false));
        public bool ShowToolTip 
        {
            get => (bool)GetValue(showToolTipProp); 
            set => SetValue(showToolTipProp, value);
        }

        public static readonly DependencyProperty symbolProp = DependencyProperty.Register("SymbolType",
                                                                    typeof(Symbols),
                                                                    typeof(Rating),
                                                                    new PropertyMetadata(Admandev.Rating.Symbols.Star, SymbolTypeChanged));  
        public Symbols SymbolType
        {
            get => (Symbols)GetValue(symbolProp);
            set => SetValue(symbolProp, value);
        }

        #endregion

        //Constructor
        public Rating()
        {
            InitializeComponent();

            Symbols = new List<Symbol>();

            AddSymbols(SymbolsCount);
        }

        //Add configured Symbols
        private void AddSymbols(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Symbol Symbol = new Symbol(this)
                {
                    Height = SymbolSize,
                    Width = SymbolSize
                };
                Symbol.SetNormalSymbolColor(NormalSymbolColor);
                Symbol.SetSelectedSymbolColor(SelectedSymbolColor);
                Symbol.SetSymbolBorderSize(SymbolBorderSize);
                Symbol.OnValueChanged += Symbol_OnValueChanged;

                if(Symbols.Count > 0)
                {
                    Symbol.Margin = new Thickness(SymbolSpacing, 0, 0, 0);
                }

                Symbols.Add(Symbol);
                this.SP_Symbols.Children.Add(Symbol);
            }
        }

        //Remove all Symbols from StackPanel
        private void ClearSymbols()
        {
            Symbols.Clear();
            this.SP_Symbols.Children.Clear();
        }

        //Update Symbols values whenever a Symbol value is changing
        private void Symbol_OnValueChanged(Symbol Symbol, double value)
        {
            double tmpRate = value;
            int SymbolIndex = Symbols.IndexOf(Symbol);

            for (int i = 0; i < SymbolIndex; i++)
            {
                tmpRate++;
            }

            FillSymbols(tmpRate);

            this.LB_ToolTip.Content = Math.Round(tmpRate, 2);
        }

        private void FillSymbols(double SymbolsCount)
        {
            if (SymbolsCount > Symbols.Count)
            {
                SymbolsCount = Symbols.Count;
            }

            double tmpRate = SymbolsCount;
            for (int i = 1; i <= Symbols.Count; i++)
            {
                if (tmpRate >= 1)
                {
                    Symbols[i - 1].SetValue(1);
                }
                else if (tmpRate > 0)
                {
                    Symbols[i - 1].SetValue(tmpRate);
                }
                else
                {
                    Symbols[i - 1].SetValue(0);
                }

                tmpRate--;
            }

            this.LB_ToolTip.Content = Math.Abs(Math.Round(tmpRate, 2));
        }

        //vote
        private void Rating_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (LockAfterRating)
            {
                ReadOnly = true;
            }
            
            double rate = 0;
            foreach (Symbol Symbol in Symbols)
            {
                rate += Symbol.Value;
            }

            this.Rate = rate;
            RateEvent?.Invoke(rate, PercentRate);
        }

        //On mouse leave, reset displayed rate 
        private void Rating_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            FillSymbols(Rate);   
        }

        #region Properties events

        private static void NormalSymbolColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Rating)d).OnNormalSymbolColorChanged();
        }

        //Update normal Symbols color
        private void OnNormalSymbolColorChanged()
        {
            foreach (Symbol Symbol in Symbols)
            {
                Symbol.SetNormalSymbolColor(NormalSymbolColor);
            }
        }

        private static void SelectedSymbolColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Rating)d).OnSelectedSymbolColorChanged();
        }

        //Update selected Symbols color
        private void OnSelectedSymbolColorChanged()
        {
            foreach (Symbol Symbol in Symbols)
            {
                Symbol.SetSelectedSymbolColor(SelectedSymbolColor);
            }
        }

        private static void SymbolsCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Rating)d).OnSymbolsCountChanged();
        }

        //Update displayed Symbols count
        private void OnSymbolsCountChanged()
        {
            ClearSymbols();
            AddSymbols(SymbolsCount);
            FillSymbols(Rate);
        }

        private static void SymbolSpacingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Rating)d).OnSymbolSpacingChanged();
        }

        //Update Symbols margins
        private void OnSymbolSpacingChanged()
        {
            for(int i = 0; i < Symbols.Count;  i++)
            {
                if(i > 0)
                {
                    Symbols[i].Margin = new Thickness(SymbolSpacing, 0, 0, 0);
                }
            }
        }

        private static void RateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Rating)d).OnRatePropertyValueChanged();
        }

        //Update Symbols values according the new rate
        private void OnRatePropertyValueChanged()
        {
            FillSymbols(Rate);
        }

        private static void SymbolSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Rating)d).OnSymbolSizeChanged();
        }

        //Update Symbols size
        private void OnSymbolSizeChanged()
        {
            foreach (Symbol Symbol in Symbols)
            {
                Symbol.Height = SymbolSize;
                Symbol.Width = SymbolSize;
            }
        }

        private static void SymbolBorderSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Rating)d).OnSymbolBorderSizeChanged();
        }

        private void OnSymbolBorderSizeChanged()
        {
            foreach (Symbol Symbol in Symbols)
            {
                Symbol.SetSymbolBorderSize(SymbolBorderSize);
            }
        }

        private static void SymbolTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Rating)d).OnSymbolTypeChanged();
        }

        private void OnSymbolTypeChanged()
        {
            foreach (Symbol Symbol in Symbols)
            {
                Symbol.SetSymbolType(SymbolType);
            }
        }

        #endregion

    }
}
