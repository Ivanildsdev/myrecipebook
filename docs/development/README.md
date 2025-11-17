# Guia de Desenvolvimento

Este guia fornece informações para desenvolvedores que desejam contribuir ou trabalhar no projeto MyRecipeBook.

## 🚀 Início Rápido

### Pré-requisitos

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/sql-server) ou [SQL Server Express](https://www.microsoft.com/sql-server/sql-server-downloads)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) ou [VS Code](https://code.visualstudio.com/)
- [Git](https://git-scm.com/)

### Configuração Inicial

1. **Clone o repositório:**
```bash
git clone <url-do-repositorio>
cd myrecipebook
```

2. **Restaurar pacotes NuGet:**
```bash
dotnet restore
```

3. **Configurar banco de dados:**
   - Edite `src/Backend/MyRecipeBook.API/appsettings.Development.json`
   - Configure a connection string do SQL Server

4. **Executar a aplicação:**
```bash
dotnet run --project src/Backend/MyRecipeBook.API/MyRecipeBook.API.csproj
```

5. **Acessar Swagger:**
   - Abra o navegador em `https://localhost:5001/swagger`

## 📁 Estrutura do Projeto

```
myrecipebook/
├── src/
│   ├── Backend/
│   │   ├── MyRecipeBook.API/          # API REST
│   │   ├── MyRecipeBook.Application/  # Casos de uso
│   │   ├── MyRecipeBook.Domain/       # Domínio
│   │   └── MyRecipeBook.Infrastructure/ # Infraestrutura
│   └── Shared/
│       ├── MyRecipeBook.Communication/ # DTOs
│       └── MyRecipeBook.Exceptions/    # Exceções
├── tests/
│   ├── CommonTestUtilities/           # Utilitários de teste
│   ├── UseCase.Test/                  # Testes de Use Cases
│   ├── Validators.Test/               # Testes de Validators
│   └── WebApi.Test/                   # Testes de integração
└── docs/                              # Documentação
```

## 🏗️ Arquitetura

O projeto segue os princípios da Clean Architecture:

- **API**: Camada de apresentação (Controllers, Middleware)
- **Application**: Lógica de aplicação (Use Cases, Validators)
- **Domain**: Regras de negócio (Entities, Contracts)
- **Infrastructure**: Implementações técnicas (DataAccess, Security)

Para mais detalhes, consulte [Arquitetura](../architecture/README.md).

## 💻 Convenções de Código

### Nomenclatura

- **Classes**: PascalCase (`UserController`, `RegisterUserUseCase`)
- **Interfaces**: Prefixo `I` + PascalCase (`IUserRepository`, `IDoLoginUseCase`)
- **Métodos**: PascalCase (`Execute`, `Validate`)
- **Variáveis locais**: camelCase (`userName`, `emailAddress`)
- **Constantes**: PascalCase (`MAX_RETRY_COUNT`)

### Estrutura de Arquivos

**Use Cases:**
```
UseCases/
└── User/
    └── Register/
        ├── IRegisterUserUseCase.cs
        ├── RegisterUserUseCase.cs
        └── RegisterUserValidator.cs
```

**Controllers:**
```
Controllers/
└── UserController.cs
```

### Padrões de Código

**Use Case Pattern:**
```csharp
public interface IRegisterUserUseCase
{
    Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request);
}

public class RegisterUserUseCase : IRegisterUserUseCase
{
    public async Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request)
    {
        // Implementação
    }
}
```

**Repository Pattern:**
```csharp
public interface IUserReadOnlyRepository
{
    Task<User?> GetByEmail(string email);
    Task<bool> ExistsActiveUserWithEmail(string email);
}
```

## 🧪 Desenvolvimento Orientado por Testes

### Executando Testes

```bash
# Todos os testes
dotnet test

# Testes específicos
dotnet test tests/UseCase.Test/UseCase.Test.csproj

# Com cobertura
dotnet test /p:CollectCoverage=true
```

### Escrevendo Testes

**Exemplo de teste de Use Case:**
```csharp
[Fact]
public async Task Execute_Success()
{
    // Arrange
    var useCase = CreateUseCase();
    var request = RequestRegisterUserJsonBuilder.Build();

    // Act
    var response = await useCase.Execute(request);

    // Assert
    response.Should().NotBeNull();
    response.Email.Should().Be(request.Email);
}
```

## 🔄 Fluxo de Trabalho

### Adicionando um Novo Use Case

1. **Criar interface no Domain (se necessário):**
```csharp
// Domain/Contracts/Services/INewService.cs
public interface INewService
{
    Task<Result> DoSomething();
}
```

2. **Criar Use Case:**
```csharp
// Application/UseCases/NewFeature/INewFeatureUseCase.cs
public interface INewFeatureUseCase
{
    Task<ResponseJson> Execute(RequestJson request);
}
```

3. **Implementar Use Case:**
```csharp
// Application/UseCases/NewFeature/NewFeatureUseCase.cs
public class NewFeatureUseCase : INewFeatureUseCase
{
    // Implementação
}
```

4. **Criar Validator:**
```csharp
// Application/UseCases/NewFeature/NewFeatureValidator.cs
public class NewFeatureValidator : AbstractValidator<RequestJson>
{
    // Regras de validação
}
```

5. **Criar Controller:**
```csharp
// API/Controllers/NewFeatureController.cs
[HttpPost]
public async Task<IActionResult> Execute(
    [FromServices] INewFeatureUseCase useCase,
    [FromBody] RequestJson request)
{
    var response = await useCase.Execute(request);
    return Ok(response);
}
```

6. **Registrar no DI:**
```csharp
// Application/DependencyInjectionExtension.cs
services.AddScoped<INewFeatureUseCase, NewFeatureUseCase>();
```

## 🐛 Debugging

### Visual Studio

1. Defina pontos de interrupção
2. Pressione F5 para iniciar com debug
3. Use a janela de variáveis locais para inspecionar valores

### VS Code

1. Configure `.vscode/launch.json`:
```json
{
  "version": "0.2.0",
  "configurations": [
    {
      "name": ".NET Core Launch (web)",
      "type": "coreclr",
      "request": "launch",
      "preLaunchTask": "build",
      "program": "${workspaceFolder}/src/Backend/MyRecipeBook.API/bin/Debug/net8.0/MyRecipeBook.API.dll",
      "args": [],
      "cwd": "${workspaceFolder}/src/Backend/MyRecipeBook.API",
      "stopAtEntry": false,
      "serverReadyAction": {
        "action": "openExternally",
        "pattern": "\\bNow listening on:\\s+(https?://\\S+)"
      }
    }
  ]
}
```

## 📦 Gerenciamento de Pacotes

### Adicionar Pacote NuGet

```bash
# Adicionar a um projeto específico
dotnet add package NomeDoPacote --version 1.0.0

# Exemplo
dotnet add src/Backend/MyRecipeBook.Application/MyRecipeBook.Application.csproj package FluentValidation
```

### Atualizar Pacotes

```bash
# Listar pacotes desatualizados
dotnet list package --outdated

# Atualizar todos os pacotes
dotnet add package NomeDoPacote --version NovaVersao
```

## 🔍 Ferramentas Úteis

### Análise de Código

```bash
# Analisar código
dotnet format

# Verificar problemas
dotnet build --no-restore
```

### Migrações

```bash
# Criar nova migração
dotnet ef migrations add NomeDaMigracao --project src/Backend/MyRecipeBook.Infrastructure

# Aplicar migrações
dotnet ef database update --project src/Backend/MyRecipeBook.Infrastructure
```

## 📝 Checklist de Desenvolvimento

Ao adicionar uma nova funcionalidade:

- [ ] Criar Use Case seguindo o padrão estabelecido
- [ ] Criar Validator com FluentValidation
- [ ] Criar testes unitários
- [ ] Criar testes de integração (se aplicável)
- [ ] Atualizar documentação da API
- [ ] Verificar se o código compila sem erros
- [ ] Executar todos os testes
- [ ] Verificar cobertura de código

## 🚨 Problemas Comuns

### Erro de Connection String

**Problema:** `Invalid connection string`

**Solução:** Verifique se a connection string está correta no `appsettings.Development.json`

### Erro de Migração

**Problema:** `Migration failed`

**Solução:** Certifique-se de que o banco de dados existe e a connection string está correta

### Erro de Token JWT

**Problema:** `Token validation failed`

**Solução:** Verifique se a `SigningKey` no `appsettings.json` tem pelo menos 32 caracteres

## 📚 Recursos Adicionais

- [Documentação .NET](https://docs.microsoft.com/dotnet/)
- [ASP.NET Core Docs](https://docs.microsoft.com/aspnet/core/)
- [Entity Framework Core](https://docs.microsoft.com/ef/core/)
- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)


