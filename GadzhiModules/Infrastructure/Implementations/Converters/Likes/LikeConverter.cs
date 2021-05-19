using System.Collections.Generic;
using System.Linq;
using GadzhiCommon.Models.Interfaces.Likes;
using GadzhiDTOBase.TransferModels.Likes;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Implementations.Likes;

namespace GadzhiModules.Infrastructure.Implementations.Converters.Likes
{
    /// <summary>
    /// Преобразование лайков из трансферной модели
    /// </summary>
    public static class LikeConverter
    {
        /// <summary>
        /// Преобразовать в клиентские модели
        /// </summary>
        public static IReadOnlyCollection<ILikeIdentity> ToClients(IEnumerable<LikeIdentityResponse> likeIdentityResponses) =>
            likeIdentityResponses.Select(ToClient).ToList();

        /// <summary>
        /// Преобразовать в клиентскую модель
        /// </summary>
        public static ILikeIdentity ToClient(LikeIdentityResponse likeIdentityResponse) =>
            new LikeIdentityClient(likeIdentityResponse.PersonId, likeIdentityResponse.PersonFullname,
                                   likeIdentityResponse.LastDateLike, likeIdentityResponse.LikeCount);
    }
}