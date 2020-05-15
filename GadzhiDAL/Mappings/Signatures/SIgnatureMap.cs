using FluentNHibernate.Mapping;
using GadzhiDAL.Entities.Signatures;
using NHibernate.Type;

namespace GadzhiDAL.Mappings.Signatures
{
    /// <summary>
    /// Структура в БД для идентификатора личности с подписью
    /// </summary>
    public class SignatureMap : ClassMap<SignatureEntity>
    {
        public SignatureMap()
        {
            Id(x => x.Id).Not.Nullable();
            Map(x => x.FullName).Not.Nullable();
            Map(x => x.SignatureJpeg).CustomType<BinaryBlobType>().LazyLoad();
        }
    }
}