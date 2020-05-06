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
    public abstract class PackageDataEntityBase : EntityBase<string>
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public override string Id { get; protected set; }

        /// <summary>
        /// Время создания запроса на конвертирование
        /// </summary>
        public virtual DateTime CreationDateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// Идентификация имени локального пользователя
        /// </summary>
        public virtual string IdentityLocalName { get; set; } = String.Empty;

        /// <summary>
        /// Идентификация имени сервера
        /// </summary>
        public virtual string IdentityServerName { get; set; } = String.Empty;

        /// <summary>
        /// Установить идентификатор
        /// </summary>        
        public virtual void SetId(Guid id)
        {
            Id = id.ToString();
        }       
    }
}
