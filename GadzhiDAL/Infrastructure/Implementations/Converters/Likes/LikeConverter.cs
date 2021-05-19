using System.Collections.Generic;
using System.Linq;
using GadzhiDAL.Entities.Likes;
using GadzhiDTOBase.TransferModels.Likes;

namespace GadzhiDAL.Infrastructure.Implementations.Converters.Likes
{
    /// <summary>
    /// Преобразование лайков в модели БД
    /// </summary>
    public static class LikeConverter
    {
        /// <summary>
        /// Преобразовать в трансферные модели
        /// </summary>
        public static IList<LikeIdentityResponse> ToResponses(IEnumerable<LikeIdentityEntity> likeIdentityEntities) =>
            likeIdentityEntities.Select(ToResponse).ToList();

        /// <summary>
        /// Преобразовать в трансферную модель
        /// </summary>
        public static LikeIdentityResponse ToResponse(LikeIdentityEntity likeIdentityEntity) =>
            new LikeIdentityResponse(likeIdentityEntity.PersonId, likeIdentityEntity.PersonFullname,
                                     likeIdentityEntity.LastDateLike, likeIdentityEntity.LikeCount);
    }
}