using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ContactManager.Pages.MasterDetail;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace ContactManager
{
    public sealed partial class Pane : UserControl
    {
        public Pane()
        {
            this.InitializeComponent();
        }

        private void NavigateToMasterDetailPage(object sender, RoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(MasterDetailPage));
        }

        private void NavigateToHome(object sender, RoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(MainPage));
        }
    }
}
