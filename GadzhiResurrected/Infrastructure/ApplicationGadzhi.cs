using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhieResurrected.Infrastructure
{
    public class ApplicationGadzhi : IApplicationGadzhi
    {
        /// <summary>
        /// Добавить файлы для конвертации
        /// </summary>
        public async Task AddFromFiles()
        {
           await Task.Delay(2000);
        }

        /// <summary>
        /// Указать папку для конвертации
        /// </summary>     
        public Task AddFromFolder()
        {
            return Task.Delay(2000);
        }

        /// <summary>
        /// Очистить список файлов
        /// </summary>  
        public Task ClearFiles()
        {
            return Task.Delay(2000);
        }

        /// <summary>
        /// Удалить файлы
        /// </summary>
        public Task RemoveFiles()
        {
            return Task.Delay(2000);
        }
    }
}
