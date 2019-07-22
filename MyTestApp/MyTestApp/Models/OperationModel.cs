using System.Collections.Generic;

namespace MyTestApp.Models
{
    public class OperationModel
    {
        public string Instruccion { get; set; }
        public object Problem { get; set; }
        public List<string> Options { get; set; }
        public object Resoult { get; set; }
    }
}