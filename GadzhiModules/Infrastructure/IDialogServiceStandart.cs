using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiModules.Infrastructure
{
    /// <summary>
    /// Стандартные диалоговые окна
    /// </summary>
    public interface IDialogServiceStandard
    {
        /// <summary>
        /// Выбор файлов
        /// </summary>     
        IEnumerable<string> OpenFileDialog(bool isMultiselect, string filter);

        /// <summary>
        /// Выбор папки
        /// </summary>     
        IEnumerable<string> OpenFolderDialog(bool isMultiselect);
    }
}
