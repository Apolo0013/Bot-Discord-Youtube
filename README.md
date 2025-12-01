# Bot Discord – Download de Vídeo/Áudio do YouTube

Um bot simples feito em **C#** usando a biblioteca **Discord.Net**, com a função de **baixar vídeos ou áudios do YouTube** e enviar no Discord.  
O download é feito usando um código próprio (ainda sem repositório público).

---

## 🚀 Funcionalidades

- Baixar **vídeo** do YouTube.  
- Baixar **áudio** do YouTube.  
- Responder mensagens no servidor.  
- Sistema simples e direto.

---

## 📦 Tecnologias

O bot usa:

- **Discord.Net**
- **Discord.Net.WebSocket**

Instalação via .NET CLI:

```
dotnet add package Discord.Net
dotnet add package Discord.Net.WebSocket
```

---

## ⚙️ Configuração

1. Clone o projeto.
2. Abra a solução no Visual Studio / VS Code.
3. Instale os pacotes acima.
4. No arquivo onde o bot inicia, procure:

```csharp
string Token = "";
```

5. Coloque o token do seu bot ali:

```csharp
string Token = "SEU_TOKEN_AQUI";
```

6. Rode a aplicação:

```
dotnet run
```

---

## 🧩 Download de Vídeo/Áudio

O download usa um código auxiliar próprio -> [**Repositorio**](https://github.com/Apolo0013/YouTube-Backend-Download).  
Ele recebe o link do YouTube e retorna o arquivo convertido para vídeo ou áudio.

Você só precisa enviar o link no Discord conforme sua lógica de mensagens.

---

## 🔧 Uso

1. Adicione o bot ao servidor.
2. Escreva o comando ou mensagem definida por você.  
3. O bot baixa e te envia o vídeo/áudio.

Simples.

---

## 📄 Requisitos

- .NET 6+
- Token de bot válido do Discord
- Permissão para ler e enviar mensagens

---

## 📝 Observação

Nenhum token está incluso no repositório.  
Certifique-se de **manter seu token sempre privado**.

---

Pronto. Bot simples, funcional e direto.
