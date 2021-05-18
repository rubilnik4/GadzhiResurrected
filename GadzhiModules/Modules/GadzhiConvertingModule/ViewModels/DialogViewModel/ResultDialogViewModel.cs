﻿using System.Threading.Tasks;
using GadzhiModules.Modules.GadzhiConvertingModule.ViewModels.Base;
using Prism.Commands;

namespace GadzhiModules.Modules.GadzhiConvertingModule.ViewModels.DialogViewModel
{
    /// <summary>
    /// Диалоговое окно результатов конвертирования
    /// </summary>
    public class ResultDialogViewModel: DialogViewModelBase
    {
        public ResultDialogViewModel(bool hasErrors)
        {
            HasErrors = hasErrors;
        }

        /// <summary>
        /// Наличие ошибок
        /// </summary>
        public bool HasErrors { get; }

        /// <summary>
        /// Отсутствие ошибок
        /// </summary>
        public bool NoErrors => !HasErrors;

        /// <summary>
        /// Заголовок
        /// </summary>
        public override string Title => "Итоги";

        /// <summary>
        /// Сообщение
        /// </summary>
        public string Message => (NoErrors)
                                 ? "Ставь лайк! Файлы готовы"
                                 : "Упс! Что-то пошло не так...";
    }
}