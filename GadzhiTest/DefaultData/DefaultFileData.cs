using System.Collections.Generic;
using System.Linq;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Implementations.FileConverting;

namespace GadzhiTest.DefaultData.Model
{

    static public class DefaultFileData
    {
        static public IReadOnlyList<FileData> FileDataToTestTwoPositions => new List<FileData>()
            {
                 new FileData ("C:\\folder\\firstName.doc"),
                 new FileData ("C:\\folder\\secondName.Dgn"),
            };

        static public IReadOnlyList<FileData> FileDataToTestFourPositions => new List<FileData>()
            {
                 FileDataToTestTwoPositions[0],
                 FileDataToTestTwoPositions[1],
                 new FileData ("C:\\folder\\thirdName.doc"),
                 new FileData ("C:\\folder\\FourthName.Dgn"),
            };

        static public IReadOnlyList<FileData> FileDataToTestThreePositions => new List<FileData>()
            {
                 FileDataToTestTwoPositions[0],
                 FileDataToTestTwoPositions[1],
                 new FileData ("C:\\folder\\thirdName.Dgn"),
            };

        static public IReadOnlyList<string> FileDataToTestOnlyPath =>
            FileDataToTestTwoPositions.Select(file => file.FilePath).ToList();


    }
}
