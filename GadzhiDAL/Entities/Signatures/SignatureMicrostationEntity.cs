using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GadzhiDAL.Entities.FilesConvert.Base;

namespace GadzhiDAL.Entities.Signatures
{
    /// <summary>
    /// Идентификатор личности с подписью для Microstation
    /// </summary>
    public class SignatureMicrostationEntity : EntityBase<int>
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public override int Id { get; protected set; }

        /// <summary>
        /// Наименование базы
        /// </summary>
        public virtual string NameDatabase { get; set; }

        /// <summary>
        /// База подписей Microstation
        /// </summary>       
        public virtual IList<byte> MicrostationDataBase { get; set; }
    }
}
