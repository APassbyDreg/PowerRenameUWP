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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PowerRenameUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RenamePage : Page
    {
        public RenamePage()
        {
            this.InitializeComponent();
            if (App.datas.files.Count == 0)
            {
                showWarning("please select some files first", typeof(SelectPage));
            }
            else if (App.datas.regex.blocks.Count == 0 || !App.datas.updateFiles())
            {
                showWarning("please check your regular expression", typeof(OrganizePage));
            }
            patternList.ItemsSource = App.datas.regex.blocks;
            sortSource.ItemsSource = App.datas.regex.blocks;
            previewList.ItemsSource = App.datas.files;
            loadSortStatus();
        }

        private void loadSortStatus()
        {
            for (int i = 0; i < App.datas.regex.blocks.Count; i++)
            {
                if (App.datas.regex.blocks[i].name == App.datas.sortAttrib)
                {
                    sortSource.SelectedItem = App.datas.regex.blocks[i];
                    break;
                }
            }
            if (App.datas.sortRevStatus)
            {
                sortRevStatus.SelectedIndex = 1;
            }
        }

        private async void showWarning(string str, Type dest)
        {
            ContentDialog dialog = new WarningDialog(str);
            ContentDialogResult result = await dialog.ShowAsync();
            if (dest != null)
            {
                this.Frame.Navigate(dest, null, new DrillInNavigationTransitionInfo());
            }
        }

        private void refreshPreview()
        {
            App.datas.updateFiles();
            previewList.ItemsSource = null;
            previewList.ItemsSource = App.datas.files;
        }

        private void Nav_Previous(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(OrganizePage), null, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromLeft });
        }

        private void sortProfile_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sortSource.SelectedItem != null)
            {
                var item = (RegexBlock)sortSource.SelectedItem;
                App.datas.sortAttrib = item.name;
                App.datas.sortRevStatus = sortRevStatus.SelectedIndex == 1;
            }
            if (e.RemovedItems.Count != 0)
            {
                refreshPreview();
            }
        }
    }
}
