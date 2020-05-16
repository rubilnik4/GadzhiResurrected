using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrostationSignatures.DependencyInjection;
using MicrostationSignatures.Infrastructure.Interfaces;
using MicrostationSignatures.Models.Implementations;
using MicrostationSignatures.Models.Interfaces;
using Unity;

namespace MicrostationSignatures
{
    internal class Program
    {
        /// <summary>
        /// Контейнер инверсии зависимости
        /// </summary>
        private static readonly IUnityContainer Container = new UnityContainer();

        private static async Task Main()
        {
            BootStrapUnity.ConfigureContainer(Container);

            var signaturesToJpeg = Container.Resolve<ISignaturesToJpeg>();
            var projectSignatureSettings = Container.Resolve<IProjectSignatureSettings>();

            await signaturesToJpeg.SendJpegSignaturesToDataBase(projectSignatureSettings.SignatureTemplateFilePath);
            await signaturesToJpeg.SendMicrostationSignaturesToDatabase(ProjectSignatureSettings.SignatureMicrostationFileName);

            Console.ReadLine();
        }
    }
}
