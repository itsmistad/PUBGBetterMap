using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PUBGBetterMap
{
    /// <summary>
    /// Interaction logic for OverlayWindow.xaml
    /// </summary>
    public partial class OverlayWindow : Window
    {
        public static OverlayWindow Current;
        public Point DefaultSize;
        public double DefaultLeft;
        public bool IsActivated;

        public OverlayWindow()
        {
            InitializeComponent();

            this.WindowStyle = WindowStyle.None;
            this.AllowsTransparency = true;
            this.ResizeMode = ResizeMode.NoResize;
            this.Top = 0;
            this.Left = 0;
            this.Width = 0;
            this.Height = 0;
            if (SystemParameters.PrimaryScreenWidth == 1920 && SystemParameters.PrimaryScreenHeight == 1080)
            {
                this.Left = this.DefaultLeft = 412;
                DefaultSize = new Point(1096, 1080);
                this.Width = 1096;
                this.Height = 1080;
            }
            else if (SystemParameters.PrimaryScreenWidth == 2560 && SystemParameters.PrimaryScreenHeight == 1080)
            {
                this.Left = this.DefaultLeft = 732;
                DefaultSize = new Point(1096, 1080);
                this.Width = 1096;
                this.Height = 1080;
            }
            else
                MessageBox.Show("Your current resolution is not yet supported.");

            OverlayWindow.Current = this;
        }
    }
}
