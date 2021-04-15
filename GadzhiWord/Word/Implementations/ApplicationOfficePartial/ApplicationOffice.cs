using System;
using System.Runtime.InteropServices;
using GadzhiApplicationCommon.Models.Implementation.Resources;
using GadzhiWord.Factory;
using GadzhiWord.Word.Interfaces;
using InteropWord = Microsoft.Office.Interop.Word.Application;
using InteropExcel = Microsoft.Office.Interop.Excel.Application;

namespace GadzhiWord.Word.Implementations.ApplicationOfficePartial
{
    /// <summary>
    /// Класс для работы с приложением Word
    /// </summary>
    public partial class ApplicationOffice : IApplicationOffice
    {
        /// <summary>
        /// Ресурсы, используемые модулем Word
        /// </summary>
        public ResourcesWord ResourcesWord { get; }

        public ApplicationOffice(ResourcesWord resourcesWord)
        {
            ResourcesWord = resourcesWord;
        }

        /// <summary>
        /// Экземпляр приложения
        /// </summary>
        private InteropWord _applicationWord;

        /// <summary>
        /// Экземпляр приложения
        /// </summary>
        private InteropWord ApplicationWord
        {
            get
            {
                _applicationWord ??= WordInstance.Instance();

                try
                {
                    string version = _applicationWord.Version;
                }
                catch (COMException)
                {
                    _applicationWord = WordInstance.Instance();
                }

                return _applicationWord;
            }
        }

        /// <summary>
        /// Экземпляр приложения
        /// </summary>
        private InteropExcel _applicationExcel;

        /// <summary>
        /// Экземпляр приложения
        /// </summary>
        private InteropExcel ApplicationExcel
        {
            get
            {
                _applicationExcel ??= ExcelInstance.Instance();

                try
                {
                    string version = _applicationExcel.Version;
                }
                catch (COMException)
                {
                    _applicationExcel = ExcelInstance.Instance();
                }

                return _applicationExcel;
            }
        }

        /// <summary>
        /// Загрузилась ли оболочка Word и Excel
        /// </summary>
        public bool IsApplicationValid => ApplicationWord != null && ApplicationExcel != null;

        /// <summary>
        /// Закрыть приложение
        /// </summary>
        public void CloseApplication()
        {
            _applicationWord?.Quit();
            _applicationExcel?.Quit();
            _applicationWord = null;
            _applicationExcel = null;
        }    
    }
}
