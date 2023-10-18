using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageLib.Exceptions
{
    public class ImportFormatException : Exception
    {
        public string Caption { get; } = "JSON Ungültig Formatiert!";
        public string Text { get; } = "Jede nachricht muss ein Schlüsselwort und eine Antwort enthalten.";

        public ImportFormatException(): base() { }

        public ImportFormatException(string text): base()
        {
            Text = text;
        }  
    }
}
