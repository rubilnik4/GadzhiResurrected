namespace GadzhiCommon.Enums.FilesConvert
{
    /// <summary>
    /// Статус обработки пакета
    /// </summary>
    public enum StatusProcessingProject
    {
        NeedToLoadFiles,
        NeedToStartConverting,
        Sending,
        InQueue,
        Converting,
        ConvertingComplete,
        Receiving,
        Writing,
        End,
        Error,
        Abort,
        Archived,
    }
}
