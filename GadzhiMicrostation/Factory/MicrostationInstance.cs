using MicroStationDGN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Factory
{
    internal static class MicrostationInstance
    {
        /// <summary>
        /// Создание экземпляра приложения
        /// </summary>        
        internal static Application Instance()
        {
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
    }
}
