# Documentação do Microserviço - Bemol Digital

Bem-vindo à documentação oficial do Microserviço. Esta documentação fornece informações detalhadas sobre como configurar, executar e usar o microserviço.

## Índice

1. [Visão Geral](#visão-geral)
2. [Pré-requisitos](#pré-requisitos)
3. [Configuração](#configuração)
4. [Compilação e Execução](#compilação-e-execução)
5. [Endpoints e APIs](#endpoints-e-apis)
6. [Estrutura do Código](#estrutura-do-código)
7. [Fluxos de Trabalho](#fluxos-de-trabalho)
8. [Integração](#integração)
9. [Testes](#testes)
10. [Funcionalidades](#funcionalidades)
11. [O que pode ser melhorado futuramente](#o-que-pode-ser-melhorado-futuramente)
12. [Referências](#referências)

## 1. Visão Geral

O Microserviço é uma aplicação onde foi desenvolvido dois microsserviços em C#, nesse projeto, o primeiro recebe um objeto no formato
JSON via HTTP POST, armazena esse objeto no banco de dados não relacional Azure Cosmos DB e envia uma mensagem para uma fila do Azure Service Bus com as informações do objeto armazenado.

O segundo microsserviço consome a fila e processaa os objetos do banco a partir dos dados que foram
recebidos. Ele pode ser acessado através desse link: https://github.com/Rogenis/Desafio-Bemol-Microservice-worker

## 2. Pré-requisitos

Antes de começar, certifique-se de ter instalado os seguintes pré-requisitos:
- .NET SDK
- Conta do Azure com acesso ao Azure Cosmos DB e Azure Service Bus.

## 3. Configuração

Siga estas etapas para configurar o ambiente:
1. Clone o repositório do microserviço.
2. Configure as variáveis de ambiente no arquivo `appsettings.json`.
- Exemplo:
```
{
  "ConnectionStrings": {
  "CosmosDBConnection": "AccountEndpoint=https://servicecosmosdbname.documents.azure.com:443;AccountKey=youaccountname;"
  },
  "ServiceBusQueueSettings": {
    "ConnectionString": "Endpoint=sb://servicebusname.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=yousharedacesskey",
    "QueueName": "filamicroservico"
  },
}
```
3. Nas linhas 57 e 58, edite as configurações do Nome do Bnco de dados e do Container.
```
  {
    string databaseName = "YourDtabaseName"; // Nome do banco de dados
    string containerName = "YourContainerName; // Nome do container
  }
```
4. No pasta Filters, entre no arquivo AuthorizationFilter, e edite a linha 8 para o valor que você deseja para a chave api key.
   - Essa chave deve validar o token de autenticação enviado no cabeçalho da requisição e retornar um erro 401 caso o token seja inválido ou não seja enviado
```
  {
    _expectedApiKey = "yoursecretkey"; 
  }
```

## 4. Compilação e Execução

Para compilar e executar o microserviço, siga os passos abaixo:
1. Abra o terminal e navegue até o diretório do projeto.
2. Execute o comando `dotnet build` para compilar o projeto.
3. Execute o comando `dotnet run` para iniciar o microserviço.

4. No postman (ou algumaoutra ferramente de solicitações HTTP, como o Insomnia), faça uma requisição POST para o endpoint `http://localhost:5000/api/MyController` com o seguinte corpo:
```
{
    "id": "1",
    "key": "value"
    "AnotherKey": "AnotherValue"
}
```
5. Ainda na ferramenta, coloque no Header o valor que você definiu para chave api key.
```
{
    "value": "yoursecretkey"
}
```
6.
  - [ ] Resultado deve ser o retorno do objeto.
   
  - [ ] Verificar se a mensagem foi salva na fila no Azure Service Bus
        
![Screenshot from 2023-08-17 16-47-40](https://github.com/Rogenis/Desafio-Bemol-Microservice/assets/49156356/fda3dd8d-9aac-482e-b35a-5bc95b507def)


- [ ] Verificar se o objeto foi salvo no banco no Azure Cosmos DB

![Screenshot from 2023-08-17 16-48-43](https://github.com/Rogenis/Desafio-Bemol-Microservice/assets/49156356/ddbc714d-3b72-48bc-8961-eb70fb803f2a)



## 5. Endpoints e APIs

O microserviço expõe os seguintes endpoints e APIs:

- `POST /api/MyController`: Cria um novo item.
- `GET api/my/test-exception`: Testa as exceções e retorna uma mensagem de erro genérica no formato JSON. 

## 6. Estrutura do Código

A estrutura do projeto é organizada da seguinte forma:
- `Controllers/`: Contém os controladores da aplicação.
- `DTOs/`: Contém as classes de Transferência de Dados.
- `Models/`: Contém os modelos de dados.
- `Services/`: Contém os serviços da aplicação.
- `appsettings.json`: Contém as configurações da aplicação.
- `Program.cs`: Contém o ponto de entrada da aplicação.
- `Startup.cs`: Contém a configuração da aplicação.
- `Repositóries`: Contém as classes de acesso ao banco de dados.
- `Filters`: Contém os filtros AuthorizationFilter, ExceptionFilter e ActionFilter.

## 7. Fluxos de Trabalho
*Em construção*
Descrever os fluxos de trabalho típicos suportados pelo microserviço, como criar um item, atualizar um item, etc.

## 8. Integração

Sistema se integra com o MicrosserviceWorker via GRPC. *Documentação em construção*

## 9. Testes

1. A pasta `Tests`: Contém as classes de testes unitários.
2. Navegue até a pagina, e execute o comando `dotnet run` para iniciar os testes unitários.


## 10. Funcionalidades
- Criar item no banco
*Em contrução*

## 11. O que pode ser melhorado futuramente:
- Uso do docker para facilitar a execução do projeto.
*Em contrução*

## 12. Referências

- Documentação do .NET SDK: [link](https://docs.microsoft.com/dotnet/core/sdk)
- Documentação do Azure Cosmos DB: [link](https://docs.microsoft.com/azure/cosmos-db/)
- Documentação do Azure Service Bus: [link](https://docs.microsoft.com/azure/service-bus/)
