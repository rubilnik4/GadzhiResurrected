﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GadzhiModules.Modules.GadzhiConvertingModule.ViewModels.DialogViewModel;

namespace GadzhiModules.Modules.GadzhiConvertingModule.Views.DialogViews
{
    /// <summary>
    /// Interaction logic for SuccessDialogView.xaml
    /// </summary>
    public partial class SuccessDialogView : UserControl
    {
        public SuccessDialogView(SuccessDialogViewModel successDialogViewModel)
        {
            InitializeComponent();
            DataContext = successDialogViewModel;
        }
    }
}
