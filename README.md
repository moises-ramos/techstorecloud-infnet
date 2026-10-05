# Projeto Final TechStore - Cloud - MVP

#Sobre o Projeto
*Este é um projeto de MVP para cadastro de produtos criado para a disciplina de Sistemas em Nuvem com Azure. O objetivo é demonstrar a comunicação entre microsserviços na plataforma Azure.*

## Tecnologias Utilizadas

- **FrontEnd:** HTML, CSS, JavaScript (Hospadado no Azure Storage Account)
- **BackEnd:** C#, .NET 8, ASP.NET Core Web API (Hospadado no Azure App Service)
- **Banco de Dados:** Azure SQL Database com Entity FrameWork Core

## Arquitetura da Solução
*O frontend faz requisições HTTP para a API no App Service. A API se conecta ao Azure SQL Database usando Entity Framework. Precisei mover o frontend para o Storage Account devido a restrições na assinatura de estudante.*

## Como rodar o projeto localmente
*Passo a passo para poder testar o projeto no computador pessoal*
1. Precisa ter o SDK do .NET 8 instalado.
2. Alterar a string de conexão no `appsettings.Development.json`.
3. Executar o comando `dotnet run` na pasta da API => src\TechStore.Api.
