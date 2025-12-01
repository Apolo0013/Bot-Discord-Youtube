//Class que vai receber e manipular as messagens
using System.Text.RegularExpressions;
using System.Net.Sockets;
using System.Threading.Tasks;
using Discord.WebSocket;
//Outros
using Bot.Run;
//request 
using Bot.Resquest;
//type
using Bot.Type;

namespace BotDiscord.Messagem
{
    class Bot_GetMessagem
    {
        private SocketMessage Msg;
        private Resquest Http;

        public Bot_GetMessagem(SocketMessage msg)
        {
            Msg = msg;
            Http = new Resquest();
        }

        public async Task Read()
        {
            //Verificando se nao é bot
            if (Msg.Author.IsBot) return;
            //Info Msg
            //Id autor
            ulong IDUser = Msg.Author.Id;
            //Canal
            ISocketMessageChannel Canal = Msg.Channel;
            //Conteudo da messagem
            string conteudo = Msg.Content.Trim();
            if (conteudo == "") return; // caso ele esteja vazio
            //Criando a chave para o usuario.
            if (!Global.UsersIntrucao.ContainsKey(IDUser))
            {
                Global.UsersIntrucao.Add(IDUser, new UserInstrucaoAbaixa()
                {
                    Formato = "",
                    Url = ""
                });
            }
            //Mostrando as informacoes.
            if (conteudo.StartsWith("!si")) await ShowInfoStream(IDUser);

            //!formato. Usa para fornece para nois o formato que ele que.
            if (conteudo.StartsWith("!formato"))
            {
                string[] ConteudoSplit = conteudo.Split(" ");  // conteudo splitando
                //caso ele forneca +1 de um argumento.
                if (ConteudoSplit.Length >= 3)
                {
                    await Canal.SendMessageAsync($"{Msg.Author.Mention}\nApenas 1 argumento, porfavor.");
                    return;
                }
                //temos apenas uma arg, vamos tentar pegar para int
                string MSGERROR = $"{Msg.Author.Mention}\nApenas os numeros *1 ou 2*!"; // msgg error
                string stringnumber = ConteudoSplit[1];
                //tentando transformando o argumentos em int
                if (int.TryParse(stringnumber, out int formato)) // se deu certo. em converte
                {
                    if (formato == 1 || formato == 2) // so vai pegar apenas 1 ou 2
                    {
                        Global.UsersIntrucao[IDUser].Formato = formato == 1 ? Global.VIDEO : Global.AUDIO;
                        //avisando aque foi add
                        await Canal.SendMessageAsync($"{Msg.Author.Mention}\nFormato adicionado!");
                    }
                    else await Canal.SendMessageAsync(MSGERROR);
                }
                //ele nos forneceu um string e vez de um int
                else await Canal.SendMessageAsync(MSGERROR);
            }

            //Cmd para obter a url
            if (conteudo.StartsWith("!url"))
            {
                //conteudo splitado
                string[] ConteudoSplit = conteudo.Split(" ");
                if (ConteudoSplit.Length >= 3)
                {  //aceitamos so 1 args pae
                    await Canal.SendMessageAsync($"{Msg.Author.Mention}\nApenas 1 argumento, porfavor.");
                    return;
                }
                ;
                //Messagem error
                string MSGERROR_INVALIDOURL = $"{Msg.Author.Mention}\nUrl Invalida verifique a url que voce nos forneceu.";
                //Testando URL
                //Regex by CHATGPT
                var rx = new Regex(@"^(?:https?:\/\/)?(?:www\.)?(?:youtube\.com\/(?:watch\?v=|embed\/|shorts\/)|youtu\.be\/)([A-Za-z0-9_-]{11})(?:[?&].*)?$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                //=====================================================================
                Console.WriteLine(ConteudoSplit[1]);
                string urltest = FixUrl(ConteudoSplit[1]);
                Match valido = rx.Match(urltest);
                //se ele for valido de acordo com regex
                if (valido.Success)
                {
                    //agora vamos verificar se ele é valido de acordo com YoutubeExplode
                    bool validoback = await Http.VerificarUrl(urltest);
                    if (validoback) { //url valido
                        Global.UsersIntrucao[IDUser].Url = urltest;
                        //Avisando
                        await Canal.SendMessageAsync($"{Msg.Author.Mention}\nUrl Adcionado!"); 
                    }
                    else await Canal.SendMessageAsync(MSGERROR_INVALIDOURL);
                }
                else await Canal.SendMessageAsync(MSGERROR_INVALIDOURL);
            }
            //Cmd para fazer o download
            if (conteudo.StartsWith("!bx"))
            {
                //Verificando o formato e url
                string url = Global.UsersIntrucao[IDUser].Url;
                string formato = Global.UsersIntrucao[IDUser].Formato;
                if (url == "") // url nao foi fornecida
                {
                    //avisando a faltar do argumente do url
                    await Canal.SendMessageAsync($"{Msg.Author.Mention}\nEsta faltando a url.");
                    //mostrar as informacoes
                    await ShowInfoStream(IDUser);
                    return;
                }
                else if (formato == "") // formato nao foi fornecido
                {
                    //avisando a faltar de argumento
                    await Canal.SendMessageAsync($"{Msg.Author.Mention}\nEsta faltando o formato.");
                    //Mostrando informacoes
                    await ShowInfoStream(IDUser);
                    return;
                }
                else
                {
                    //avisando que vai demora.
                    await Canal.SendMessageAsync($"{Msg.Author.Mention}\nPode demora alguns segundos...");
                    //colocando em outra task, para nao travar a principal
                    _ = Task.Run(async () =>
                    {
                        string? path = await Http.Abaixa(url, formato); // abaixando
                        if (path == null) // caso ele retorne null
                        {
                            await Canal.SendMessageAsync($"{Msg.Author.Mention}\nAlgo deu errado em abaixa o arquivo.");
                            return;
                        }
                        //path do arquivo obtido
                        //mandando
                        await Canal.SendFileAsync(path, Msg.Author.Mention);
                    });
                }
            }
        }

        //Exbir Informacao stream como url e formato
        public async Task ShowInfoStream(ulong ID)
        {
            //informacoes
            string url = Global.UsersIntrucao[ID].Url != "" ? Global.UsersIntrucao[ID].Url : "Sem Url";
            string formato = Global.UsersIntrucao[ID].Formato != "" ? Global.UsersIntrucao[ID].Formato : "Sem Formato";
            await Msg.Channel.SendMessageAsync($"{Msg.Author.Mention}\nInformacoes sobre o stream\nUrl: {url}\nFormato: {formato}\n\n**é necessario os dois valores para consigo obter o video ou audio**");
        }

        public static string FixUrl(string s)
        {
            return s
                .Replace("\u200b", "") // zero-width space
                .Replace("\u2060", "") // word joiner
                .Replace("\ufeff", "") // BOM
                .Replace("\n", "")
                .Replace("\r", "")
                .Trim('<', '>')
                .Trim();
        }
    }
}