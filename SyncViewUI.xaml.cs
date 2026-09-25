using RWLinkNotificationService.Services;
using SyncBuisnessLogic.BuisnessLogic;
using SyncUI.SyncView;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace SyncUI
{
    /// <summary>
    /// Interaction logic for SyncViewUI.xaml
    /// </summary>
    public partial class SyncViewUI : UserControl
    {
        public SyncViewUI(INotificationService notificationService)
        {
            InitializeComponent();
            DataContext = SyncService.GetInstance(notificationService);
        }
    }
}
