using System.Collections.Generic;
using System.Threading.Tasks;
using GadzhiCommon.Enums.LibraryData;
using GadzhiCommon.Models.Interfaces.LibraryData;

namespace GadzhiModules.Infrastructure.Interfaces.ApplicationGadzhi
{
    public interface IApplicationGadzhiServices
    {
        /// <summary>
        /// Конвертировать файлы на сервере
        /// </summary>
        Task ConvertingFiles();

        /// <summary>
        /// Сбросить индикаторы конвертации
        /// </summary>
        Task AbortPropertiesConverting(bool isDispose = false);

        /// <summary>
        /// Загрузить подписи из базы данных
        /// </summary>
        Task<IReadOnlyList<ISignatureLibrary>> GetSignaturesNames();

        /// <summary>
        /// Загрузить отделы из базы данных
        /// </summary>
        Task<IList<DepartmentType>> GetSignaturesDepartments();
    }
}