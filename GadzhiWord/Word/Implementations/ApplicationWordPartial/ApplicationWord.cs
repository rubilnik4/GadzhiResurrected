using ConvertingModels.Models.Interfaces.ApplicationLibrary.Application;
using ConvertingModels.Models.Interfaces.ApplicationLibrary.Document;
using GadzhiWord.Factory;
using GadzhiWord.Word.Implementations.DocumentWordPartial;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiWord.Word.Implementations.ApplicationWordPartial
{
    /// <summary>
    /// Класс для работы с приложением Word
    /// </summary>
    public partial class ApplicationWord: IApplicationLibrary
    {       
        public ApplicationWord()
        {           
        }

        /// <summary>
        /// Экземпляр приложения
        /// </summary>
        private Application _application;

        /// <summary>
        /// Экземпляр приложения
        /// </summary>
        private Application Application
        {
            get
            {
                if (_application == null)
                {
                    _application = WordInstance.Instance();                                       
                }              
                return _application;
            }
        }      

        /// <summary>
        /// Загрузилась ли оболочка Microstation
        /// </summary>
        public bool IsApplicationValid => Application != null;      
    }
}
