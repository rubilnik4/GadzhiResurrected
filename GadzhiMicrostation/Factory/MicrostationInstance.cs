using MicroStationDGN;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace GadzhiMicrostation.Factory
{
    internal static class MicrostationInstance
    {
        /// <summary>
        /// Создание экземпляра приложения
        /// </summary>        
        internal static Application Instance()
        {
            KillAllPreviousProcess();

            var applicationObjectConnector = new ApplicationObjectConnector();
            var microstationapplication = applicationObjectConnector.Application;

            SetPropertiesToApplication(microstationapplication);

            return microstationapplication;
        }

        /// <summary>
        /// Параметры приложения 
        /// </summary>       
        private static void SetPropertiesToApplication(Application microstationapplication)
        {
            microstationapplication.SetCExpressionValue("userPrefsP->extFlags.immediatelySaveChanges", 1); //сохранять изменения
            microstationapplication.SetCExpressionValue("userPrefsP->extFlags.fileDesignOnExit", 0); //отключить диалоговые окна
            microstationapplication.SetCExpressionValue("userPrefsP->extFlags.compressOnExit", 1); //сжать при выходе

            microstationapplication.Visible = true;
        }

        private static void KillAllPreviousProcess()
        {
            var microstationProcesses = Process.GetProcesses().
                                                Where(process => process.ProcessName.
                                                                         ToLower(CultureInfo.CurrentCulture).
                                                                         Contains("ustation"));
            foreach (var microstationProcess in microstationProcesses)
            {
                microstationProcess.Kill();
            }
        }
    }
}
