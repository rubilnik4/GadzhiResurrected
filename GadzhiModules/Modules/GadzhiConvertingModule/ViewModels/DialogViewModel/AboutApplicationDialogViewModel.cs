using GadzhiModules.Modules.GadzhiConvertingModule.Models.Enums.DialogViewModel;
using GadzhiModules.Modules.GadzhiConvertingModule.ViewModels.Base;
using Prism.Commands;

namespace GadzhiModules.Modules.GadzhiConvertingModule.ViewModels.DialogViewModel
{
    /// <summary>
    /// О приложении
    /// </summary>
    public class AboutApplicationDialogViewModel: DialogViewModelBase
    {
        /// <summary>
        /// Заголовок
        /// </summary>
        public override string Title => "О программе";
    }
}