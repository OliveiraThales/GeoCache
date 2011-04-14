using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Vishcious.ArcGIS.SLContrib
{
    public class RadialPanel : Panel
    {
        // Dependency properties
        public static readonly DependencyProperty RadiusProperty;
        public static readonly DependencyProperty ItemAlignmentProperty;
        public static readonly DependencyProperty ItemOrientationProperty;

        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        public ItemAlignmentOptions ItemAlignment
        {
            get { return (ItemAlignmentOptions)GetValue(ItemAlignmentProperty); }
            set { SetValue(ItemAlignmentProperty, value); }
        }

        public ItemOrientationOptions ItemOrientation
        {
            get { return (ItemOrientationOptions)GetValue(ItemOrientationProperty); }
            set { SetValue(ItemOrientationProperty, value); }
        }

        private static void RadiusChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            RadialPanel panel = (RadialPanel)sender;
            panel.Refresh(new Size(panel.Width, panel.Height));
        }

        private static void ItemAlignmentChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            RadialPanel panel = (RadialPanel)sender;
            panel.Refresh(new Size(panel.Width, panel.Height));
        }

        private static void ItemOrientationChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            RadialPanel panel = (RadialPanel)sender;
            panel.Refresh(new Size(panel.Width, panel.Height));
        }

        // Static constructor
        static RadialPanel()
        {
            RadiusProperty = DependencyProperty.Register("Radius", typeof(double), typeof(RadialPanel),
                new PropertyMetadata(new PropertyChangedCallback(RadialPanel.RadiusChanged)));
            ItemAlignmentProperty = DependencyProperty.Register("ItemAlignment", typeof(ItemAlignmentOptions), typeof(RadialPanel),
                new PropertyMetadata(new PropertyChangedCallback(RadialPanel.ItemAlignmentChanged)));
            ItemOrientationProperty = DependencyProperty.Register("ItemOrientation", typeof(ItemOrientationOptions), typeof(RadialPanel),
                new PropertyMetadata(new PropertyChangedCallback(RadialPanel.ItemOrientationChanged)));
        }

        // Overrides
        protected override Size MeasureOverride(Size availableSize)
        {
            Size max = new Size(0.0, 0.0);

            // Call Measure on each child and record the maximum
            // width and height of the child elements
            foreach (UIElement element in this.Children)
            {
                element.Measure(availableSize);
                max.Width = Math.Max(max.Width, element.DesiredSize.Width);
                max.Height = Math.Max(max.Height, element.DesiredSize.Height);
            }

            // Compute our own desired size, taking into account the fact
            // that availableSize could specify infinite widths and heights
            // (which are not valid return values)
            double width = double.IsPositiveInfinity(availableSize.Width) ?
                (2.0 * this.Radius) + max.Width : availableSize.Width;
            double height = double.IsPositiveInfinity(availableSize.Height) ?
                (2.0 * this.Radius) + max.Height : availableSize.Height;

            // Return our desired size
            return new Size(width, height);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            // Size and position the child elements
            this.Refresh(finalSize);
            return finalSize;
        }

        // Helper methods
        private void Refresh(Size size)
        {
            int i = 0;
            double inc = 360.0 / this.Children.Count;

            foreach (FrameworkElement element in this.Children)
            {
                double width = 0.0;
                double height = 0.0;

                switch (this.ItemAlignment)
                {
                    case ItemAlignmentOptions.Left:
                        width = 0.0;
                        height = 0.0;
                        break;

                    case ItemAlignmentOptions.Center:
                        width = element.DesiredSize.Width / 2.0;
                        height = element.DesiredSize.Height / 2.0;
                        break;

                    case ItemAlignmentOptions.Right:
                        width = element.DesiredSize.Width;
                        height = element.DesiredSize.Height;
                        break;
                }

                double angle = inc * i++;

                if (this.ItemOrientation == ItemOrientationOptions.Rotated)
                {
                    RotateTransform transform = new RotateTransform();
                    transform.CenterX = width;
                    transform.CenterY = height;
                    transform.Angle = angle;
                    element.RenderTransform = transform;
                }

                double x = this.Radius * Math.Cos((Math.PI * angle) / 180.0);
                double y = this.Radius * Math.Sin((Math.PI * angle) / 180.0);

                element.Arrange(new Rect((x + (size.Width / 2.0)) - width,
                    (y + (size.Height / 2.0)) - height,
                    element.DesiredSize.Width,
                    element.DesiredSize.Height));
            }
        }
    }

    // Enums
    public enum ItemAlignmentOptions
    {
        Left, Center, Right
    }

    public enum ItemOrientationOptions
    {
        Upright, Rotated
    }
}