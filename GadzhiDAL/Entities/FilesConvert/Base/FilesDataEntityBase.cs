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
            IdentityLocalName = "";
            IdentityServerName = "";
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
        /// Идентефикация имени локального пользователя
        /// </summary>
        public virtual string IdentityLocalName { get; set; }

        /// <summary>
        /// Идентефикация имени сервера
        /// </summary>
        public virtual string IdentityServerName { get; set; }

        /// <summary>
        /// Установить идентефикатор
        /// </summary>        
        public virtual void SetId(Guid id)
        {
            Id = id.ToString();
        }       
    }
}
