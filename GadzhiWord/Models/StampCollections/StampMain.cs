using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiWord.Models.StampCollections
{
    /// <summary>
    /// Основные поля штампа
    /// </summary>
    public class StampMain
    {
        private readonly List<StampPersonSignature> _stampPersonSignatures;
        public StampMain()
        {
            _stampPersonSignatures = new List<StampPersonSignature>();
        }

       
    }
}
