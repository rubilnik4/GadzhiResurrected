using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiModules.Infrastructure.FabricChannel
{
    /// <summary>
    /// Вызов функций WCF через прокси с параметрами на App.config
    /// </summary>
    public interface IServiceInvoker
    {
        R InvokeService<T, R>(Func<T, R> invokeHandler) where T : class;
    }
}
