using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChannelAdam.ServiceModel;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Extensions.Functional;
using GadzhiCommon.Extensions.Functional.Result;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiConverting.Infrastructure.Implementations.Converters;
using GadzhiDTOServer.Contracts.FilesConvert;

namespace GadzhiConverting.Models.Implementations
{
    /// <summary>
    /// Ресурсы, используемые модулями конвертации
    /// </summary>
    public class ConvertingResources
    {
        /// <summary>
        /// Путь для сохранения подписей Microstation
        /// </summary>
        private readonly string _signatureMicrostationFileName;

        /// <summary>
        /// Путь для сохранения штампов Microstation
        /// </summary>
        private readonly string _stampMicrostationFileName;

        /// <summary>
        /// Сервис для добавления и получения данных о конвертируемых пакетах в серверной части, обработки подписей
        /// </summary>     
        private readonly IServiceConsumer<IFileConvertingServerService> _fileConvertingServerService;

        /// <summary>
        /// Проверка состояния папок и файлов
        /// </summary>   
        private readonly IFileSystemOperations _fileSystemOperations;

        public ConvertingResources(string signatureMicrostationFileName, string stampMicrostationFileName,
                                   IServiceConsumer<IFileConvertingServerService> fileConvertingServerService,
                                   IFileSystemOperations fileSystemOperations)
        {
            if (String.IsNullOrWhiteSpace(signatureMicrostationFileName)) throw new ArgumentNullException(nameof(signatureMicrostationFileName));
            if (String.IsNullOrWhiteSpace(stampMicrostationFileName)) throw new ArgumentNullException(nameof(stampMicrostationFileName));

            _signatureMicrostationFileName = signatureMicrostationFileName;
            _stampMicrostationFileName = stampMicrostationFileName;
            _fileConvertingServerService = fileConvertingServerService ?? throw new ArgumentNullException(nameof(fileConvertingServerService));
            _fileSystemOperations = fileSystemOperations ?? throw new ArgumentNullException(nameof(fileSystemOperations));
        }

        /// <summary>
        /// Имена для подписей
        /// </summary>
        private Task<IReadOnlyList<ISignatureLibrary>> SignatureNamesTask => GetSignatureNames();

        /// <summary>
        /// Имена для подписей
        /// </summary>
        private IReadOnlyList<ISignatureLibrary> _signatureNames;

        /// <summary>
        /// Имена для подписей
        /// </summary>
        public IReadOnlyList<ISignatureLibrary> SignatureNames => _signatureNames ??= SignatureNamesTask.Result;

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
        public IResultValue<string> SignaturesMicrostation => _signaturesMicrostation ??= SignaturesMicrostationTask.Result;

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
        public IResultValue<string> StampMicrostation => _stampMicrostation ??= StampMicrostationTask.Result;

        /// <summary>
        /// Загрузить отложенные данные
        /// </summary>
        public async Task LoadData() => await Task.WhenAll(SignatureNamesTask, SignaturesMicrostationTask, StampMicrostationTask);

        /// <summary>
        /// Получить подписи для Microstation
        /// </summary>
        private async Task<IResultValue<string>> GetSignaturesMicrostation() =>
            await _fileConvertingServerService.Operations.GetSignaturesMicrostation().
                  MapAsync(ConverterDataFileFromDto.MicrostationDataFileFromDto).
                  MapAsyncBind(signatures => _fileSystemOperations.UnzipFileAndSaveWithResult(_signatureMicrostationFileName, signatures.MicrostationDataBase));

        /// <summary>
        /// Получить штампы для Microstation
        /// </summary>
        private async Task<IResultValue<string>> GetStampsMicrostation() =>
            await _fileConvertingServerService.Operations.GetStampsMicrostation().
                  MapAsync(ConverterDataFileFromDto.MicrostationDataFileFromDto).
                  MapAsyncBind(stamps => _fileSystemOperations.UnzipFileAndSaveWithResult(_stampMicrostationFileName, stamps.MicrostationDataBase));

        /// <summary>
        /// Загрузить имена для подписей
        /// </summary>
        private async Task<IReadOnlyList<ISignatureLibrary>> GetSignatureNames() =>
            await _fileConvertingServerService.Operations.GetSignaturesNames().
                  MapAsync(ConverterDataFileFromDto.SignaturesLibraryFromDto);
    }
}
