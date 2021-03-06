﻿namespace GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields
{
    /// <summary>
    /// Базовая текстовая ячейка штампа
    /// </summary>  
    public interface IStampTextField: IStampField
    {
        /// <summary>
        /// Текст поля в штампе
        /// </summary>
        string Text { get; }

        /// <summary>
        /// Получить слово максимальной длины
        /// </summary>
        string MaxLengthWord { get; }
    }
}