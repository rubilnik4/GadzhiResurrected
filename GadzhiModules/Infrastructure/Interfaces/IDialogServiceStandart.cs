using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiModules.Infrastructure.Interfaces
{
    /// <summary>
    /// Стандартные диалоговые окна
    /// </summary>
    public interface IDialogServiceStandard
    {
        /// <summary>
        /// Выбор файлов
        /// </summary>     
        Task<IEnumerable<string>> OpenFileDialog(bool isMultiselect, string filter);

        /// <summary>
        /// Выбор папки
        /// </summary>     
        Task<IEnumerable<string>> OpenFolderDialog(bool isMultiselect);

        /// <summary>
        /// Информационное сообщение
        /// </summary>   
        void ShowMessage(string messageText);       

        /// <summary>
        /// Диалоговое окно с подтверждением
        /// </summary>  
        bool ShowMessageOkCancel(string messageText);

        /// <summary>
        /// Диалоговое окно с повтором и пропуском
        /// </summary>  
        bool ShowMessageRetryCancel(string messageText);

        /// <summary>
        /// Обертка для функции с диалоговым окном повтора
        /// </summary>
        Task RetryOrIgnoreBoolFunction(Func<Task<bool>> asyncFunc, string messageText);
    }
}
