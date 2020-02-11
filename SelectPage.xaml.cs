using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Media.Animation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PowerRenameUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SelectPage : Page
    {
        public SelectPage()
        {
            this.InitializeComponent();
            updateList();
        }

        private void updateList()
        {
            list.Items.Clear();
            foreach(var file in App.datas.files)
            {
                TextBlock item = new TextBlock();
                item.Text = file.path + file.name;
                list.Items.Add(item);
            }
            updateButtons();
        }

        private void updateButtons()
        {
            buttonClear.IsEnabled = (list.Items.Count > 0);
            buttonDelete.IsEnabled = (list.SelectedItems.Count > 0);
            buttonChooseAll.IsEnabled = (list.Items.Count > 0) && (list.SelectedItems.Count < list.Items.Count);
            buttonUnchooseAll.IsEnabled = (list.SelectedItems.Count > 0);
        }

        private async void Select_Files(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.List;
            openPicker.SuggestedStartLocation = PickerLocationId.Desktop;
            openPicker.FileTypeFilter.Add("*");
            IReadOnlyList<StorageFile> files = await openPicker.PickMultipleFilesAsync();
            if (files.Count > 0)
            {
                foreach(var file in files)
                {
                    App.datas.addFile(file.Path, file.Name);
                }
            }
            updateList();
        }

        private void Delete_Selected(object sender, RoutedEventArgs e)
        {
            List<int> indexs = new List<int>();
            foreach (var i in list.SelectedRanges)
            {
                for (int index = i.FirstIndex; index <= i.LastIndex; index++)
                {
                    indexs.Add(index);
                }
            }
            App.datas.deleteFile(indexs);
            updateList();
        }

        private void Clear_All(object sender, RoutedEventArgs e)
        {
            App.datas.files.Clear();
            updateList();
        }

        private void Choose_All(object sender, RoutedEventArgs e)
        {
            list.SelectAll();
        }

        private void Unchoose_All(object sender, RoutedEventArgs e)
        {
            list.SelectedItems.Clear();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            updateButtons();
        }
        
        private void Nav_Next(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(OrganizePage), null, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
        }
    }
}
