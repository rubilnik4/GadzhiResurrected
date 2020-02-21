namespace GadzhiCommon.Enums.FilesConvert
{
    /// <summary>
    /// Статус обработки файлов
    /// </summary>
    public enum StatusProcessing
    {
        NotSend,
        Sending,
        InQueue,
        Converting,
        Completed,
        Writing,
        End,
        Error,
    }
}
