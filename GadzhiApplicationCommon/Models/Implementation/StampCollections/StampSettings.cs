using System;
using GadzhiApplicationCommon.Models.Implementation.FilesConvert;

namespace GadzhiApplicationCommon.Models.Implementation.StampCollections
{
    /// <summary>
    /// 
    /// </summary>
    public class StampSettings: ConvertingSettingsApplication
    {
        public StampSettings(StampIdentifier id, string department)
            :base(department)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
        }

        /// <summary>
        /// Идентификатор штампа
        /// </summary>
        public StampIdentifier Id { get; }
    }
}