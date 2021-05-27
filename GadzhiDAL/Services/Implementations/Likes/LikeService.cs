using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GadzhiCommon.Extensions.Functional;
using GadzhiCommon.Models.Implementations.Functional;
using GadzhiDAL.Entities.Likes;
using GadzhiDAL.Factories.Interfaces;
using GadzhiDAL.Infrastructure.Implementations.Converters.Likes;
using GadzhiDAL.Infrastructure.Implementations.DateTimes;
using GadzhiDAL.Services.Interfaces.Likes;
using GadzhiDTOBase.TransferModels.Likes;
using GadzhiDTOBase.TransferModels.Signatures;
using NHibernate.Linq;
using Unity;

namespace GadzhiDAL.Services.Implementations.Likes
{
    /// <summary>
    /// Сервис лайков в БД
    /// </summary>
    public class LikeService : ILikeService
    {
        public LikeService(IUnityContainer container)
        {
            _container = container;
        }

        /// <summary>
        ///Контейнер зависимостей
        /// </summary>
        private readonly IUnityContainer _container;

        /// <summary>
        /// Получить список пользователей и лайков
        /// </summary>
        public async Task<IList<LikeIdentityResponse>> GetLikes()
        {
            using var unitOfWork = _container.Resolve<IUnitOfWork>();
            return await unitOfWork.Session.Query<LikeIdentityEntity>().ToListAsync().
                   MapAsync(LikeConverter.ToResponses);
        }

        /// <summary>
        /// Поставить лайк
        /// </summary>
        public async Task<Unit> SendLike(string personId, string personFullName)
        {
            using var unitOfWork = _container.Resolve<IUnitOfWork>();
            var likeEntity = await unitOfWork.Session.GetAsync<LikeIdentityEntity>(personId);
            if (likeEntity == null)
            {
                var likeUpdateEntity = new LikeIdentityEntity(personId, personFullName, DateTimeService.GetDateTimeNow(), 1);
                await unitOfWork.Session.SaveOrUpdateAsync(likeUpdateEntity);
            }
            else
            {
                likeEntity.PersonFullname = personFullName;
                likeEntity.LastDateLike = DateTimeService.GetDateTimeNow();
                likeEntity.LikeCount += 1;
            }
            await unitOfWork.CommitAsync();
            return Unit.Value;
        }
    }
}