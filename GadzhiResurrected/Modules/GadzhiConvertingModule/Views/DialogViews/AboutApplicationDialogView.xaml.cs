using System.Windows.Controls;
using GadzhiResurrected.Modules.GadzhiConvertingModule.ViewModels.DialogViewModel;

namespace GadzhiResurrected.Modules.GadzhiConvertingModule.Views.DialogViews
{
    /// <summary>
    /// Interaction logic for AboutApplicationDialogView.xaml
    /// </summary>
    public partial class AboutApplicationDialogView : UserControl
    {
        public AboutApplicationDialogView(AboutApplicationDialogViewModel aboutApplicationDialogViewModel)
        {
            InitializeComponent();
            DataContext = aboutApplicationDialogViewModel;
        }
    }
}
