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

        public ResultFileDataSource(IErrorConverting error)
            : base(error.AsEnumerable()) { }

        public ResultFileDataSource(IEnumerable<IErrorConverting> errors)
           : base(errors) { }

        public ResultFileDataSource(IEnumerable<IFileDataSourceServer> filedatasSource)
          : this(filedatasSource, Enumerable.Empty<IErrorConverting>()) { }

        public ResultFileDataSource(IEnumerable<IFileDataSourceServer> filedatasSource, IEnumerable<IErrorConverting> errors)
            : base(filedatasSource, errors)
        {
            if (!Validate(filedatasSource)) throw new NullReferenceException(nameof(filedatasSource));
        }

        /// <summary>
        /// Добавить ответ
        /// </summary>      
        public IResultFileDataSource ConcatResult(IResultFileDataSource resultFileDataSource) =>
            resultFileDataSource != null ?
            new ResultFileDataSource(resultFileDataSource.Value, resultFileDataSource.Errors) :
            this;
    }
}
