using System.Collections.Generic;
using GadzhiDAL.Entities.FilesConvert.Base;

namespace GadzhiDAL.Models.Implementations
{
    /// <summary>
    /// Идентификатор личности с подписью для Microstation
    /// </summary>
    public class MicrostationDataFileEntity : EntityBase<string>
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public override string Id { get; protected set; }

        /// <summary>
        /// Наименование базы
        /// </summary>
        public virtual string NameDatabase { get; set; }

        /// <summary>
        /// База подписей Microstation
        /// </summary>       
        public virtual IList<byte> MicrostationDataBase { get; set; }

        /// <summary>
        /// Установить идентификатор
        /// </summary>        
        public virtual void SetId(string id)
        {
            Id = id;
        }
    }
}
