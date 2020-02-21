using GadzhiModules.Modules.FilesConvertModule.Models.Implementations;
using System.Collections.Generic;
using System.Linq;

namespace GadzhiTest.DefaultData.Model
{

    static public class DefaultFileData
    {
        static public IReadOnlyList<FileData> FileDataToTestTwoPositions => new List<FileData>()
            {
                 new FileData ("C:\\folder\\firstName.doc"),
                 new FileData ("C:\\folder\\secondName.dgn"),
            };

        static public IReadOnlyList<FileData> FileDataToTestFourPositions => new List<FileData>()
            {
                 FileDataToTestTwoPositions[0],
                 FileDataToTestTwoPositions[1],
                 new FileData ("C:\\folder\\thirdName.doc"),
                 new FileData ("C:\\folder\\FourthName.dgn"),
            };

        static public IReadOnlyList<FileData> FileDataToTestThreePositions => new List<FileData>()
            {
                 FileDataToTestTwoPositions[0],
                 FileDataToTestTwoPositions[1],
                 new FileData ("C:\\folder\\thirdName.dgn"),
            };

        static public IReadOnlyList<string> FileDataToTestOnlyPath =>
            FileDataToTestTwoPositions.Select(file => file.FilePath).ToList();


    }
}
