using System.Collections.Generic;

namespace GadzhiCommon.Models.Interfaces.LibraryData
{
    /// <summary>
    /// Имя с идентификатором и изображением в байтовом виде
    /// </summary>
    public interface ISignatureFileDataBase<out TPerson>: ISignatureLibraryBase<TPerson>
        where TPerson: IPersonInformation
    {
        /// <summary>
        /// Изображение подписи
        /// </summary>
        IList<byte> SignatureSource { get; }
    }
}