using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChannelAdam.ServiceModel;
using GadzhiCommon.Extensions.Functional;
using GadzhiCommon.Extensions.Functional.Result;
using GadzhiCommon.Infrastructure.Implementations.Logger;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiCommon.Models.Interfaces.LibraryData;
using GadzhiConverting.Infrastructure.Implementations.Converters;
using GadzhiConvertingLibrary.Infrastructure.Implementations.Services;
using GadzhiDTOBase.Infrastructure.Implementations.Converters;
using GadzhiDTOServer.Contracts.FilesConvert;
using Nito.AsyncEx.Synchronous;

namespace GadzhiConverting.Models.Implementations
{
    /// <summary>
    /// Ресурсы, используемые модулями конвертации
    /// </summary>
    public class ConvertingResources
    {
        public ConvertingResources(string signatureMicrostationFileName, string stampMicrostationFileName,
                                 SignatureServerServiceFactory signatureServerServiceFactory,
                                 IFileSystemOperations fileSystemOperations)
        {
            if (String.IsNullOrWhiteSpace(signatureMicrostationFileName)) throw new ArgumentNullException(nameof(signatureMicrostationFileName));
            if (String.IsNullOrWhiteSpace(stampMicrostationFileName)) throw new ArgumentNullException(nameof(stampMicrostationFileName));

            _signatureMicrostationFileName = signatureMicrostationFileName;
            _stampMicrostationFileName = stampMicrostationFileName;
            _signatureServerServiceFactory = signatureServerServiceFactory ?? throw new ArgumentNullException(nameof(signatureServerServiceFactory));
            _fileSystemOperations = fileSystemOperations ?? throw new ArgumentNullException(nameof(fileSystemOperations));
        }

        /// <summary>
        /// Путь для сохранения подписей Microstation
        /// </summary>
        private readonly string _signatureMicrostationFileName;

        /// <summary>
        /// Путь для сохранения штампов Microstation
        /// </summary>
        private readonly string _stampMicrostationFileName;

        /// <summary>
        /// Фабрика для создания подключения к WCF сервису подписей для сервера
        /// </summary>    
        private readonly SignatureServerServiceFactory _signatureServerServiceFactory;

        /// <summary>
        /// Проверка состояния папок и файлов
        /// </summary>   
        private readonly IFileSystemOperations _fileSystemOperations;

        /// <summary>
        /// Имена для подписей
        /// </summary>
        private Task<IResultCollection<ISignatureLibrary>> SignatureNamesTask =>
            GetSignatureNames();

        /// <summary>
        /// Имена для подписей
        /// </summary>
        private IResultCollection<ISignatureLibrary> _signatureNames;

        /// <summary>
        /// Имена для подписей
        /// </summary>
        public IResultCollection<ISignatureLibrary> SignatureNames =>
            _signatureNames ??= SignatureNamesTask.WaitAndUnwrapException();

        /// <summary>
        /// Подписи Microstation
        /// </summary>
        private Task<IResultValue<string>> SignaturesMicrostationTask => GetSignaturesMicrostation();

        /// <summary>
        /// Подписи Microstation
        /// </summary>
        private IResultValue<string> _signaturesMicrostation;

        /// <summary>
        /// Подписи Microstation
        /// </summary>
        public IResultValue<string> SignaturesMicrostation => 
            _signaturesMicrostation ??= SignaturesMicrostationTask.WaitAndUnwrapException();

        /// <summary>
        /// Штампы для Microstation
        /// </summary>
        private Task<IResultValue<string>> StampMicrostationTask => GetStampsMicrostation();

        /// <summary>
        /// Штампы для Microstation
        /// </summary>
        private IResultValue<string> _stampMicrostation;

        /// <summary>
        /// Штампы для Microstation
        /// </summary>
        public IResultValue<string> StampMicrostation => _stampMicrostation ??= StampMicrostationTask.WaitAndUnwrapException();

        /// <summary>
        /// Получить подписи для Microstation
        /// </summary>
        [Logger]
        private async Task<IResultValue<string>> GetSignaturesMicrostation() =>
            await _signatureServerServiceFactory.UsingServiceRetry(service => service.Operations.GetSignaturesMicrostation()).
            ResultValueOkAsync(ConverterMicrostationDataFromDto.MicrostationDataFileFromDto).
            ResultValueOkBindAsync(signatures => _fileSystemOperations.UnzipFileAndSaveWithResult(_signatureMicrostationFileName, 
                                                                                                  signatures.MicrostationDataBase));

        /// <summary>
        /// Получить штампы для Microstation
        /// </summary>
        [Logger]
        private async Task<IResultValue<string>> GetStampsMicrostation() =>
            await _signatureServerServiceFactory.UsingServiceRetry(service => service.Operations.GetStampsMicrostation()).
            ResultValueOkAsync(ConverterMicrostationDataFromDto.MicrostationDataFileFromDto).
            ResultValueOkBindAsync(stamps => _fileSystemOperations.UnzipFileAndSaveWithResult(_stampMicrostationFileName, 
                                                                                              stamps.MicrostationDataBase));
        /// <summary>
        /// Загрузить имена для подписей
        /// </summary>
        [Logger]
        private async Task<IResultCollection<ISignatureLibrary>> GetSignatureNames() =>
            await _signatureServerServiceFactory.UsingServiceRetry(service => service.Operations.GetSignaturesNames()).
            ResultValueOkAsync(ConverterDataFileFromDto.SignaturesLibraryFromDto).
            MapAsync(result => result.ToResultCollection());
    }
}
