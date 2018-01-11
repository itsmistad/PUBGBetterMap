using MahApps.Metro.Controls;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using MahApps.Metro;
using System.Collections.Generic;

namespace PUBGBetterMap
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public static MainWindow Current;
        public static List<mrousavy.HotKey> Hotkeys;

        public MainWindow()
        {
            /* 
             * Supported accents:
             *      Red, Green, Blue, Purple, Orange, Lime, Emerald, Teal, 
             *      Cyan, Cobalt, Indigo, Violet, Pink, Magenta, Crimson, 
             *      Amber, Yellow, Brown, Olive, Steel, Mauve, Taupe, Sienna
             *      
             * Supported themes:
             *      BaseLight, BaseDark
             */
            SetTheme("Cyan", "BaseDark");

            InitializeComponent();
            this.AllowsTransparency = true;
            this.Opacity = 0.9;

            MainWindow.Current = this;
            OverlayWindow.Current = new OverlayWindow();
            this.slider_Opacity.Value = 80;
            OverlayWindow.Current.Map.Opacity = this.slider_Opacity.Value / 100;
            Hotkeys = new List<mrousavy.HotKey>();
        }

        #region Theme Mangament
        public Accent CurrentAccent
        {
            get
            {
                return ThemeManager.DetectAppStyle(Application.Current).Item2;
            }
        }

        public AppTheme CurrentTheme
        {
            get
            {
                return ThemeManager.DetectAppStyle(Application.Current).Item1;
            }
        }

        public void SetTheme(string accent, string theme = "BaseLight")
        {
            ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent(accent), ThemeManager.GetAppTheme(theme));

            try
            {
                this.GlowBrush = typeof(Brushes).GetProperties()
                    .FirstOrDefault(x => x.Name == CurrentAccent.Name)
                    .GetValue(null) as Brush;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Uh oh. The glow brush could not be correlated to the accent of the application. Error: " + Environment.NewLine + ex.Message);
                this.GlowBrush = null;
            }
        }
        #endregion

        #region Events
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Hotkeys.Add(new mrousavy.HotKey(ModifierKeys.None, Key.OemTilde, this, delegate 
            {
                if (OverlayWindow.Current.IsVisible) OverlayWindow.Current.Hide(); else OverlayWindow.Current.Show();
            }));
        }

        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            foreach (var hotkey in Hotkeys)
                hotkey.Dispose();
        }

        private void OnClosed(object sender, EventArgs e)
        {
            App.Current.Shutdown();
        }

        private void slider_Opacity_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            OverlayWindow.Current.Map.Opacity = e.NewValue / 100;
        }
        #endregion
    }
}
