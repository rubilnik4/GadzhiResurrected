using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiDTOServer.TransferModels.Signatures
{
    /// <summary>
    /// Идентификатор личности с подписью Microstation. Трансферная модель
    /// </summary>
    [DataContract]
    public  class MicrostationDataFileDto
    {
        /// <summary>
        /// Наименование базы
        /// </summary>
        [DataMember]
        public string NameDatabase { get; set; }

        /// <summary>
        /// База подписей Microstation
        /// </summary>     
        [DataMember]
        public byte[] MicrostationDataBase { get; set; }
    }
}
