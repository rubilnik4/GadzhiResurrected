using ConvertingModels.Models.Interfaces.FilesConvert;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiConverting.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiConverting.Models.Implementations
{
    /// <summary>
    /// Вариант ответа для конвертации файлов
    /// </summary>
    public class ResultFileDataSource : ResultConvertingValue<IEnumerable<IFileDataSourceServer>>, IResultFileDataSource
    {
        public ResultFileDataSource()
            : base() { }

        public ResultFileDataSource(IErrorConverting errorConverting)
            : base(errorConverting.AsEnumerable()) { }

        public ResultFileDataSource(IEnumerable<IErrorConverting> errorsConverting)
           : base(errorsConverting) { }

        public ResultFileDataSource(IEnumerable<IFileDataSourceServer> filedatasSource)
          : this(filedatasSource, Enumerable.Empty<IErrorConverting>()) { }

        public ResultFileDataSource(IEnumerable<IFileDataSourceServer> filedatasSource, IEnumerable<IErrorConverting> errorsConverting)
            : base(filedatasSource, errorsConverting)
        {
            if (!Validate(filedatasSource)) throw new NullReferenceException(nameof(filedatasSource));
        }
    }
}
