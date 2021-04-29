using FluentNHibernate.Mapping;
using GadzhiDAL.Entities.ServerStates;

namespace GadzhiDAL.Mappings.ServerStates
{
    /// <summary>
    /// Структура в БД для доступа сервера
    /// </summary>
    public class ServerAccessMap : ClassMap<ServerAccessEntity>
    {
        public ServerAccessMap()
        {
            Id(x => x.Identity).Not.Nullable().Default("");
            Map(x => x.LastAccess).Not.Nullable();
        }
    }
}