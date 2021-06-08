using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GadzhiCommon.Models.Interfaces.Errors;

namespace GadzhiResurrected.Infrastructure.Interfaces
{
    /// <summary>
    /// Стандартные диалоговые окна
    /// </summary>
    public interface IDialogService
    {
        /// <summary>
        /// Выбор файлов
        /// </summary>     
        IReadOnlyCollection<string> OpenFileDialog(bool isMultiSelect, string filter);

        /// <summary>
        /// Выбор папки
        /// </summary>     
        IReadOnlyCollection<string> OpenFolderDialog(bool isMultiSelect);

        /// <summary>
        /// Информационное сообщение
        /// </summary>  
        Task ShowMessage(string message);

        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        Task ShowError(IErrorCommon fileError);

        /// <summary>
        /// Сообщения об ошибках
        /// </summary>
        Task ShowErrors(IEnumerable<IErrorCommon> fileErrors);

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
