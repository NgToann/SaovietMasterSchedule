using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for UpdateHistoryWindow.xaml
    /// </summary>
    public partial class UpdateHistoryWindow : Window
    {
        string updateInformation;
        string currentVersion;
        public UpdateHistoryWindow(string updateInformation, string currentVersion)
        {
            this.updateInformation = updateInformation;
            this.currentVersion = currentVersion;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtUpdateInformation.Text = updateInformation;
            txtTitle.Text = String.Format("Master Schedule - Version: {0}", currentVersion);
        }

        private void gridTitle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        bool isWiden = false;
        private void window_initiateWiden(object sender, MouseButtonEventArgs e)
        {
            isWiden = true;
        }

        private void window_endWiden(object sender, MouseButtonEventArgs e)
        {
            isWiden = false;

            // Make sure capture is released.
            Rectangle rect = (Rectangle)sender;
            rect.ReleaseMouseCapture();
        }

        private void window_Widen(object sender, MouseEventArgs e)
        {
            Rectangle rect = (Rectangle)sender;
            if (isWiden)
            {
                rect.CaptureMouse();
                double newWidth = e.GetPosition(this).X + 5;
                if (newWidth > 0) this.Width = newWidth;
            } 
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                this.Close();
        }
    }
}
