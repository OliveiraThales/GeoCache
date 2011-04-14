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
using System.Collections.Generic;
using System.Linq;

namespace Vishcious.ArcGIS.SLContrib
{
    public class StackPanel : Panel
    {

        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(StackPanel), new PropertyMetadata(Orientation.Vertical, OrientationPropertyChanged));

        static void OrientationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((StackPanel)d).OrientationChanged((Orientation)e.NewValue);
        }

        void OrientationChanged(Orientation newValue){
            InvalidateMeasure();
            InvalidateArrange();
        }

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set
            { SetValue(OrientationProperty, value); }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            // add up space used by non-stretching elements
            var AutoChildren = Children.Where(o => GetLength((DependencyObject)o).IsAuto);
            var StarChildren = Children.Where(o => GetLength((DependencyObject)o).IsStar);
            var AbsoluteChildren = Children.Where(o => GetLength((DependencyObject)o).IsAbsolute);

            if (Orientation == Orientation.Vertical)
            {
                // measure all auto children to get their desired size
                foreach (UIElement child in AutoChildren)
                {
                    child.Measure(new Size(availableSize.Width, double.PositiveInfinity));
                }

                double AutoSize = AutoChildren.Sum(o => o.DesiredSize.Height);
                double AbsoluteSize = AbsoluteChildren.Sum(o => GetLength((DependencyObject)o).Value);
                // get remaining space to be shared by all stretching elements
                double StarSize = availableSize.Height - AutoSize - AbsoluteSize;
                double StarUnitSize = StarSize / StarChildren.Sum(o => GetLength((DependencyObject)o).Value);

                // loop thru and arrange everyone
                Size sizeSoFar = new Size();
                foreach (UIElement child in Children)
                {
                    double ChildLength;
                    GridLength size = GetLength((DependencyObject)child);

                    if (size.IsAuto)
                        ChildLength = child.DesiredSize.Height;
                    else if (size.IsAbsolute)
                        ChildLength = size.Value;
                    else
                        ChildLength = StarUnitSize * size.Value;

                    child.Measure(new Size(availableSize.Width, ChildLength));
                    sizeSoFar.Height += ChildLength;
                    sizeSoFar.Width = Math.Max(child.DesiredSize.Width, sizeSoFar.Width);
                }

                return new Size(sizeSoFar.Width, base.MeasureOverride(availableSize).Height);
            }
            else
            {
                // measure all auto children to get their desired size
                foreach (UIElement child in AutoChildren)
                {
                    child.Measure(new Size(double.PositiveInfinity, availableSize.Height));
                }

                double AutoSize = AutoChildren.Sum(o => o.DesiredSize.Width);
                double AbsoluteSize = AbsoluteChildren.Sum(o => GetLength((DependencyObject)o).Value);
                // get remaining space to be shared by all stretching elements
                double StarSize = availableSize.Width - AutoSize - AbsoluteSize;
                double StarUnitSize = StarSize / StarChildren.Sum(o => GetLength((DependencyObject)o).Value);

                // loop thru and arrange everyone
                Size sizeSoFar = new Size();
                foreach (UIElement child in Children)
                {
                    double ChildLength;
                    GridLength size = GetLength((DependencyObject)child);

                    if (size.IsAuto)
                        ChildLength = child.DesiredSize.Width;
                    else if (size.IsAbsolute)
                        ChildLength = size.Value;
                    else
                        ChildLength = StarUnitSize * size.Value;

                    child.Measure(new Size(ChildLength, availableSize.Height));
                    sizeSoFar.Width += ChildLength;
                    sizeSoFar.Height = Math.Max(child.DesiredSize.Height, sizeSoFar.Height);
                }

                return new Size(base.MeasureOverride(availableSize).Width, sizeSoFar.Height);
            }
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            Size sizeSoFar = new Size();

            // add up space used by non-stretching elements
            var AutoChildren = Children.Where(o => GetLength((DependencyObject)o).IsAuto);
            var StarChildren = Children.Where(o => GetLength((DependencyObject)o).IsStar);
            var AbsoluteChildren = Children.Where(o => GetLength((DependencyObject)o).IsAbsolute);

            if (Orientation == Orientation.Vertical)
            {
                double AutoSize = AutoChildren.Sum(o => o.DesiredSize.Height);
                double AbsoluteSize = AbsoluteChildren.Sum(o => GetLength((DependencyObject)o).Value);
                // get remaining space to be shared by all stretching elements
                double StarSize = finalSize.Height - AutoSize - AbsoluteSize;
                double StarUnitSize = StarSize / StarChildren.Sum(o => GetLength((DependencyObject)o).Value);

                // loop thru and arrange everyone
                foreach (UIElement child in Children)
                {
                    double ChildLength;
                    GridLength size = GetLength((DependencyObject)child);

                    if (size.IsAuto)
                        ChildLength = child.DesiredSize.Height;
                    else if (size.IsAbsolute)
                        ChildLength = size.Value;
                    else
                        ChildLength = StarUnitSize * size.Value;

                    child.Arrange(new Rect(0, sizeSoFar.Height, finalSize.Width, ChildLength));
                    sizeSoFar.Height += ChildLength;
                }
            }
            else {
                double AutoSize = AutoChildren.Sum(o => o.DesiredSize.Width);
                double AbsoluteSize = AbsoluteChildren.Sum(o => GetLength((DependencyObject)o).Value);
                // get remaining space to be shared by all stretching elements
                double StarSize = finalSize.Width - AutoSize - AbsoluteSize;
                double StarUnitSize = StarSize / StarChildren.Sum(o => GetLength((DependencyObject)o).Value);

                // loop thru and arrange everyone
                foreach (UIElement child in Children)
                {
                    double ChildLength;
                    GridLength size = GetLength((DependencyObject)child);

                    if (size.IsAuto)
                        ChildLength = child.DesiredSize.Width;
                    else if (size.IsAbsolute)
                        ChildLength = size.Value;
                    else
                        ChildLength = StarUnitSize * size.Value;

                    child.Arrange(new Rect(sizeSoFar.Width, 0, ChildLength, finalSize.Height));
                    sizeSoFar.Width += ChildLength;
                }
            }

            return base.ArrangeOverride(finalSize);
        }

        public static readonly DependencyProperty LengthProperty = DependencyProperty.RegisterAttached("Length", typeof(GridLength), typeof(StackPanel), null);

        public static void SetLength(DependencyObject obj, GridLength Length)
        {
            obj.SetValue(LengthProperty, Length);
        }

        public static GridLength GetLength(DependencyObject obj)
        {
            return (GridLength)obj.GetValue(LengthProperty);
        }
    }
}
