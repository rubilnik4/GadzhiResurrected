using ConvertingModels.Models.Interfaces.FilesConvert;
using ConvertingModels.Models.Interfaces.Printers;
using GadzhiWord.Infrastructure.Interfaces;
using GadzhiWord.Models.Interfaces;
using GadzhiWord.Word.Interfaces;
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

        public ConvertingFileWord(IApplicationWord applicationWord,
                                  IWordProject wordProject)
        {
            _applicationWord = applicationWord;
            _wordProject = wordProject;
        }


        /// <summary>
        /// Запустить конвертацию. Инициировать начальные значения
        /// </summary>      
        public IFileDataServer ConvertingFile(IFileDataServer fileDataServer, IPrintersInformation printersInformation)
        {
            _wordProject.SetInitialFileData(fileDataServer, printersInformation);

            if (_applicationWord.IsApplicationValid)
            {
                _loggerMicrostation.ShowMessage("Загрузка файла Microstation");
                _applicationMicrostation.OpenDesignFile(_microstationProject.FileDataMicrostation.FilePathServer);
                //    _applicationMicrostation.SaveDesignFile(_microstationProject.CreateFileSavePath(_microstationProject.FileDataMicrostation.FileName,
                //                                                                                    FileExtentionMicrostation.dgn));

                //    _loggerMicrostation.ShowMessage("Создание файлов PDF");
                //    _applicationMicrostation.CreatePdfFile(_microstationProject.CreateFileSavePath(_microstationProject.FileDataMicrostation.FileName,
                //                                                                                FileExtentionMicrostation.pdf));

                //    _loggerMicrostation.ShowMessage("Создание файла DWG");
                //    _applicationMicrostation.CreateDwgFile(_microstationProject.CreateFileSavePath(_microstationProject.FileDataMicrostation.FileName,
                //                                                                                    FileExtentionMicrostation.dwg));

                //    _applicationMicrostation.CloseDesignFile();
            }

            return _wordProject.FileDataServer;
        }
    }
}
