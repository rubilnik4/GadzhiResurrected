using System;
using System.Diagnostics.CodeAnalysis;
using GadzhiCommon.Models.Interfaces.Likes;

namespace GadzhiDAL.Entities.Likes
{
    /// <summary>
    /// Лайк. Сущность
    /// </summary>
    [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
    public class LikeIdentityEntity: ILikeIdentity
    {
        public LikeIdentityEntity()
        { }

        public LikeIdentityEntity(string personId, string personFullname,
                                  DateTime lastDateLike, int likeCount)
        {
            PersonId = personId;
            PersonFullname = personFullname;
            LastDateLike = lastDateLike;
            LikeCount = likeCount;
        }
        /// <summary>
        /// Идентификатор
        /// </summary>
        public virtual string PersonId { get; protected set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public virtual string PersonFullname { get; set; }

        /// <summary>
        /// Последнее обновление
        /// </summary>
        public virtual DateTime LastDateLike { get; set; }

        /// <summary>
        /// Количество лайков
        /// </summary>
        public virtual int LikeCount { get; set; }
    }
}