# Sistema de Gestão de Portfólio de Investimentos

Este é um sistema de gestão de portfólio de investimentos desenvolvido em C# para uma empresa de consultoria financeira. A aplicação permite que os usuários da operação gerenciem investimentos disponíveis, e os clientes possam comprar, vender e acompanhar seus investimentos.

## Funcionalidades Principais

- **Gestão de Produtos Financeiros**: 
  - CRUD para produtos financeiros (criar, ler, atualizar e deletar).
  - Disparo de e-mails diários para notificar administradores sobre produtos com vencimento próximo.

- **Serviço de Negociação**:
  - Comprar e vender produtos financeiros.
  - Consulta de extratos de transações para clientes e produtos.

- **Desempenho e Escalabilidade**:
  - A aplicação suporta um grande volume de requisições com um tempo de resposta inferior a 100ms.

## Requisitos

- .NET 6.0 ou superior
- SQL Server
- SMTP para envio de e-mails
- Ferramentas para teste de carga (opcional): K6

## Configuração

### Banco de Dados

1. Configure o SQL Server e crie um banco de dados chamado `InvestmentDb`.
2. Atualize a string de conexão em `appsettings.json` com suas credenciais de banco de dados: (ou mantenha as mesmas, preferencialmente)


"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=InvestmentDb;Trusted_Connection=True;TrustServerCertificate=True;"
}

### Configuração de Email

Mantenha as mesmas configurações, criei esse e-mail apenas para essa API.

### Executando a Aplicação

Restaure os pacotes NuGet:
dotnet restore

### Aplique as migrações para o banco de dados:
dotnet ef database update

### Execute a aplicação:
dotnet run

## A API estará disponível em https://localhost:5001.

###Endpoints da API
Os endpoints estão documentados no próprio Swagger.

### Testes de Carga

Utilize o K6 para realizar testes de carga:
- Instale o K6.
- Execute o script de teste de carga:
k6 run testeCarga.js
