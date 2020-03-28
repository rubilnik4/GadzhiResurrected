using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiWord.Models.Interfaces.StampCollections
{
    /// <summary>
    /// Структура подписи Word
    /// </summary>
    public interface IStampSignatureWord: IStampSignature<IStampFieldWord>, ISignatureInformation
    {
    }
}
