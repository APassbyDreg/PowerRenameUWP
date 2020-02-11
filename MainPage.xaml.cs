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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

//The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PowerRenameUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            NavListBox.SelectedIndex = 1;
            Nav(null, null);
        }

        private void Show_Menu(object sender, RoutedEventArgs e)
        {
            MainSplitView.IsPaneOpen = !MainSplitView.IsPaneOpen;
        }

        private void Open_Settings(object sender, RoutedEventArgs e)
        {
            NavListBox.SelectedItem = null;

        }

        private void Nav(object sender, SelectionChangedEventArgs e)
        {
            switch (NavListBox.SelectedIndex)
            {
                case 0:
                    NavListBox.SelectedIndex = 1;
                    break;
                case 1: // nav to "select"
                    ContentFrame.Navigate(typeof(SelectPage), null, new DrillInNavigationTransitionInfo());
                    break;
                case 2: // nav to "sort"
                    ContentFrame.Navigate(typeof(OrganizePage), null, new DrillInNavigationTransitionInfo());
                    break;
                case 3: // nav to "edit"
                    ContentFrame.Navigate(typeof(RenamePage), null, new DrillInNavigationTransitionInfo());
                    break;
                default:
                    break;
            }
        }

        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            switch (e.SourcePageType.Name)
            {
                case "SelectPage":
                    NavListBox.SelectedIndex = 1;
                    break;
                case "OrganizePage":
                    NavListBox.SelectedIndex = 2;
                    break;
                case "RenamePage":
                    NavListBox.SelectedIndex = 3;
                    break;
                default:
                    NavListBox.SelectedItem = null;
                    break;
            }
        }
    }
}
