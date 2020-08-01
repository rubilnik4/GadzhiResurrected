using System.Diagnostics;
using System.Linq;
using GadzhiApplicationCommon.Extensions.StringAdditional;
using Microsoft.Office.Interop.Excel;

namespace GadzhiWord.Factory
{
    /// <summary>
    /// Создание экземпляра приложения Word
    /// </summary>     
    public static class ExcelInstance
    {
        /// <summary>
        /// Создание экземпляра приложения Microstation
        /// </summary>        
        public static Application Instance()
        {
            KillAllPreviousProcess();

            var excelApplication = new Application();
            SetPropertiesToApplication(excelApplication);

            return excelApplication;
        }

        /// <summary>
        /// Параметры приложения 
        /// </summary>       
        private static void SetPropertiesToApplication(Application excelApplication)
        {
            excelApplication.ScreenUpdating = false;
            excelApplication.Visible = false;
            excelApplication.DisplayAlerts = false;
        }

        /// <summary>
        /// Уничтожить все предыдущие процессы
        /// </summary>
        public static void KillAllPreviousProcess()
        {
            var excelProcesses = Process.GetProcesses().Where(process => process.ProcessName.ContainsIgnoreCase("excel"));
            foreach (var wordProcess in excelProcesses)
            {
                wordProcess.Kill();
            }
        }
    }
}