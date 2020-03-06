using ConvertingModels.Models.Interfaces.FilesConvert;
using ConvertingModels.Models.Interfaces.Printers;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiWord.Infrastructure.Interfaces;
using GadzhiWord.Models.Interfaces;
using GadzhiWord.Word.Interfaces;
using GadzhiWord.Word.Interfaces.ApplicationWordPartial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiWord.Infrastructure.Implementations
{
    /// <summary>
    /// Обработка и конвертирование файла DOC
    /// </summary>
    public class ConvertingFileWord: IConvertingFileWord
    {
        /// <summary>
        /// Класс для работы с приложением Word
        /// </summary>
        private readonly IApplicationWord _applicationWord;

        /// <summary>
        /// Модель хранения данных конвертации Word
        /// </summary>
        private readonly IWordProject _wordProject;

        /// <summary>
        /// Класс для отображения изменений и логгирования
        /// </summary>
        private readonly IMessagingService _messagingService;

        public ConvertingFileWord(IApplicationWord applicationWord,
                                  IWordProject wordProject,
                                  IMessagingService messagingService)
        {
            _applicationWord = applicationWord;
            _wordProject = wordProject;
            _messagingService = messagingService;
        }


        /// <summary>
        /// Запустить конвертацию. Инициировать начальные значения
        /// </summary>      
        public IFileDataServer ConvertingFile(IFileDataServer fileDataServer, IPrintersInformation printersInformation)
        {
            _wordProject.SetInitialFileData(fileDataServer, printersInformation);

            if (_applicationWord.IsApplicationValid)
            {
                _messagingService.ShowAndLogMessage("Загрузка файла Word");
                _applicationWord.OpenDocument(_wordProject.FileDataServerWord.FilePathServer);
                _applicationWord.SaveDocument(_wordProject.CreateFileSavePath(_wordProject.FileDataServerWord.FileNameWithoutExtensionClient,
                                                                              FileExtention.docx));

                _messagingService.ShowAndLogMessage("Создание файлов PDF");
                _applicationWord.CreatePdfFile(_wordProject.CreateFileSavePath(_wordProject.FileDataServerWord.FileNameWithoutExtensionClient,
                                                                               FileExtention.pdf));

                //    _loggerMicrostation.ShowMessage("Создание файла DWG");
                //    _applicationMicrostation.CreateDwgFile(_microstationProject.CreateFileSavePath(_microstationProject.FileDataMicrostation.FileName,
                //                                                                                    FileExtentionMicrostation.dwg));

                _applicationWord.CloseDesignFile();
            }

            return _wordProject.FileDataServerWord;
        }
    }
}
