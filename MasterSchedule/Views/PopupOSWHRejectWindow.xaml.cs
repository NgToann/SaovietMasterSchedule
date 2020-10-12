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
using MasterSchedule.ViewModels;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for PopupOSWHRejectWindow.xaml
    /// </summary>
    public partial class PopupOSWHRejectWindow : Window
    {
        List<NoticeOutsoleWHInventoryViewModel> noticeOSWHInventoryRejectList;
        public PopupOSWHRejectWindow(List<NoticeOutsoleWHInventoryViewModel> noticeOSWHInventoryRejectList)
        {
            this.noticeOSWHInventoryRejectList = noticeOSWHInventoryRejectList;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dgReject.ItemsSource = noticeOSWHInventoryRejectList;
        }
    }
}
