using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiDAL.Entities.FilesConvert.Base;
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
        public static IReadOnlyCollection<HistoryDataResponse> ToResponses<TEntity>(IEnumerable<TEntity> packages)
            where TEntity : PackageDataEntityBase =>
            packages.Select(ToResponse).ToList();

        /// <summary>
        /// Преобразовать в транспортную модель
        /// </summary>
        public static HistoryDataResponse ToResponse<TEntity>(TEntity package)
            where TEntity : PackageDataEntityBase =>
            new HistoryDataResponse(Guid.Parse(package.Id), package.CreationDateTime, package.IdentityLocalName,
                                    package.StatusProcessingProject, 0);
    }
}