using GadzhiCommon.Models.Interfaces.Likes;

namespace GadzhiModules.Modules.GadzhiConvertingModule.Models.Implementations.Likes
{
    /// <summary>
    /// Пользователь с лайком
    /// </summary>
    public class LikeIdentityClient: ILikeIdentity
    {
        public LikeIdentityClient(string personId, string personFullname, int likeCount)
        {
            PersonId = personId;
            PersonFullname = personFullname;
            LikeCount = likeCount;
        }

        /// <summary>
        /// Идентификатор
        /// </summary>
        public string PersonId { get; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string PersonFullname { get; }

        /// <summary>
        /// Количество лайков
        /// </summary>
        public int LikeCount { get; }
    }
}