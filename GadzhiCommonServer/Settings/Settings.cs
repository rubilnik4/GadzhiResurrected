using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiCommonServer.Settings
{
    /// <summary>
    /// Параметры серверной части
    /// </summary>
    public static class Settings
    {
        public static string DataBasePath => System.IO.Directory.GetCurrentDirectory();
    }
}
