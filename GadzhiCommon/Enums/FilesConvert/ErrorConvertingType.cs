namespace GadzhiCommon.Enums.FilesConvert
{
    /// <summary>
    /// Типы ошибок при конвертации файлов
    /// </summary>
    public enum ErrorConvertingType
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
        ArgumentOutOfRange,
        FormatException,
        InvalidEnumArgumentException,
        AttemptingCount,
        InternalError,
        UnknownError,
        ApplicationNotLoad, // Microstation and Word Errors
        LibraryNotFound,
        FieldNotFound,
        FileNotOpen,
        FileNotSaved,
        StampNotFound,   
        TableNotFound,
        RangeNotValid,
        PrinterNotInstall,
        PaperSizeNotFound,
        PdfPrintingError,
        ExportError,
        SignatureNotFound,     
    }
}
