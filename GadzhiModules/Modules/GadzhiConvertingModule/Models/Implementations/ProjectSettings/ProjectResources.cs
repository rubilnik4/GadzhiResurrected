using System.Collections.Generic;
using System.Threading.Tasks;
using GadzhiCommon.Infrastructure.Implementations.Logger;
using GadzhiCommon.Models.Interfaces.LibraryData;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Interfaces.ProjectSettings;
using Nito.Mvvm;

namespace GadzhiModules.Modules.GadzhiConvertingModule.Models.Implementations.ProjectSettings
{
    /// <summary>
    /// Общие данные, используемые в проекте
    /// </summary>
    public class ProjectResources: IProjectResources
    {
       // [Logger]
        public ProjectResources(Task<IReadOnlyList<ISignatureLibrary>> personSignatures)
        {
           PersonSignatures = NotifyTask.Create(personSignatures);
        }

        /// <summary>
        /// Подписи
        /// </summary>
        public NotifyTask<IReadOnlyList<ISignatureLibrary>> PersonSignatures { get; }
    }

}