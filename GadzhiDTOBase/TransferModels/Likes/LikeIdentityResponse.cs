﻿using System;
using System.Runtime.Serialization;
using GadzhiCommon.Models.Interfaces.Likes;

namespace GadzhiDTOBase.TransferModels.Likes
{
    /// <summary>
    /// Пользователь с лайком. Трансферная модель
    /// </summary>
    [DataContract]
    public class LikeIdentityResponse: ILikeIdentity
    {
        public LikeIdentityResponse(string personId, string personFullname, DateTime lastDateLike , int likeCount)
        {
            PersonId = personId;
            PersonFullname = personFullname;
            LastDateLike = lastDateLike;
            LikeCount = likeCount;
        }

        /// <summary>
        /// Идентификатор
        /// </summary>
        [DataMember]
        public string PersonId { get; private set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        [DataMember]
        public string PersonFullname { get; private set; }

        /// <summary>
        /// Последнее обновление
        /// </summary>
        [DataMember]
        public DateTime LastDateLike { get; private set; }

        /// <summary>
        /// Количество лайков
        /// </summary>
        [DataMember]
        public int LikeCount { get; private set; }
    }
}