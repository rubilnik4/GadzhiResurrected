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
        public MicrostationDataFileDto(string nameDatabase, byte[] microstationDataBase)
        {
            NameDatabase = nameDatabase;
            MicrostationDataBase = microstationDataBase;
        }

        /// <summary>
        /// Наименование базы
        /// </summary>
        [DataMember]
        public string NameDatabase { get; private set; }

        /// <summary>
        /// База подписей Microstation
        /// </summary>     
        [DataMember]
        public byte[] MicrostationDataBase { get; private set; }
    }
}
