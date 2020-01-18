using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiModules.Infrastructure
{
    public class ApplicationGadzhi : IApplicationGadzhi
    {
        public ApplicationGadzhi()
        {
        }
        /// <summary>
        /// Добавить файлы для конвертации
        /// </summary>
        public async Task AddFromFiles()
        {
            //using (var fbd = new FolderBrowserDialog())
            //{
            //    DialogResult result = fbd.ShowDialog();

            //    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
            //    {
            //        string[] files = Directory.GetFiles(fbd.SelectedPath);

            //        System.Windows.Forms.MessageBox.Show("Files found: " + files.Length.ToString(), "Message");
            //    }
            //}
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
