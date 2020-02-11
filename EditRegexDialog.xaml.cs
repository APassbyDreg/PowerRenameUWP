﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PowerRenameUWP
{
    public sealed partial class EditRegexDialog : ContentDialog
    {
        RegexBlock block = null;
        bool isNameValid = false;
        bool isRegexValid = false;
        string[] regTransferCharacterList = new string[]{"\\", "{", "}", "(", ")", "[", "]", ".", "*", "+", "?", "^", "$", "|"};

        internal EditRegexDialog(RegexBlock block)
        {
            this.block = block;
            this.InitializeComponent();
            foreach (var act in Processes.actions.Keys)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = act;
                process.Items.Add(item);
            }
            process.SelectedIndex = 0;

            if (block != null)
            {
                expTypeSwitch.IsOn = true;
                this.IsPrimaryButtonEnabled = true;
                name.Text = block.name;
                name.IsEnabled = false;
                isNameValid = true;
                regex.Text = block.exp;
                isRegexValid = true;
                comment.Text = block.comment;
                for (int i = 0; i < process.Items.Count; i++)
                {
                    if (Processes.actions[((ComboBoxItem)process.Items[i]).Content.ToString()] == block.process)
                    {
                        process.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (!expTypeSwitch.IsOn)
            {
                foreach (string c in regTransferCharacterList)
                {
                    regex.Text = regex.Text.Replace(c, "\\" + c);
                }
            }

            if (block == null)
            {
                App.datas.regex.add(regex.Text, name.Text, comment.Text, Processes.actions[((ComboBoxItem)process.SelectedItem).Content.ToString()]);
            }
            else
            {
                block.exp = regex.Text;
                block.comment = comment.Text;
                block.process = Processes.actions[((ComboBoxItem)process.SelectedItem).Content.ToString()];
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void name_TextChanged(object sender, TextChangedEventArgs e)
        {
            isNameValid = true;
            if (name.Text.Length == 0)
            {
                nameErrTip.Text = "provide a name";
                isNameValid = false;
            }
            else
            {
                nameErrTip.Text = "name conflict, try another one";
                foreach (var block in App.datas.regex.blocks)
                {
                    if (block.name == name.Text)
                    {
                        isNameValid = false;
                        break;
                    }
                }
            }

            this.IsPrimaryButtonEnabled = isNameValid & isRegexValid;
            if (isNameValid)
            {
                nameErrTip.Visibility = Visibility.Collapsed;
            }
            else
            {
                nameErrTip.Visibility = Visibility.Visible;
            }
        }

        private void regex_TextChanged(object sender, TextChangedEventArgs e)
        {
            isRegexValid = true;
            if (regex.Text.Length == 0)
            {
                regexErrTip.Text = "provide a expression";
                isNameValid = false;
            }
            else if (expTypeSwitch.IsOn)
            {
                try
                {
                    Regex.Match("", regex.Text);
                }
                catch (ArgumentException)
                {
                    isRegexValid = false;
                    regexErrTip.Text = "regex invalid";
                }
            }

            this.IsPrimaryButtonEnabled = isNameValid & isRegexValid;
            if (isRegexValid)
            {
                regexErrTip.Visibility = Visibility.Collapsed;
            }
            else
            {
                regexErrTip.Visibility = Visibility.Visible;
            }
        }

        private void expTypeSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            if (expTypeSwitch.IsOn)
            {
                foreach (string c in regTransferCharacterList)
                {
                    regex.Text = regex.Text.Replace(c, "\\" + c);
                }
            }
            else
            {
                foreach (string c in regTransferCharacterList)
                {
                    regex.Text = regex.Text.Replace("\\" + c, c);
                }
            }
            regex_TextChanged(null, null);
        }
    }
}
