using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using GadzhiCommon.Models.Implementations.Functional;
using GadzhiDAL.Services.Interfaces.Likes;
using GadzhiDTOBase.TransferModels.Likes;
using GadzhiDTOClient.Contracts.Likes;
using GadzhiWcfHost.Infrastructure.Implementations.Authentication;

namespace GadzhiWcfHost.Services.Likes
{
    /// <summary>
    /// Сервис лайков
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
                    ConcurrencyMode = ConcurrencyMode.Multiple,
                    IncludeExceptionDetailInFaults = true)]
    public class LikeClientService : ILikeClientService
    {
        public LikeClientService(ILikeService likeService)
        {
            _likeService = likeService;
        }

        /// <summary>
        /// Сервис лайков
        /// </summary>
        private readonly ILikeService _likeService;

        /// <summary>
        /// Получить список пользователей и лайков
        /// </summary>
        public async Task<IList<LikeIdentityResponse>> GetLikes() =>
            await _likeService.GetLikes();

        /// <summary>
        /// Поставить лайк
        /// </summary>
        public async Task<Unit> SendLike(string personId, string personFullName) =>
            await _likeService.SendLike(personId, personFullName);
    }
}
