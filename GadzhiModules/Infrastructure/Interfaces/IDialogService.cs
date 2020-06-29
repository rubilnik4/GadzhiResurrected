using GadzhiCommon.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GadzhiCommon.Models.Interfaces.Errors;

namespace GadzhiModules.Infrastructure.Interfaces
{
    /// <summary>
    /// Стандартные диалоговые окна
    /// </summary>
    public interface IDialogService
    {
        /// <summary>
        /// Выбор файлов
        /// </summary>     
        Task<IEnumerable<string>> OpenFileDialog(bool isMultiSelect, string filter);

        /// <summary>
        /// Выбор папки
        /// </summary>     
        Task<IEnumerable<string>> OpenFolderDialog(bool isMultiSelect);

        /// <summary>
        /// Информационное сообщение
        /// </summary>  
        Task ShowMessage(string message);

        /// <summary>
        /// Диалоговое окно с подтверждением
        /// </summary>  
        Task<bool> ShowMessageOkCancel(string messageText);

        /// <summary>
        /// Диалоговое окно с повтором и пропуском
        /// </summary>  
        Task<bool> ShowMessageRetryCancel(string message);

        /// <summary>
        /// Обертка для функции с диалоговым окном повтора
        /// </summary>
        Task RetryOrIgnoreBoolFunction(Func<Task<bool>> asyncFunc, string message);
    }
}
