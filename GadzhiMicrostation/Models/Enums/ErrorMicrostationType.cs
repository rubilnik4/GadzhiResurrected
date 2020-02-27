namespace GadzhiMicrostation.Models.Enums
{
    /// <summary>
    /// Тип ошибки при конвертации Microstation
    /// </summary>
    public enum ErrorMicrostationType
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
