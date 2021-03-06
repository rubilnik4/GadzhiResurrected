﻿using System;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiDTOClient.TransferModels.FilesConvert;

namespace GadzhiResurrected.Infrastructure.Implementations.Validates
{
    /// <summary>
    /// Проверка данных пакета перед отправкой
    /// </summary>
    public static class PackageDataValidation
    {
        /// <summary>
        /// Проверить файлы перед отправкой на корректность
        /// </summary>
        public static IResultError ValidatePackageData(PackageDataRequestClient packageDataRequestClient) =>
            ValidateSignature(packageDataRequestClient);

        /// <summary>
        /// Проверить подпись
        /// </summary>
        public static IResultError ValidateSignature(PackageDataRequestClient packageDataRequestClient) =>
            !String.IsNullOrWhiteSpace(packageDataRequestClient.ConvertingSettings.PersonId)
            ? new ResultError()
            : new ResultError(new ErrorCommon(ErrorConvertingType.SignatureNotFound, "Необходимо выбрать подпись в параметрах"));
    }
}