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
        TableNotFound,
        FileNotFound,
        FieldNotFound,
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
