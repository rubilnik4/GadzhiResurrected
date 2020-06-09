using MicroStationDGN;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using GadzhiApplicationCommon.Extensions.StringAdditional;

namespace GadzhiMicrostation.Factory
{
    public static class MicrostationInstance
    {
        /// <summary>
        /// Создание экземпляра приложения Microstation
        /// </summary>        
        internal static Application Instance()
        {
            KillAllPreviousProcess();

            var applicationObjectConnector = new ApplicationObjectConnector();
            var microstationApplication = applicationObjectConnector.Application;
            SetPropertiesToApplication(microstationApplication);

            return microstationApplication;
        }

        /// <summary>
        /// Параметры приложения 
        /// </summary>       
        private static void SetPropertiesToApplication(Application microstationApplication)
        {
            microstationApplication.SetCExpressionValue("userPrefsP->extFlags.immediatelySaveChanges", 1); //сохранять изменения
            microstationApplication.SetCExpressionValue("userPrefsP->extFlags.fileDesignOnExit", 0); //отключить диалоговые окна
            microstationApplication.SetCExpressionValue("userPrefsP->extFlags.compressOnExit", 1); //сжать при выходе

            microstationApplication.Visible = true;
        }

        /// <summary>
        /// Уничтожить все предыдущие процессы
        /// </summary>
        public static void KillAllPreviousProcess()
        { 
            var microstationProcesses = Process.GetProcesses().
                                        Where(process => process.ProcessName.ContainsIgnoreCase("ustation"));
            foreach (var microstationProcess in microstationProcesses)
            {
                microstationProcess.Kill();
            }
        }
    }
}
