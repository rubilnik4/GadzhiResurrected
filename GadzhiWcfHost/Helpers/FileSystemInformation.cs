using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GadzhiWcfHost.Helpers
{
    public static class FileSystemInformation
    {
        public static string ApplicationPath => System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;

        public static string ConvertionDirectory => ApplicationPath + "ConvertingDirectory.gitignore\\";
    }
}