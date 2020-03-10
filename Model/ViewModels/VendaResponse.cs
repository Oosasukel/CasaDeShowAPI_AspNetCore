namespace CasaDeShowAPI.Model.ViewModels
{
    public class VendaResponse
    {
        public string Id { get; set; }
        public string Evento { get; set; }
        public decimal PrecoUnitario { get; set; }
        public string Usuario { get; set; }
        public int Quantidade { get; set; }

        public VendaResponse(Venda venda, string uriEvento, string uriUsuario){
            Id = venda.Id;
            Evento = uriEvento;
            PrecoUnitario = venda.PrecoUnitario;
            Quantidade = venda.Quantidade;
            Usuario = uriUsuario;
        }
    }
}