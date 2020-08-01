using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GadzhiCommon.Extensions.StringAdditional;

namespace GadzhiWord.Factory
{
    /// <summary>
    /// Создание экземпляра приложения Word
    /// </summary>     
    public static class WordInstance
    {
        /// <summary>
        /// Создание экземпляра приложения Microstation
        /// </summary>        
        public static Application Instance()
        {
            KillAllPreviousProcess();
         
            var wordApplication = new Application();
            SetPropertiesToApplication(wordApplication);

            return wordApplication;
        }

        /// <summary>
        /// Параметры приложения 
        /// </summary>       
        private static void SetPropertiesToApplication(Application wordApplication)
        {           
            wordApplication.ScreenUpdating = false;
            wordApplication.Visible = false;
            wordApplication.Options.AnimateScreenMovements = false;
            wordApplication.Options.UpdateLinksAtOpen = false;
            wordApplication.Options.CheckGrammarAsYouType = false;
            wordApplication.Options.CheckGrammarWithSpelling = false;
            wordApplication.Options.BackgroundSave = false;
            wordApplication.Options.AutoWordSelection = false;
            wordApplication.Options.AllowClickAndTypeMouse = false;
            wordApplication.Options.SmartCursoring = false;
            wordApplication.Options.ContextualSpeller = false;
            wordApplication.Options.CheckSpellingAsYouType = false;          
            wordApplication.DisplayAlerts = WdAlertLevel.wdAlertsNone;          
        }

        /// <summary>
        /// Уничтожить все предыдущие процессы
        /// </summary>
        public static void KillAllPreviousProcess()
        {
            var wordProcesses = Process.GetProcesses().Where(process => process.ProcessName.ContainsIgnoreCase("word"));
            foreach (var wordProcess in wordProcesses)
            {
                wordProcess.Kill();
            }
        }
    }
}
