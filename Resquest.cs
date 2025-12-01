using System.Net.Http.Json;
using Bot.Run;

//Type
using Bot.Type;

namespace Bot.Resquest
{
    class Resquest
    {
        private readonly HttpClient Http = new HttpClient();

        public async Task<bool> VerificarUrl(string url)
        {
            var resposta = await Http.GetFromJsonAsync<VerificarUrlType>($"http://localhost:5194/VerificarURL?url={url}");
            return resposta!.Valido;
        }

        public async Task<string?> Abaixa(string url, string formato)
        {
            string PathArquivos = Path.Combine(Directory.GetCurrentDirectory(), "arquivos");
            //Criando a pasta ondem vai fica os arquivo caso ele nao exista
            if (File.Exists(PathArquivos)) Directory.CreateDirectory(PathArquivos);
            //ID
            string ID = Guid.NewGuid().ToString();
            if (formato == Global.VIDEO)
            {
                byte[]? buffer = await Http.GetByteArrayAsync($"http://localhost:5194/AbaixaMelhor?url={url}&formato=video");
                Console.WriteLine(buffer);
                if (buffer == null) return null; // algo deu errado
                //escrevendo o buffer
                string path = Path.Combine(PathArquivos, $"{ID}.mp4");
                await File.WriteAllBytesAsync(path, buffer);
                return path;
            }
            else if (formato == Global.AUDIO)
            {
                byte[] buffer = await Http.GetByteArrayAsync($"http://localhost:5194/AbaixaMelhor?url={url}&formato=audio");
                if (buffer == null) return null; // algo deu errado
                //escrevendo
                string path = Path.Combine(PathArquivos ,$"{ID}.mp3");
                await File.WriteAllBytesAsync(path, buffer);
                return path;
            }
            else return null;
        }
    }
}