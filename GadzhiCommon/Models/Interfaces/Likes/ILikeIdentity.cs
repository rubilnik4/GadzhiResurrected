﻿using System;

namespace GadzhiCommon.Models.Interfaces.Likes
{
    /// <summary>
    /// Пользователь с лайком
    /// </summary>
    public interface ILikeIdentity
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        string PersonId { get; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        string PersonFullname { get; }

        /// <summary>
        /// Последнее обновление
        /// </summary>
        DateTime LastDateLike { get; }

        /// <summary>
        /// Количество лайков
        /// </summary>
        int LikeCount { get; }
    }
}