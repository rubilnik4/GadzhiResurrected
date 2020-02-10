﻿using FluentNHibernate.Mapping;
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
    public class FileDataMap : ClassMap<FileDataEntity>
    {
        public FileDataMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.FilePath);
            Map(x => x.IsCompleted);
            Map(x => x.ColorPrint).CustomType<ColorPrint>();
            Map(x => x.StatusProcessing).CustomType<StatusProcessing>();
            HasMany(x => x.FileConvertErrorType).Element("ErrorType");          
            Map(x => x.FileDataSource).CustomType<BinaryBlobType>(); ;            
            References(x => x.FilesDataEntity);
        }
    }
}
