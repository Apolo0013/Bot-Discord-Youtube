namespace Bot.Type
{
    class UserInstrucaoAbaixa
    {
        public string Formato { set; get; } = "";
        public string Url { set; get; } = "";
    }

    ////////////////////////////////////////////////////////////////
    /// Resquest
    ////////////////////////////////////////////////////////////////
    class VerificarUrlType
    {
        public bool Sucesso { set; get; }
        public bool Valido { set; get; }
    }
}