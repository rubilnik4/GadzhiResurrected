using System.Collections.Generic;
using System.Threading.Tasks;
using GadzhiCommon.Models.Implementations.Functional;
using GadzhiDTOBase.TransferModels.Likes;

namespace GadzhiDAL.Services.Interfaces.Likes
{
    /// <summary>
    /// Сервис лайков в БД
    /// </summary>
    public interface ILikeService
    {
        /// <summary>
        /// Получить список пользователей и лайков
        /// </summary>
        Task<IList<LikeIdentityResponse>> GetLikes();

        /// <summary>
        /// Поставить лайк
        /// </summary>
        Task<Unit> SendLike(string personId, string personFullName);
    }
}