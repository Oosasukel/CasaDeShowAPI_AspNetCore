namespace CasaDeShowAPI.Model.ViewModels
{
    public static class CasaExtensions{

        public static Casa ToCasa(this CasaForm casaForm){
            Casa casa = new Casa();
            casa.Nome = casaForm.Nome;
            casa.Endereco = casaForm.Endereco;

            return casa;
        }

        public static CasaResponse ToCasaResponse(this Casa casa, string eventosUri){
            CasaResponse casaCreated = new CasaResponse(casa, eventosUri);
            
            return casaCreated;
        }
    }
}