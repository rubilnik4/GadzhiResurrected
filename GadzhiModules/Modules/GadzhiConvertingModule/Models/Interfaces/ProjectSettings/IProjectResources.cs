using System.Collections.Generic;
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
        public NotifyTask<IReadOnlyList<ISignatureLibrary>> PersonSignatures { get; }
    }
}