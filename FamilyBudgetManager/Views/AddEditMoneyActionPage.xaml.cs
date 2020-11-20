﻿using FamilyBudgetManager.ViewModels;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FamilyBudgetManager.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddEditMoneyActionPage : Page
    {
        public AddEditMoneyActionPage()
        {
            this.InitializeComponent();
        }

        private AddEditMoneyActionViewModel _viewModel;
        public AddEditMoneyActionViewModel ViewModel
        {
            get { return _viewModel ?? (_viewModel = (AddEditMoneyActionViewModel)DataContext); }
        }

        private void SumTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string sumText = SumTextBox.Text; //Text that is currently in text box

            foreach (char c in sumText)
            {
                if (c < '0' || c > '9')
                {
                    SumTextBox.BorderBrush= new SolidColorBrush(Windows.UI.Colors.Red);
                    return;
                }
            }

            SumTextBox.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Gray);
        }
    }
}
