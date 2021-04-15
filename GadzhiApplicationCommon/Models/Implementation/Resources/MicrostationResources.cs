using System;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;

namespace GadzhiApplicationCommon.Models.Implementation.Resources
{
    /// <summary>
    /// Ресурсы, используемые модулем Microstation
    /// </summary>
    public class MicrostationResources: ResourcesApplication
    {
        public MicrostationResources(SignaturesSearching signaturesSearching,
                                     string signatureMicrostationFileName, string stampMicrostationFileName)
        : base(signaturesSearching)
        {
            SignatureMicrostationFileName = signatureMicrostationFileName;
            StampMicrostationFileName = stampMicrostationFileName;
        }

        /// <summary>
        /// Подписи для Microstation
        /// </summary>
        public string SignatureMicrostationFileName { get; }

        /// <summary>
        /// Штампы для Microstation
        /// </summary>
        public string StampMicrostationFileName { get; }
    }
}
