using GadzhiWord.Models.Implementations;
using GadzhiWord.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace GadzhiWord.DependencyInjection
{
    /// <summary>
    /// Класс для регистрации зависимостей Word
    /// </summary>
    public static class BootStrapUnityWord
    {
        public static void ConfigureContainer(IUnityContainer container)
        {
            container.RegisterSingleton<IWordProject, WordProject>();           
        }
    }
}
