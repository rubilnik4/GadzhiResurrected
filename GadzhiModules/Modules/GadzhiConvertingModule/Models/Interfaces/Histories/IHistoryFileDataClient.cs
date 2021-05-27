using GadzhiCommon.Models.Interfaces.Histories;

namespace GadzhiModules.Modules.GadzhiConvertingModule.Models.Interfaces.Histories
{
    /// <summary>
    /// Данные истории конвертации файла на клиенте
    /// </summary>
    public interface IHistoryFileDataClient: IHistoryFileData<IHistoryFileDataSource>
    { }
}