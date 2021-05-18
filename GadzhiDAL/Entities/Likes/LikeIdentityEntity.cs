using GadzhiCommon.Models.Interfaces.Likes;

namespace GadzhiDAL.Entities.Likes
{
    /// <summary>
    /// Лайк. Сущность
    /// </summary>
    public class LikeIdentityEntity: ILikeIdentity
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public virtual string PersonId { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public virtual string PersonFullname { get; set; }

        /// <summary>
        /// Количество лайков
        /// </summary>
        public virtual int LikeCount { get; set; }
    }
}