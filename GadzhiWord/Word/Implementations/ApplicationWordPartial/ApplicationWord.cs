using ConvertingModels.Models.Interfaces.ApplicationLibrary;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiWord.Factory;
using GadzhiWord.Infrastructure.Interfaces;
using GadzhiWord.Models.Interfaces;
using GadzhiWord.Word.Implementations.DocumentWordPartial;
using GadzhiWord.Word.Interfaces.ApplicationWordPartial;
using GadzhiWord.Word.Interfaces.DocumentWordPartial;
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
                    WordInstance.Instance();                                       
                }              
                return _application;
            }
        }

        /// <summary>
        /// Текущий документ Word
        /// </summary>
        public IDocumentWord ActiveDocument => new DocumentWord(_application.ActiveDocument, this);

        /// <summary>
        /// Загрузилась ли оболочка Microstation
        /// </summary>
        public bool IsApplicationValid => Application != null;
    }
}
