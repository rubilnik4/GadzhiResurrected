using System.Windows.Controls;
using GadzhiResurrected.Modules.GadzhiConvertingModule.ViewModels.DialogViewModel;

namespace GadzhiResurrected.Modules.GadzhiConvertingModule.Views.DialogViews
{
    /// <summary>
    /// Interaction logic for MessageDialogView.xaml
    /// </summary>
    public partial class MessageDialogView : UserControl
    {
        public MessageDialogView(MessageDialogViewModel messageDialogViewModel)
        {
            InitializeComponent();
            DataContext = messageDialogViewModel;
        }
    }
}
