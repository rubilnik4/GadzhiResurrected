using System.Threading.Tasks;
using GadzhiCommon.Infrastructure.Implementations.Logger;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiCommon.Models.Interfaces.LibraryData;
using GadzhiResurrected.Modules.GadzhiConvertingModule.Models.Interfaces.ProjectSettings;
using Nito.Mvvm;

namespace GadzhiResurrected.Modules.GadzhiConvertingModule.Models.Implementations.ProjectSettings
{
    /// <summary>
    /// Общие данные, используемые в проекте
    /// </summary>
    public class ProjectResources: IProjectResources
    {
        public ProjectResources(Task<IResultCollection<ISignatureLibrary>> personSignatures)
        {
           PersonSignatures = NotifyTask.Create(personSignatures);
        }

        /// <summary>
        /// Подписи
        /// </summary>
        [Logger]
        public NotifyTask<IResultCollection<ISignatureLibrary>> PersonSignatures { get; }
    }

}