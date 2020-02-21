using GadzhiCommonServer.Infrastructure.Implementations;
using System.IO;

namespace GadzhiWcfHost.Helpers
{
    public static class HostSystemInformation
    {
        public static string DataBasePath
        {
            get
            {
                string hostPath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
                var directoryPath = new DirectoryInfo(hostPath);
                string dataBasePath = directoryPath.Parent.FullName + "\\" +
                                      SettingsServer.DataBaseDirectoryDefault + "\\" +
                                      SettingsServer.DataBaseNameDefault;
                return dataBasePath;
            }
        }
    }
}