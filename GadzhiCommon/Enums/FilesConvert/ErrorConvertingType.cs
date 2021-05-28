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
        ValueNotInitialized,
        FormatException,
        InvalidEnumArgumentException,
        AttemptingCount,
        InternalError,
        DatabaseError,
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
