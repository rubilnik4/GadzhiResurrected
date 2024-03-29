﻿using System.Configuration;
using GadzhiDAL.DependencyInjection;
using GadzhiDAL.Services.Implementations;
using GadzhiDAL.Services.Interfaces;
using GadzhiDTOClient.Contracts.FilesConvert;
using GadzhiDTOClient.Contracts.Histories;
using GadzhiDTOClient.Contracts.Likes;
using GadzhiDTOClient.Contracts.ServerStates;
using GadzhiDTOClient.Contracts.Signatures;
using GadzhiDTOServer.Contracts.FilesConvert;
using GadzhiDTOServer.Contracts.Signatures;
using GadzhiWcfHost.Services;
using GadzhiWcfHost.Services.FilesConvert;
using GadzhiWcfHost.Services.Histories;
using GadzhiWcfHost.Services.Likes;
using GadzhiWcfHost.Services.ServerStates;
using GadzhiWcfHost.Services.Signatures;
using Unity;
using Unity.Lifetime;
using Unity.Wcf;

namespace GadzhiWcfHost.DependencyInjection
{
    /// <summary>
    /// Класс для подключения инверсии зависимостей
    /// </summary>
    public class WcfServiceFactory : UnityServiceHostFactory
    {
        /// <summary>
        /// Регистрация сервисов
        /// </summary>
        protected override void ConfigureContainer(IUnityContainer container)
        {
            container.
            RegisterType<IFileConvertingClientService, FileConvertingClientService>(new HierarchicalLifetimeManager()).
            RegisterType<ISignatureClientService, SignatureClientService>(new HierarchicalLifetimeManager()).
            RegisterType<IFileConvertingServerService, FileConvertingServerService>(new HierarchicalLifetimeManager()).
            RegisterType<ISignatureServerService, SignatureServerService>(new HierarchicalLifetimeManager()).
            RegisterType<IServerStateClientService, ServerStateClientService>(new HierarchicalLifetimeManager()).
            RegisterType<IHistoryClientService, HistoryClientService>(new HierarchicalLifetimeManager()).
            RegisterType<ILikeClientService, LikeClientService>(new HierarchicalLifetimeManager());

            GadzhiDAL.DependencyInjection.DependencyInjection.ConfigureContainer(container);
        }
    }
}