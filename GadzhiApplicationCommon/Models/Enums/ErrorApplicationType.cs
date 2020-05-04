namespace GadzhiApplicationCommon.Models.Enums
{
    /// <summary>
    /// Тип ошибки при конвертации в приложении
    /// </summary>
    public enum ErrorApplicationType
    {
        ApplicationNotLoad,
        FileNotOpen,
        FileNotSaved,
        StampNotFound,
        FileNotFound,
        RangeNotValid,
        PrinterNotInstall,
        PaperSizeNotFound,
        PdfPrintingError,
        DwgCreatingError,
        SignatureNotFound,
        IncorrectExtension,
        ArgumentNullReference,
        NullReference,
        UnknownError,    
    }
}
