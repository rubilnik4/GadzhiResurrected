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
        Receiving,
        Writing,
        End,
        Error,
    }
}
