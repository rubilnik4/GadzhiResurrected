using System.Windows.Controls;
using GadzhiResurrected.Modules.GadzhiConvertingModule.ViewModels.DialogViewModel;

namespace GadzhiResurrected.Modules.GadzhiConvertingModule.Views.DialogViews
{
    /// <summary>
    /// Interaction logic for ResultDialogView.xaml
    /// </summary>
    public partial class ResultDialogView : UserControl
    {
        public ResultDialogView(ResultDialogViewModel resultDialogViewModel)
        {
            InitializeComponent();
            DataContext = resultDialogViewModel;
        }
    }
}
