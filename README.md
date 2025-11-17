# MyRecipeBook

API REST desenvolvida em .NET para gerenciamento de receitas culinárias, seguindo os princípios da Clean Architecture.

## Sobre o Projeto

O MyRecipeBook é uma aplicação backend que permite aos usuários gerenciar suas receitas culinárias de forma organizada e segura. O projeto foi desenvolvido seguindo as melhores práticas de arquitetura de software, garantindo código limpo, testável e escalável.

## Funcionalidades

- Autenticação JWT - Sistema de autenticação seguro com tokens JWT
- Gerenciamento de Usuários - Registro, perfil e atualização de dados do usuário
- Segurança - Criptografia de senhas com SHA512
- API REST - Endpoints RESTful bem estruturados
- Validação - Validação robusta de dados de entrada
- Testes - Cobertura de testes unitários e de integração

## Tecnologias

- .NET - Framework principal
- ASP.NET Core - Framework web
- Entity Framework Core - ORM para acesso a dados
- FluentMigrator - Migrações de banco de dados
- SQL Server - Banco de dados relacional
- JWT - Autenticação e autorização
- Swagger/OpenAPI - Documentação da API
- AutoMapper - Mapeamento de objetos
- xUnit - Framework de testes

## Arquitetura

O projeto segue os princípios da Clean Architecture, organizando o código em camadas bem definidas:

```
┌─────────────────────────────────────────┐
│         MyRecipeBook.API                │
│   (Controllers, Middleware, Filters)    │
└─────────────────┬───────────────────────┘
                  │
┌─────────────────▼───────────────────────┐
│      MyRecipeBook.Application           │
│   (Use Cases, Validators, Services)     │
└─────────────────┬───────────────────────┘
                  │
┌─────────────────▼───────────────────────┐
│        MyRecipeBook.Domain              │
│  (Entities, Contracts, Domain Services) │
└─────────────────┬───────────────────────┘
                  │
┌─────────────────▼───────────────────────┐
│    MyRecipeBook.Infrastructure          │
│ (DataAccess, Security, External Services)│
└─────────────────────────────────────────┘
```

### Camadas

- **API**: Camada de apresentação, responsável por receber requisições HTTP e retornar respostas
- **Application**: Camada de aplicação, contém a lógica de negócio e casos de uso
- **Domain**: Camada de domínio, define entidades e contratos (interfaces)
- **Infrastructure**: Camada de infraestrutura, implementa acesso a dados e serviços externos

## Estrutura do Projeto

```
myrecipebook/
├── src/
│   ├── Backend/
│   │   ├── MyRecipeBook.API/
│   │   ├── MyRecipeBook.Application/
│   │   ├── MyRecipeBook.Domain/
│   │   └── MyRecipeBook.Infrastructure/
│   └── Shared/
│       ├── MyRecipeBook.Communication/
│       └── MyRecipeBook.Exceptions/
├── tests/
│   ├── UseCase.Test/
│   ├── Validators.Test/
│   ├── WebApi.Test/
│   └── CommonTestUtilities/
└── docs/
    ├── architecture/
    ├── api/
    └── ...
```

## Como Executar

### Pré-requisitos

- .NET SDK (versão 8.0 ou superior)
- SQL Server (ou SQL Server Express)
- Visual Studio 2022 ou VS Code

### Configuração

1. Clone o repositório:

```bash
git clone https://github.com/seu-usuario/myrecipebook.git
cd myrecipebook
```

2. Configure a string de conexão no arquivo `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=MyRecipeBook;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

3. Configure as chaves de segurança no `appsettings.json`:

```json
{
  "JwtTokenSettings": {
    "SecretKey": "sua-chave-secreta-aqui",
    "ExpirationTimeInHours": 2
  },
  "PasswordSettings": {
    "AdditionalKey": "chave-adicional-criptografia"
  }
}
```

4. Restaure as dependências:

```bash
dotnet restore
```

5. Execute as migrações (são executadas automaticamente na inicialização)

6. Execute a aplicação:

```bash
cd src/Backend/MyRecipeBook.API
dotnet run
```

7. Acesse a documentação Swagger em: `https://localhost:5001/swagger`

## Testes

Execute os testes com o comando:

```bash
dotnet test
```

## Documentação

Para mais detalhes sobre a arquitetura e implementação, consulte:

- [Documentação de Arquitetura](docs/architecture/README.md)
- [Documentação da API](docs/api/README.md)
- [Guia de Desenvolvimento](docs/development/README.md)

## Segurança

- Senhas são criptografadas usando SHA512 com chave adicional
- Autenticação baseada em JWT (JSON Web Tokens)
- Validação de entrada em todas as requisições
- Tratamento centralizado de exceções

## Princípios Aplicados

- **Separation of Concerns** - Separação clara de responsabilidades
- **Dependency Inversion** - Dependências apontam para abstrações
- **Single Responsibility** - Cada classe tem uma única responsabilidade
- **Open/Closed** - Aberto para extensão, fechado para modificação
- **Interface Segregation** - Interfaces específicas e focadas

## Contribuindo

Contribuições são bem-vindas. Sinta-se à vontade para abrir uma issue ou enviar um pull request.

## Licença

Este projeto está sob a licença MIT. Veja o arquivo `LICENSE` para mais detalhes.
