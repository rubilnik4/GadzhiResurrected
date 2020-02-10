using GadzhiDAL.Entities.FilesConvert;
using GadzhiDAL.Factories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiDAL.Services.Implementations
{
    /// <summary>
    /// Сервис для добавления и получения данных о конвертируемых пакетах
    /// </summary>
    public class FilesDataService : IFilesDataService
    {
        /// <summary>
        /// Класс обертка для управления транзакциями
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Репозиторий для конвертируемых файлов
        /// </summary>
        private readonly IRepository<FilesDataEntity> _repositoryFilesData;

        public FilesDataService(IUnitOfWork unitOfWork, IRepository<FilesDataEntity> repositoryFilesData)
        {
            _unitOfWork = unitOfWork;
            _repositoryFilesData = repositoryFilesData;
        }

        /// <summary>
        /// Добавить пакет в очередь на конвертирование в базу
        /// </summary>       
        public async Task AddFilesDataAsync(FilesDataEntity filesDataEntity)
        {
            using (_unitOfWork.BeginTransaction())
            {
                await _repositoryFilesData.AddAsync(filesDataEntity);
                await _unitOfWork.CommitAsync();
            }
        }
    }
}
