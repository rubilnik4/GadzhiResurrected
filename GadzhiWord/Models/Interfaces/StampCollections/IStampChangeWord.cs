using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;

namespace GadzhiWord.Models.Interfaces.StampCollections
{
    /// <summary>
    /// Строка с ответственным лицом и подписью Word
    /// </summary>
    public interface IStampChangeWord : IStampChange<IStampFieldWord>, IStampSignatureWord
    {
    }
}
