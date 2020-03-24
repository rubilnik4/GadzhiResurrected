using GadzhiWord.Factory;
using GadzhiWord.Word.Interfaces;
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
    public partial class ApplicationWord : IApplicationWord
    {
        /// <summary>
        /// Ресурсы, используемые модулем Word
        /// </summary>
        public WordResources WordResources { get; }

        public ApplicationWord(WordResources wordResources)
        {
            WordResources = wordResources;
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

        /// <summary>
        /// Закрыть приложение
        /// </summary>
        public void CloseApplication()
        {
            _application.Quit();
        }
    }
}
