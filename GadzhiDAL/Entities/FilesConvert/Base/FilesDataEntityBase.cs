using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommonServer.Enums;
using GadzhiDAL.Entities.FilesConvert.Main;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GadzhiDAL.Entities.FilesConvert.Base
{
    /// <summary>
    /// Класс содержащий данные о конвертируемых файлах в базе данных
    /// </summary>
    public abstract class FilesDataEntityBase : EntityBase<string>
    {
        public FilesDataEntityBase()
        {
            CreationDateTime = DateTime.Now;           
            IdentityMachine = new IdentityMachine()
            {
                AttemptingConvertCount = 0,
                IdentityLocalName = "",
                IdentityServerName = "",
            };
        }

        /// <summary>
        /// Идентефикатор
        /// </summary>
        public override string Id { get; protected set; }

        /// <summary>
        /// Время создания запроса на конвертирование
        /// </summary>
        public virtual DateTime CreationDateTime { get; set; }      

        /// <summary>
        /// Идентефикация устройства
        /// </summary>
        public virtual IdentityMachine IdentityMachine { get; set; }        
    }
}
