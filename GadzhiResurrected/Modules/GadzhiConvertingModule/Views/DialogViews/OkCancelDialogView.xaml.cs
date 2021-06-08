using System.Windows.Controls;
using GadzhiResurrected.Modules.GadzhiConvertingModule.ViewModels.DialogViewModel;

namespace GadzhiResurrected.Modules.GadzhiConvertingModule.Views.DialogViews
{
    /// <summary>
    /// Interaction logic for OkCancelDialogView.xaml
    /// </summary>
    public partial class OkCancelDialogView : UserControl
    {
        public OkCancelDialogView(OkCancelDialogViewModel okCancelDialogViewModel)
        {
            InitializeComponent();
            DataContext = okCancelDialogViewModel;
        }
    }
}
