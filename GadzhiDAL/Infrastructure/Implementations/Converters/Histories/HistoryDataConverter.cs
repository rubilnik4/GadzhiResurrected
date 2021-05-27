using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiDAL.Entities.FilesConvert;
using GadzhiDTOBase.TransferModels.Histories;

namespace GadzhiDAL.Infrastructure.Implementations.Converters.Histories
{
    /// <summary>
    /// Преобразование данных истории
    /// </summary>
    public static class HistoryDataConverter
    {
        /// <summary>
        /// Преобразовать в транспортные модели
        /// </summary>
        public static IReadOnlyCollection<HistoryDataResponse> ToResponses(IEnumerable<PackageDataEntity> packages) =>
            packages.Select(ToResponse).ToList();

        /// <summary>
        /// Преобразовать в транспортную модель
        /// </summary>
        public static HistoryDataResponse ToResponse(PackageDataEntity package) =>
            new HistoryDataResponse(Guid.Parse(package.Id), package.CreationDateTime, package.IdentityLocalName,
                                    package.StatusProcessingProject, package.FileDataEntities.Count);
    }
}