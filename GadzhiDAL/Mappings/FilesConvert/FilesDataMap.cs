using FluentNHibernate.Mapping;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiDAL.Entities.FilesConvert;
using NHibernate.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiDAL.Mappings.FilesConvert
{
    public class FilesDataMap : ClassMap<FilesDataEntity>
    {
        public FilesDataMap()
        {
            Id(x => x.Id);
            Map(x => x.CreationDateTime).Not.Nullable();
            Map(x => x.IdentityName).Not.Nullable().Default("");
            Map(x => x.IsCompleted).Not.Nullable();            
            Map(x => x.StatusProcessingProject).CustomType<StatusProcessingProject>().Not.Nullable();
            HasMany(x => x.FilesData)
                    .Inverse()
                    .Cascade.All();
        }        
    }
}
