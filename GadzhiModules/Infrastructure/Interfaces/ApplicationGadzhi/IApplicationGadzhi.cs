using System;


namespace GadzhiModules.Infrastructure.Interfaces.ApplicationGadzhi
{
    /// <summary>
    /// Слой приложения, инфраструктура
    /// </summary>
    public interface IApplicationGadzhi : IApplicationGadzhiFileData, IApplicationGadzhiServices, IDisposable
    {
        /// <summary>
        /// Закрыть приложение
        /// </summary>
        void CloseApplication();
    }
}
