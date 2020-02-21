namespace GadzhiCommon.Enums.FilesConvert
{
    /// <summary>
    /// Типы ошибок при конвертации файлов
    /// </summary>
    public enum FileConvertErrorType
    {
        NoError,
        FileNotFound,
        IncorrectFileName,
        IncorrectExtension,
        IncorrectDataSource,
        RejectToSave,
        AbortOperation,
        TimeOut,
        Communication,
        NullReference,
        ArgumentNullReference,
        AttemptingCount,
        UnknownError,
    }
}
