using FluentNHibernate.Mapping;
using GadzhiCommon.Enums.LibraryData;
using GadzhiDAL.Entities.PaperSizes;
using GadzhiDAL.Entities.Signatures;
using NHibernate.Type;

namespace GadzhiDAL.Mappings.PaperSizes
{
    public class PaperSizeMap : ClassMap<PaperSizeEntity>
    {
        public PaperSizeMap()
        {
            Id(x => x.PaperSize).Not.Nullable().Default("");
            HasManyToMany(x => x.FileDataSources).Inverse();
        }
    }
}