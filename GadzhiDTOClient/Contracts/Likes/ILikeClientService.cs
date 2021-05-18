using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using GadzhiCommon.Models.Implementations.Functional;
using GadzhiDTOBase.TransferModels.Likes;

namespace GadzhiDTOClient.Contracts.Likes
{
    /// <summary>
    /// Сервис лайков
    /// </summary>
    [ServiceContract]
    public interface ILikeClientService
    {
        /// <summary>
        /// Получить список пользователей и лайков
        /// </summary>
        [OperationContract]
        Task<IList<LikeIdentityResponse>> GetLikes();

        /// <summary>
        /// Поставить лайк
        /// </summary>
        [OperationContract]
        Task<Unit> SendLike(string personId, string personFullName);
    }
}