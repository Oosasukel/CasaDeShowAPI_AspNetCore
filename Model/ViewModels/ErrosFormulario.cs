using System.Collections.Generic;

namespace CasaDeShowAPI.Model.ViewModels
{
    public class ErrosFormulario
    {
        public List<ErroFormulario> Erros { get; set; }
    }

    public class ErroFormulario{
        public string Campo { get; set; }
        public string Message { get; set; }
    }
}