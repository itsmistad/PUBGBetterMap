using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PUBGBetterMap
{
    // Credit goes to Wiesław Šoltés [@ https://goo.gl/bHP3hc] for the original class, ZoomBorder.
    public class ZoomableElement : Border
    {
        private UIElement child = null;
        private Point origin;
        private Point start;
        private double zoom = 1, scale = 1;

        private TranslateTransform GetTranslateTransform(UIElement element)
        {
            return (TranslateTransform)((TransformGroup)element.RenderTransform)
              .Children.First(tr => tr is TranslateTransform);
        }

        private ScaleTransform GetScaleTransform(UIElement element)
        {
            return (ScaleTransform)((TransformGroup)element.RenderTransform)
              .Children.First(tr => tr is ScaleTransform);
        }

        public override UIElement Child
        {
            get { return base.Child; }
            set
            {
                if (value != null && value != this.Child)
                    this.Initialize(value);
                base.Child = value;
            }
        }

        public void Initialize(UIElement element)
        {
            this.child = element;
            if (child != null)
            {
                TransformGroup group = new TransformGroup();
                ScaleTransform st = new ScaleTransform();
                group.Children.Add(st);
                TranslateTransform tt = new TranslateTransform();
                group.Children.Add(tt);
                child.RenderTransform = group;
                child.RenderTransformOrigin = new Point(0.0, 0.0);
                this.MouseWheel += child_MouseWheel;
                this.MouseLeftButtonDown += child_MouseLeftButtonDown;
                this.MouseLeftButtonUp += child_MouseButtonUp;
                this.MouseMove += child_MouseMove;
                this.MouseRightButtonDown += child_MouseRightButtonDown;
                this.MouseRightButtonUp += child_MouseButtonUp;
            }
        }

        public void Reset()
        {
            if (child != null)
            {
                // reset zoom
                var st = GetScaleTransform(child);
                st.ScaleX = 1.0;
                st.ScaleY = 1.0;

                // reset pan
                var tt = GetTranslateTransform(child);
                tt.X = 0.0;
                tt.Y = 0.0;
            }
        }

        #region Child Events

        private void child_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (child != null)
            {
                var st = GetScaleTransform(child);
                var tt = GetTranslateTransform(child);

                zoom = e.Delta > 0 ? 0.25 : -0.25;
                if ((e.Delta <= 0 && st.ScaleY <= 1) || (e.Delta > 0 && st.ScaleY >= 6))
                    return;

                Point relative = e.GetPosition(child);
                double abosuluteX;
                double abosuluteY;

                abosuluteX = relative.X * st.ScaleX + tt.X;
                abosuluteY = relative.Y * st.ScaleY + tt.Y;
               
                st.ScaleX += zoom;
                st.ScaleY += zoom;

                this.scale = st.ScaleY;
                var scaledX = OverlayWindow.Current.DefaultSize.X * (scale - 1);
                var scaledY = OverlayWindow.Current.DefaultSize.Y * (scale - 1);

                tt.X = abosuluteX - relative.X * st.ScaleX;
                tt.Y = abosuluteY - relative.Y * st.ScaleY;
                if (tt.X > 0) tt.X = 0; else if (tt.X < -scaledX) tt.X = -scaledX;
                if (tt.Y > 0) tt.Y = 0; else if (tt.Y < -scaledY) tt.Y = -scaledY;

                double scaledXSize = OverlayWindow.Current.DefaultSize.X * st.ScaleX;
                double xGrowth = scaledXSize - OverlayWindow.Current.DefaultSize.X;
                //OverlayWindow.Current.Width = Math.Min(scaledXSize, SystemParameters.PrimaryScreenWidth);
                //OverlayWindow.Current.Left = Math.Max(OverlayWindow.Current.DefaultLeft - (xGrowth / 2), 0);
            }
        }

        private void child_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (child != null)
            {
                if (e.RightButton == MouseButtonState.Pressed)
                    return;
                var tt = GetTranslateTransform(child);
                start = e.GetPosition(this);
                origin = new Point(tt.X, tt.Y);
                child.CaptureMouse();
            }
        }

        private void child_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (child != null)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                    return;
                start = e.GetPosition(this);
                child.CaptureMouse();
            }
        }

        private void child_MouseButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (child != null)
            {
                List<UIElement> toRemove = new List<UIElement>();
                foreach (UIElement child in OverlayWindow.Current.Grid.Children)
                    if (child is Line) toRemove.Add(child);
                foreach (UIElement child in toRemove)
                    OverlayWindow.Current.Grid.Children.Remove(child);
                child.ReleaseMouseCapture();
            }
        }

        private void child_MouseMove(object sender, MouseEventArgs e)
        {
            if (child != null)
            {
                if (child.IsMouseCaptured)
                {
                    if (e.LeftButton == MouseButtonState.Pressed)
                    {
                        var tt = GetTranslateTransform(child);
                        Vector v = start - e.GetPosition(this);

                        var scaledX = OverlayWindow.Current.DefaultSize.X * (scale - 1);
                        var scaledY = OverlayWindow.Current.DefaultSize.Y * (scale - 1);

                        tt.X = origin.X - v.X;
                        tt.Y = origin.Y - v.Y;
                        if (tt.X > 0) tt.X = 0; else if (tt.X < -scaledX) tt.X = -scaledX;
                        if (tt.Y > 0) tt.Y = 0; else if (tt.Y < -scaledY) tt.Y = -scaledY;
                    }
                    else if (e.RightButton == MouseButtonState.Pressed)
                    {
                        Line line = new Line();
                        line.Stroke = Brushes.Crimson;
                        line.StrokeThickness = 3;
                        line.X1 = start.X;
                        line.Y1 = start.Y;
                        line.X2 = e.GetPosition(this).X;
                        line.Y2 = e.GetPosition(this).Y;

                        start = e.GetPosition(this);

                        OverlayWindow.Current.Grid.Children.Add(line);
                    }
                }
            }
        }

        #endregion
    }
}
