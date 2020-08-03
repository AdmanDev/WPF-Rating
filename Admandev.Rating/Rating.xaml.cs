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
        private readonly List<Star> stars;

        //Delegate
        public delegate void RateDelegate(double rate, double percentRate);

        //Event
        public event RateDelegate RateEvent;

        #region Properties

        public static readonly DependencyProperty normalStarColorProp = DependencyProperty.Register("NormalStarColor",
                                                                  typeof(Brush),
                                                                  typeof(Rating),
                                                                  new PropertyMetadata(
                                                                      new SolidColorBrush(Colors.Transparent),
                                                                      NormalStarColorChanged));

        public Brush NormalStarColor
        {
            get => (Brush)GetValue(normalStarColorProp);
            set => SetValue(normalStarColorProp, value);
        }

        public static readonly DependencyProperty selectedStarColorProp = DependencyProperty.Register("SelectedStarColor",
                                                                  typeof(Brush),
                                                                  typeof(Rating),
                                                                  new PropertyMetadata(
                                                                      new SolidColorBrush(Colors.Red),
                                                                      SelectedStarColorChanged));

        public Brush SelectedStarColor 
        { 
            get => (Brush)GetValue(selectedStarColorProp);
            set => SetValue(selectedStarColorProp, value);
        }

        public static readonly DependencyProperty starCountProp = DependencyProperty.Register("StarsCount",
                                                                typeof(int),
                                                                typeof(Rating),
                                                                new PropertyMetadata(5, StarsCountChanged));
        public int StarsCount 
        {
            get => (int)GetValue(starCountProp);
            set => SetValue(starCountProp, value);
        }

        public static readonly DependencyProperty starSpacingProp = DependencyProperty.Register("StarSpacing",
                                                                    typeof(double),
                                                                    typeof(Rating),
                                                                    new PropertyMetadata(5d, StarSpacingChanged));
        public double StarSpacing 
        {
            get => (double)GetValue(starSpacingProp);
            set => SetValue(starSpacingProp, value);
        }

        public static readonly DependencyProperty starSizeProp = DependencyProperty.Register("StarSize",
                                                                 typeof(double),
                                                                 typeof(Rating),
                                                                 new PropertyMetadata(40d, StarSizeChanged));
        public double StarSize 
        {
            get => (double)GetValue(starSizeProp); 
            set => SetValue(starSizeProp, value);
        }

        public static readonly DependencyProperty starBorderSizeProp = DependencyProperty.Register(
                                                                       "StarBorderSize",
                                                                       typeof(double),
                                                                       typeof(Rating),
                                                                       new PropertyMetadata(0.5d, StarBorderSizeChanged));

        public double StarBorderSize 
        {
            get => (double)GetValue(starBorderSizeProp);
            set => SetValue(starBorderSizeProp, value); 
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
                return Rate / stars.Count;
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

        #endregion

        //Constructor
        public Rating()
        {
            InitializeComponent();

            stars = new List<Star>();

            AddStars(StarsCount);
        }

        //Add configured stars
        private void AddStars(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Star star = new Star(this)
                {
                    Height = StarSize,
                    Width = StarSize
                };
                star.SetNormalStarColor(NormalStarColor);
                star.SetSelectedStarColor(SelectedStarColor);
                star.SetStarBorderSize(StarBorderSize);
                star.OnValueChanged += Star_OnValueChanged;

                if(stars.Count > 0)
                {
                    star.Margin = new Thickness(StarSpacing, 0, 0, 0);
                }

                stars.Add(star);
                this.SP_Stars.Children.Add(star);
            }
        }

        //Remove all stars from StackPanel
        private void ClearStars()
        {
            stars.Clear();
            this.SP_Stars.Children.Clear();
        }

        //Update stars values whenever a star value is changing
        private void Star_OnValueChanged(Star star, double value)
        {
            double tmpRate = value;
            int starIndex = stars.IndexOf(star);

            for (int i = 0; i < starIndex; i++)
            {
                tmpRate++;
            }

            FillStars(tmpRate);

            this.LB_ToolTip.Content = Math.Round(tmpRate, 2);
        }

        private void FillStars(double starsCount)
        {
            if (starsCount > stars.Count)
            {
                starsCount = stars.Count;
            }

            double tmpRate = starsCount;
            for (int i = 1; i <= stars.Count; i++)
            {
                if (tmpRate >= 1)
                {
                    stars[i - 1].SetValue(1);
                }
                else if (tmpRate > 0)
                {
                    stars[i - 1].SetValue(tmpRate);
                }
                else
                {
                    stars[i - 1].SetValue(0);
                }

                tmpRate--;
            }

            this.LB_ToolTip.Content = Math.Abs(Math.Round(tmpRate, 2));
        }

        //vote
        private void Rating_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ReadOnly = true;

            double rate = 0;
            foreach (Star star in stars)
            {
                rate += star.Value;
            }

            this.Rate = rate;
            RateEvent.Invoke(rate, PercentRate);
        }

        //On mouse leave, reset displayed rate 
        private void Rating_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            FillStars(Rate);   
        }

        #region Properties events

        private static void NormalStarColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Rating)d).OnNormalStarColorChanged();
        }

        //Update normal stars color
        private void OnNormalStarColorChanged()
        {
            foreach (Star star in stars)
            {
                star.SetNormalStarColor(NormalStarColor);
            }
        }

        private static void SelectedStarColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Rating)d).OnSelectedStarColorChanged();
        }

        //Update selected stars color
        private void OnSelectedStarColorChanged()
        {
            foreach (Star star in stars)
            {
                star.SetSelectedStarColor(SelectedStarColor);
            }
        }

        private static void StarsCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Rating)d).OnStarsCountChanged();
        }

        //Update displayed stars count
        private void OnStarsCountChanged()
        {
            ClearStars();
            AddStars(StarsCount);
            FillStars(Rate);
        }

        private static void StarSpacingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Rating)d).OnStarSpacingChanged();
        }

        //Update stars margins
        private void OnStarSpacingChanged()
        {
            for(int i = 0; i < stars.Count;  i++)
            {
                if(i > 0)
                {
                    stars[i].Margin = new Thickness(StarSpacing, 0, 0, 0);
                }
            }
        }

        private static void RateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Rating)d).OnRatePropertyValueChanged();
        }

        //Update stars values according the new rate
        private void OnRatePropertyValueChanged()
        {
            FillStars(Rate);
        }

        private static void StarSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Rating)d).OnStarSizeChanged();
        }

        //Update stars size
        private void OnStarSizeChanged()
        {
            foreach (Star star in stars)
            {
                star.Height = StarSize;
                star.Width = StarSize;
            }
        }

        private static void StarBorderSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Rating)d).OnStarBorderSizeChanged();
        }

        private void OnStarBorderSizeChanged()
        {
            foreach (Star star in stars)
            {
                star.SetStarBorderSize(StarBorderSize);
            }
        }

        #endregion

    }
}
