# Documentação do MyRecipeBook

Bem-vindo à documentação do projeto MyRecipeBook, uma aplicação .NET para gerenciar seu livro de receitas pessoal.

## 📚 Índice

- [Visão Geral](#visão-geral)
- [Arquitetura](#arquitetura)
- [API](#api)
- [Configuração](#configuração)
- [Desenvolvimento](#desenvolvimento)
- [Testes](#testes)

## 🎯 Visão Geral

MyRecipeBook é uma aplicação web desenvolvida em .NET 8.0 que permite aos usuários gerenciar suas receitas pessoais. A aplicação oferece funcionalidades de autenticação, gerenciamento de perfil de usuário e operações CRUD para receitas.

### Tecnologias Principais

- **.NET 8.0** - Framework principal
- **ASP.NET Core** - API Web
- **Entity Framework Core** - ORM para acesso a dados
- **FluentMigrator** - Migrações de banco de dados
- **JWT** - Autenticação e autorização
- **Swagger/OpenAPI** - Documentação da API
- **xUnit** - Framework de testes
- **FluentAssertions** - Asserções de testes

## 🏗️ Arquitetura

O projeto segue uma arquitetura em camadas (Clean Architecture) com separação clara de responsabilidades:

```
src/
├── Backend/
│   ├── MyRecipeBook.API/          # Camada de apresentação (Controllers, Middleware, Filters)
│   ├── MyRecipeBook.Application/  # Camada de aplicação (Use Cases, Validators)
│   ├── MyRecipeBook.Domain/        # Camada de domínio (Entities, Contracts, Services)
│   └── MyRecipeBook.Infrastructure/# Camada de infraestrutura (DataAccess, Security, Migrations)
└── Shared/
    ├── MyRecipeBook.Communication/ # DTOs (Requests e Responses)
    └── MyRecipeBook.Exceptions/    # Exceções customizadas

tests/
├── CommonTestUtilities/           # Utilitários compartilhados para testes
├── UseCase.Test/                  # Testes unitários dos Use Cases
├── Validators.Test/               # Testes unitários dos Validators
└── WebApi.Test/                   # Testes de integração da API
```

Para mais detalhes sobre a arquitetura, consulte [Arquitetura](./architecture/README.md).

## 🔌 API

A API RESTful oferece os seguintes endpoints:

### Autenticação
- `POST /login` - Realizar login e obter token JWT

### Usuário
- `POST /user` - Registrar novo usuário
- `GET /user` - Obter perfil do usuário autenticado
- `PUT /user` - Atualizar dados do usuário
- `PUT /user/change-password` - Alterar senha do usuário

Para documentação completa da API, consulte [Documentação da API](./api/README.md).

## ⚙️ Configuração

A aplicação utiliza arquivos `appsettings.json` para configuração:

- `appsettings.json` - Configurações de produção
- `appsettings.Development.json` - Configurações de desenvolvimento
- `appsettings.Test.json` - Configurações para testes

Para mais detalhes sobre configuração, consulte [Configuração](./configuration/README.md).

## 💻 Desenvolvimento

### Pré-requisitos

- .NET 8.0 SDK
- SQL Server (ou banco de dados compatível)
- Visual Studio 2022 ou VS Code

### Executando a Aplicação

```bash
# Restaurar pacotes NuGet
dotnet restore

# Executar migrações do banco de dados
# (As migrações são executadas automaticamente na inicialização)

# Executar a aplicação
dotnet run --project src/Backend/MyRecipeBook.API/MyRecipeBook.API.csproj
```

A aplicação estará disponível em `https://localhost:5001` (ou porta configurada).

### Swagger

Em ambiente de desenvolvimento, o Swagger está disponível em:
- `https://localhost:5001/swagger`

Para mais informações sobre desenvolvimento, consulte [Guia de Desenvolvimento](./development/README.md).

## 🧪 Testes

O projeto inclui três tipos de testes:

1. **Testes Unitários de Use Cases** (`tests/UseCase.Test`)
2. **Testes Unitários de Validators** (`tests/Validators.Test`)
3. **Testes de Integração da API** (`tests/WebApi.Test`)

### Executando os Testes

```bash
# Executar todos os testes
dotnet test

# Executar testes de um projeto específico
dotnet test tests/UseCase.Test/UseCase.Test.csproj
dotnet test tests/Validators.Test/Validators.Test.csproj
dotnet test tests/WebApi.Test/WebApi.Test.csproj
```

Para mais informações sobre testes, consulte [Documentação de Testes](./tests/README.md).

## 📝 Convenções

- **Nomenclatura**: PascalCase para classes e métodos, camelCase para variáveis locais
- **Estrutura**: Um Use Case por arquivo, seguindo o padrão CQRS
- **Validação**: FluentValidation para validação de requisições
- **Mapeamento**: AutoMapper para mapeamento entre entidades e DTOs
- **Testes**: xUnit com FluentAssertions

## 🤝 Contribuindo

Este é um projeto de estudo. Sinta-se à vontade para explorar o código e fazer melhorias.

## 📄 Licença

Este projeto é para fins educacionais.


