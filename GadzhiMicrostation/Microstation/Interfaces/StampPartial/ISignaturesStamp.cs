namespace GadzhiMicrostation.Microstation.Interfaces.StampPartial
{
    /// <summary>
    /// Класс для работы с подписями
    /// </summary>
    public interface ISignaturesStamp
    {

        /// <summary>
        /// Вставить подписи
        /// </summary>
        void InsertSignatures();

        /// <summary>
        /// Удалить предыдущие подписи
        /// </summary>
        void DeleteSignaturesPrevious();

        /// <summary>
        /// Удалить текущие подписи
        /// </summary>
        void DeleteSignaturesInserted();
    }
}
