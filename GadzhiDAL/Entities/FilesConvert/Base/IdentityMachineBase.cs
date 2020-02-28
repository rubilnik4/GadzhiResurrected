namespace GadzhiDAL.Entities.FilesConvert.Base
{
    /// <summary>
    /// Идентефикация устройства
    /// </summary>
    public abstract class IdentityMachineBase
    {
        public IdentityMachineBase()
        {

        }

        /// <summary>
        /// Идентефикация имени локального пользователя
        /// </summary>
        public virtual string IdentityLocalName { get; set; }

        /// <summary>
        /// Идентефикация имени сервера
        /// </summary>
        public virtual string IdentityServerName { get; set; }

        /// <summary>
        /// Количество попыток конвертирования
        /// </summary>      
        public virtual int AttemptingConvertCount { get; set; }
    }
}
