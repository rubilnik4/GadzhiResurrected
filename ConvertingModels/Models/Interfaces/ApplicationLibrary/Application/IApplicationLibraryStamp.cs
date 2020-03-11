using ConvertingModels.Models.Interfaces.StampCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertingModels.Models.Interfaces.ApplicationLibrary.Application
{
    /// <summary>
    /// Печать в приложении
    /// </summary>
    public interface IApplicationLibraryStamp
    {
        /// <summary>
        /// Найти все штампы во всех моделях и листах
        /// </summary>       
        IStampContainer StampWord { get; }

        /// <summary>
        /// Вставить подписи
        /// </summary>
        void InsertStampSignatures();

        /// <summary>
        /// Удалить подписи
        /// </summary>
        void DeleteStampSignatures();       
    }
}
