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
        public static IReadOnlyCollection<HistoryDataResponse> ToResponses<TEntity, TFileEntity, TFileSourceEntity>(IEnumerable<TEntity> packages)
            where TEntity : PackageDataEntityBase<TFileEntity, TFileSourceEntity>
            where TFileEntity : FileDataEntityBase<TFileSourceEntity>
            where TFileSourceEntity : FileDataSourceEntityBase =>
            packages.Select(ToResponse<TEntity, TFileEntity, TFileSourceEntity>).ToList();

        /// <summary>
        /// Преобразовать в транспортную модель
        /// </summary>
        public static HistoryDataResponse ToResponse<TEntity, TFileEntity, TFileSourceEntity>(TEntity package)
            where TEntity : PackageDataEntityBase<TFileEntity, TFileSourceEntity>
            where TFileEntity : FileDataEntityBase<TFileSourceEntity>
            where TFileSourceEntity : FileDataSourceEntityBase =>
            new HistoryDataResponse(Guid.Parse(package.Id), package.CreationDateTime, package.IdentityLocalName,
                                    package.StatusProcessingProject, package.FileDataEntities.Count);
    }
}