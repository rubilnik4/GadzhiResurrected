using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using GadzhiDAL.Entities.Signatures;
using NHibernate.Type;

namespace GadzhiDAL.Mappings.Signatures
{
    /// <summary>
    /// Структура в БД для идентификатора личности с подписью Microstation
    /// </summary>
    public class SignatureMicrostationMap : ClassMap<SignatureMicrostationEntity>
    {
        public SignatureMicrostationMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.NameDatabase).Not.Nullable();
            Map(x => x.MicrostationDataBase).CustomType<BinaryBlobType>().LazyLoad();
        }
    }
}
