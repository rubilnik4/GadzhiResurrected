﻿using GadzhiMicrostation.Models.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Infrastructure.Interface
{
    /// <summary>
    /// Класс обертка для отлова ошибок
    /// </summary> 
    public interface IExecuteAndCatchErrorsMicrostation
    {
        /// <summary>
        ///Отлов ошибок и вызов постметода       
        /// </summary> 
        void ExecuteAndHandleError(Action method,
                                   Action ApplicationBeforeMethod = null,
                                   Action ApplicationCatchMethod = null,
                                   ErrorMicrostation errorMicrostation = null,
                                   Action ApplicationFinallyMethod = null);

    }
}
