using System.Collections.Generic;

// ReSharper disable VirtualMemberCallInConstructor

namespace GadzhiDAL.Entities.Signatures
{
    /// <summary>
    /// Идентификатор личности с подписью для Microstation
    /// </summary>
    public class MicrostationDataFileEntity 
    {
        public MicrostationDataFileEntity()
        { }

        public MicrostationDataFileEntity(string id, string nameDatabase, IList<byte> microstationDataBase)
        {
            Id = id;
            NameDatabase = nameDatabase;
            MicrostationDataBase = microstationDataBase;
        }
        /// <summary>
        /// Идентификатор
        /// </summary>
        public virtual string Id { get; protected set; }

        /// <summary>
        /// Наименование базы
        /// </summary>
        public virtual string NameDatabase { get; protected set; }

        /// <summary>
        /// База подписей Microstation
        /// </summary>       
        public virtual IList<byte> MicrostationDataBase { get; protected set; }
    }
}
