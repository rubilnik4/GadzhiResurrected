using FluentNHibernate.Mapping;
using GadzhiDAL.Entities.FilesConvert.ServerStates;

namespace GadzhiDAL.Mappings.FilesConvert.ServerStates
{
    /// <summary>
    /// Структура в БД для доступа сервера
    /// </summary>
    public class ServerAccessMap : ClassMap<ServerAccessEntity>
    {
        public ServerAccessMap()
        {
            Id(x => x.ServerIdentity).Not.Nullable();
            Map(x => x.LastAccess).Not.Nullable();
        }
    }
}