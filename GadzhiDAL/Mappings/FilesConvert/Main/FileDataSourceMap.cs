﻿using FluentNHibernate.Mapping;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiDAL.Entities.FilesConvert;
using GadzhiDAL.Entities.FilesConvert.Main;
using NHibernate.Type;

namespace GadzhiDAL.Mappings.FilesConvert.Main
{
    /// <summary>
    /// Структура в БД для конвертируемого файла
    /// </summary>
    public class FileDataSourceMap : ClassMap<FileDataSourceEntity>
    {
        public FileDataSourceMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.FileName).Not.Nullable().Default("");
            Map(x => x.FileExtensionType).CustomType<FileExtensionType>().Not.Nullable().Default("0");
            Map(x => x.PaperSize).Not.Nullable().Default("");
            Map(x => x.PrinterName).Not.Nullable().Default("");
            Map(x => x.FileDataSource).CustomType<BinaryBlobType>().LazyLoad();
            References(x => x.FileDataEntity);
        }
    }
}
