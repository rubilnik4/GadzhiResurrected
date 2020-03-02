﻿using GadzhiCommon.Extentions.StringAdditional;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        internal static Application Instance()
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
            wordApplication.Visible = true;
        }

        /// <summary>
        /// Уничтожить все предыдущие процессы
        /// </summary>
        private static void KillAllPreviousProcess()
        {
            var wordProcesses = Process.GetProcesses().Where(process => process.ProcessName.ContainsIgnoreCase("winword"));
            foreach (var wordProcess in wordProcesses)
            {
                wordProcess.Kill();
            }
        }
    }
}
