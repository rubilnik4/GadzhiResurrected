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
        private InteropWord ApplicationWord => _applicationWord ??= WordInstance.Instance();

        /// <summary>
        /// Экземпляр приложения
        /// </summary>
        private InteropExcel _applicationExcel;

        /// <summary>
        /// Экземпляр приложения
        /// </summary>
        private InteropExcel ApplicationExcel => _applicationExcel ??= ExcelInstance.Instance();

        /// <summary>
        /// Загрузилась ли оболочка Word и Excel
        /// </summary>
        public bool IsApplicationValid => ApplicationWord != null && ApplicationExcel != null;

        /// <summary>
        /// Закрыть приложение
        /// </summary>
        public void CloseApplication()
        {
            ApplicationWord.Quit();
            ApplicationExcel.Quit();
        }    
    }
}
