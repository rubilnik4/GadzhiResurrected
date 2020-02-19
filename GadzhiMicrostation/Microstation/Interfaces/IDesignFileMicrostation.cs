using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Microstation.Interfaces
{
    /// <summary>
    /// Текущий файл Microstation
    /// </summary>
    public interface IDesignFileMicrostation
    {
        /// <summary>
        /// Путь к файлу
        /// </summary>
        string FullName { get; }

        /// <summary>
        /// Загрузился ли файл
        /// </summary>
        bool IsDesingFileValid { get; }

        /// <summary>
        /// Модели и листы в текущем файле
        /// </summary>
        IList<IModelMicrostation> ModelsMicrostation { get; }

        /// <summary>
        /// Найти все штампы во всех моделях и листах
        /// </summary>       
        IList<IStamp> Stamps { get; }

        /// <summary>
        /// Сохранить файл
        /// </summary>
        void Save();

        /// <summary>
        /// Сохранить файл как
        /// </summary>
        void SaveAs(string filePath);

        /// <summary>
        /// Закрыть файл файл
        /// </summary>
        void Close();

        /// <summary>
        /// Закрыть файл файл
        /// </summary>
        void CloseWithSaving();
    }
}