using GadzhiCommon.Models.Interfaces.Likes;

namespace GadzhiResurrected.Modules.GadzhiConvertingModule.ViewModels.Tabs.LikeViewModelItems
{
    /// <summary>
    /// Лайк
    /// </summary>
    public class LikeViewModelItem
    {
        public LikeViewModelItem(ILikeIdentity likeIdentity)
        {
            _likeIdentity = likeIdentity;
        }

        /// <summary>
        /// Пользователь с лайком
        /// </summary>
        private readonly ILikeIdentity _likeIdentity;

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string PersonFullname => 
            _likeIdentity.PersonFullname;

        /// <summary>
        /// Последнее обновление
        /// </summary>
        public string LastDateLike =>
            _likeIdentity.LastDateLike.ToString("G");

        /// <summary>
        /// Количество лайков
        /// </summary>
        public int LikeCount =>
            _likeIdentity.LikeCount;
    }
}