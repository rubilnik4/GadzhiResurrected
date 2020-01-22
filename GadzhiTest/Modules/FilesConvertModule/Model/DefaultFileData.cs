using GadzhiModules.Modules.FilesConvertModule.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiTest.Modules.FilesConvertModule.Model
{

    static public class DefaultFileData
    {
        static public IReadOnlyList<FileData> FileDataToTestTwoPositions = new List<FileData>()
            {
                 new FileData ("C:\\folder\\firstName.doc"),
                 new FileData ("C:\\folder\\secondName.dgn"),
            };

        static public IReadOnlyList<FileData> FileDataToTestThreePositions = new List<FileData>()
            {
                 FileDataToTestTwoPositions[0],
                 FileDataToTestTwoPositions[1],
                 new FileData ("C:\\folder\\thirdName.dgn"),
            };

        static public IReadOnlyList<string> FileDataToTestOnlyPath =>
            FileDataToTestTwoPositions.Select(file => file.FilePath).ToList();


    }
}
