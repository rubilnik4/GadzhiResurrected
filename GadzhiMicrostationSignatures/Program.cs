using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GadzhiMicrostation.Factory;
using GadzhiMicrostationSignatures.DependencyInjection;
using GadzhiMicrostationSignatures.Infrastructure.Interfaces;
using GadzhiMicrostationSignatures.Models.Enums;
using GadzhiMicrostationSignatures.Models.Implementations;
using GadzhiMicrostationSignatures.Models.Interfaces;
using Unity;

namespace GadzhiMicrostationSignatures
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
        }
    }
}
