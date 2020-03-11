using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiConverting.Infrastructure.Interfaces.Application.ApplicationPartial
{
    /// <summary>
    /// Класс для работы с приложением конвертации
    /// </summary>
    public interface IApplicationConverting: IApplicationConvertingDocument, IApplicationConvertingPrinting
    {
       
    }
}
