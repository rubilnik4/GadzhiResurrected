namespace GadzhiMicrostation.Models.Enums
{
    /// <summary>
    /// Тип ошибки при конвертации Microstation
    /// </summary>
    public enum ErrorMicrostationType
    {
        ApplicationLoad,
        DesingFileOpen,
        StampNotFound,
        FileNotFound,
        RangeNotValid,
        PrinterNotInstall,
        PaperSizeNotFound,
        PdfPrintingError,
        IncorrectExtension,
        UnknownError,
        NullReference,
        ArgumentNullReference
    }
}
