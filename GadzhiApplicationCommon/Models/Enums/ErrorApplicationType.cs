namespace GadzhiApplicationCommon.Models.Enums
{
    /// <summary>
    /// Тип ошибки при конвертации в приложении
    /// </summary>
    public enum ErrorApplicationType
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
