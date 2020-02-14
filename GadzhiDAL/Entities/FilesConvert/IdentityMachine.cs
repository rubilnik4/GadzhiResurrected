using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiDAL.Entities.FilesConvert
{
    /// <summary>
    /// Идентефикация устройства
    /// </summary>
    public class IdentityMachine 
    {
        public IdentityMachine()
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
