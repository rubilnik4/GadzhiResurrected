using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GadzhiCommon.Extensions.Functional;
using GadzhiCommon.Extensions.Functional.Result;
using GadzhiResurrected.Infrastructure.Implementations.Converters.Likes;
using GadzhiResurrected.Infrastructure.Implementations.Services;
using GadzhiResurrected.Infrastructure.Interfaces;
using GadzhiResurrected.Modules.GadzhiConvertingModule.ViewModels.Base;
using GadzhiResurrected.Modules.GadzhiConvertingModule.ViewModels.Tabs.LikeViewModelItems;
using Nito.Mvvm;
using Prism.Commands;

namespace GadzhiResurrected.Modules.GadzhiConvertingModule.ViewModels.Tabs
{
    /// <summary>
    /// Лайки
    /// </summary>
    public class LikeViewModel : ViewModelBase
    {
        public LikeViewModel(IDialogService dialogService, LikeClientServiceFactory likeClientServiceFactory)
        {
            _likeClientServiceFactory = likeClientServiceFactory;
            DialogService = dialogService;
            UpdateLikesCommand = new DelegateCommand(() => LikeIdentities = NotifyTask.Create(GetLikeIdentities));
        }

        /// <summary>
        /// Инициализация
        /// </summary>
        private void OnInitialize() =>
            LikeIdentities ??= NotifyTask.Create(GetLikeIdentities);

        /// <summary>
        /// Стандартные диалоговые окна
        /// </summary>
        protected override IDialogService DialogService { get; }

        /// <summary>
        /// Фабрика для создания подключения к WCF сервису получения лайков
        /// </summary>
        private readonly LikeClientServiceFactory _likeClientServiceFactory;

        /// <summary>
        /// Название
        /// </summary>
        public override string Title => "Лайки";

        /// <summary>
        /// Выбор вкладки
        /// </summary>
        private bool _isSelected;

        /// <summary>
        /// Выбор вкладки
        /// </summary>
        public override bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnInitialize();
            }
        }

        /// <summary>
        /// Обновить лайки
        /// </summary>
        public DelegateCommand UpdateLikesCommand { get; }

        /// <summary>
        /// Список лайков
        /// </summary>
        private NotifyTask<IReadOnlyCollection<LikeViewModelItem>> _likeIdentities;

        /// <summary>
        /// Список лайков
        /// </summary>
        public NotifyTask<IReadOnlyCollection<LikeViewModelItem>> LikeIdentities
        {
            get => _likeIdentities;
            private set => SetProperty(ref _likeIdentities, value);
        }

        /// <summary>
        /// Получить список лайков
        /// </summary>
        private async Task<IReadOnlyCollection<LikeViewModelItem>> GetLikeIdentities() =>
            await _likeClientServiceFactory.UsingServiceRetry(service => service.Operations.GetLikes()).
            ResultValueOkAsync(LikeConverter.ToClients).
            ResultValueOkAsync(likes => likes.Select(like => new LikeViewModelItem(like))).
            WhereContinueAsync(result => result.OkStatus,
                okFunc: result => result.Value.ToList(),
                badFunc: result => new List<LikeViewModelItem>());
    }
}