using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiModules.Modules.FilesConvertModule.Models.Implementations.ReactiveSubjects
{
    /// <summary>
    /// Тип действия для реактивного свойства
    /// </summary>
    public enum ActionType
    {
        Add,
        Remove,
        Clear,
        StatusChange,
    }
}
