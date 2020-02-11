using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Data.Xml.Dom;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
    public sealed partial class OrganizePage : Page
    {
        Dictionary<string, string> presetsDict = new Dictionary<string, string>();

        public OrganizePage()
        {
            this.InitializeComponent();
            regexList.ItemsSource = App.datas.regex.blocks;
            sortSource.ItemsSource = App.datas.regex.blocks;
            loadPresets();
        }

        private void loadPresets()
        {
            XDocument xdoc = XDocument.Load("Resources/Presets.xml");
            XmlDocument presetsRootXML = new XmlDocument();
            presetsRootXML.LoadXml(xdoc.ToString());
            var presetsList = presetsRootXML.DocumentElement.GetElementsByTagName("preset");
            foreach (IXmlNode presetObj in presetsList)
            {
                MenuFlyoutItem preset = new MenuFlyoutItem();
                XmlDocument presetXML = new XmlDocument();
                presetXML.LoadXml(presetObj.GetXml());
                var presetName = presetXML.DocumentElement.GetElementsByTagName("name")[0].InnerText;
                var presetRegex = presetXML.DocumentElement.GetElementsByTagName("regex")[0].InnerText;
                presetsDict[presetName] = presetRegex;
                preset.Click += (s,e) => { PresetMenuItem_Click(presetName); };
                preset.Text = presetName;
                presets.Items.Add(preset);
            }
        }

        private void Nav_Next(object sender, RoutedEventArgs e)
        {
            regexList.ItemsSource = null;
            this.Frame.Navigate(typeof(RenamePage), null, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
        }

        private void Nav_Previous(object sender, RoutedEventArgs e)
        {
            regexList.ItemsSource = null;
            this.Frame.Navigate(typeof(SelectPage), null, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromLeft });
        }

        private async void Add_Custom(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new EditRegexDialog(null);
            ContentDialogResult result = await dialog.ShowAsync();
        }

        private async void regexList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (regexList.SelectedIndex >= 0)
            {
                ContentDialog dialog = new EditRegexDialog(App.datas.regex.blocks[regexList.SelectedIndex]);
                ContentDialogResult result = await dialog.ShowAsync();
                regexList.SelectedItem = null;
            }
        }

        private void PresetMenuItem_Click(string name)
        {
            int num = 0;
            string _name = name;
            while(true)
            {
                bool isRepeated = false;
                string addon = "";
                if (num > 0) { addon = num.ToString(); }
                foreach (RegexBlock block in App.datas.regex.blocks)
                {
                    if (block.name == name + addon)
                    {
                        isRepeated = true;
                        break;
                    }
                }
                if(!isRepeated) 
                {
                    _name += addon;
                    break; 
                }
                num++;
            }
            var rb = new RegexBlock(presetsDict[name], _name, "", null);
            App.datas.regex.blocks.Add(rb);
        }

        private void Delete_Block(object sender, RoutedEventArgs e)
        {
            var name = ((Button)sender).Tag.ToString();
            foreach (var block in App.datas.regex.blocks)
            {
                if (block.name == name)
                {
                    App.datas.regex.blocks.Remove(block);
                    break;
                }
            }
        }

        private void patternContent_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            flyoutWidth.Width = patternContent.ActualWidth * 0.8;
        }

        private void sortSource_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (App.datas.updateFiles())
            {
                var item = (RegexBlock) sortSource.SelectedItem;
                App.datas.files.Sort(delegate (FileInfo f1, FileInfo f2)
                {
                    int val = String.Compare(f1.attribs[item.name], f2.attribs[item.name]);
                    if (sortRevStatus.SelectedIndex == 1)
                    {
                        val *= -1;
                    }
                    return val;
                });
            }
        }
    }
}
