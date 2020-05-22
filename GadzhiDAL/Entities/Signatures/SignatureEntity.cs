using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using GadzhiDAL.Entities.FilesConvert.Base;

namespace GadzhiDAL.Entities.Signatures
{
    /// <summary>
    /// Идентификатор личности с подписью
    /// </summary>
    public class SignatureEntity : EntityBase<string>
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public override string Id { get; protected set; }

        /// <summary>
        /// Информация о пользователе
        /// </summary>
        public virtual PersonInformationComponent PersonInformation { get; set; }

        /// <summary>
        /// Подпись в формате Jpeg
        /// </summary>       
        public virtual IList<byte> SignatureJpeg { get; set; }

        /// <summary>
        /// Установить идентификатор
        /// </summary>        
        public virtual void SetId(string id)
        {
            Id = id;
        }
    }
}