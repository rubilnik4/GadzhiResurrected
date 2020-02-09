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
    public class FilesDataService
    {
        /// <summary>
        /// Класс обертка для управления транзакциями
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        public FilesDataService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddFilesData()
        {
            using (_unitOfWork.BeginTransaction())
            {

            }
        }
    }
}
