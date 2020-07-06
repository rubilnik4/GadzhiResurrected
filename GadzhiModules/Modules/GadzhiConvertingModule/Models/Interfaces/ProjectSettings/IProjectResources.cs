using System.Collections.Generic;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiCommon.Models.Interfaces.LibraryData;
using Nito.Mvvm;

namespace GadzhiModules.Modules.GadzhiConvertingModule.Models.Interfaces.ProjectSettings
{
    /// <summary>
    /// Общие данные, используемые в проекте
    /// </summary>
    public interface IProjectResources
    {
        /// <summary>
        /// Подписи
        /// </summary>
        public NotifyTask<IResultCollection<ISignatureLibrary>> PersonSignatures { get; }
    }
}