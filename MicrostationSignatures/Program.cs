using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GadzhiMicrostation.Factory;
using MicrostationSignatures.DependencyInjection;
using MicrostationSignatures.Infrastructure.Interfaces;
using MicrostationSignatures.Models.Enums;
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
        private static readonly IUnityContainer _container = new UnityContainer();

        private static async Task Main()
        {
            BootStrapUnity.ConfigureContainer(_container, ProjectSignatureSettings.SignaturesSaveFolder);

            MicrostationInstance.KillAllPreviousProcess();

            var projectSignatureSettings = _container.Resolve<IProjectSignatureSettings>();
            var signaturesToJpeg = _container.Resolve<ISignaturesToJpeg>();

            await signaturesToJpeg.SendJpegSignaturesToDataBase(projectSignatureSettings.SignatureTemplateFilePath);
            await signaturesToJpeg.SendMicrostationDataToDatabase(projectSignatureSettings.SignatureMicrostationFileName, 
                                                                  MicrostationDataType.Signature);
            await signaturesToJpeg.SendMicrostationDataToDatabase(projectSignatureSettings.StampMicrostationFileName,
                                                                  MicrostationDataType.Stamp);

            Console.ReadLine();
        }
    }
}
