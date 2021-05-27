using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiPrintersTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var printers = PrinterSettings.InstalledPrinters.Cast<string>();
            foreach (var printer in printers)
            {
                Console.WriteLine(printer);
            }

            Console.ReadKey();
        }
    }
}
