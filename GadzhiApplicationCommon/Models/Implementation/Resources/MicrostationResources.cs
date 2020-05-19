using System;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;

namespace GadzhiApplicationCommon.Models.Implementation.Resources
{
    /// <summary>
    /// Ресурсы, используемые модулем Microstation
    /// </summary>
    public class MicrostationResources: ResourcesApplication
    {
        public MicrostationResources(SignaturesLibrarySearching signaturesLibrarySearching,
                                     string signatureMicrostationFileName, string stampMicrostationFileName)
        : base(signaturesLibrarySearching)
        {
            if (String.IsNullOrEmpty(signatureMicrostationFileName)) throw new ArgumentNullException(nameof(signatureMicrostationFileName));
            if (String.IsNullOrEmpty(stampMicrostationFileName)) throw new ArgumentNullException(nameof(stampMicrostationFileName));

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
