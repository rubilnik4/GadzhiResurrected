using System;
using System.Globalization;
using FluentNHibernate.Mapping;
using GadzhiDAL.Entities.Likes;
using GadzhiDAL.Entities.ServerStates;

namespace GadzhiDAL.Mappings.Likes
{
    /// <summary>
    /// Структура в БД для лайков
    /// </summary>
    public class LikeMap : ClassMap<LikeIdentityEntity>
    {
        public LikeMap()
        {
            Id(x => x.PersonId).Not.Nullable().Default("");
            Map(x => x.PersonFullname).Not.Nullable().Default("");
            Map(x => x.LastDateLike).Not.Nullable();
            Map(x => x.LikeCount).Not.Nullable().Default("0");
        }
    }
}