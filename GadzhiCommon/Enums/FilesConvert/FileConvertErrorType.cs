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
        FormatException,
        AttemptingCount,
        InternalError,
        UnknownError,
        
        ApplicationNotLoad, //Microstation and Word Errors
        LibraryNotFound,
        FileNotOpen,
        FileNotSaved,
        StampNotFound,       
        RangeNotValid,
        PrinterNotInstall,
        PaperSizeNotFound,
        PdfPrintingError,
        DwgCreatingError,
        SignatureNotFound,     
    }
}
