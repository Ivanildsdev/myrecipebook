# Documentação de Testes

Este documento descreve a estratégia de testes do projeto MyRecipeBook e como executá-los.

## 📊 Visão Geral

O projeto utiliza uma estratégia de testes em camadas:

1. **Testes Unitários de Use Cases** - Testam a lógica de negócio isoladamente
2. **Testes Unitários de Validators** - Testam as regras de validação
3. **Testes de Integração da API** - Testam os endpoints end-to-end

## 🧪 Estrutura de Testes

```
tests/
├── CommonTestUtilities/     # Utilitários compartilhados
│   ├── Entities/            # Builders de entidades
│   ├── Repositories/         # Mocks de repositórios
│   ├── Requests/            # Builders de requests
│   └── ...
├── UseCase.Test/            # Testes de Use Cases
│   ├── Login/
│   └── User/
├── Validators.Test/         # Testes de Validators
│   └── User/
└── WebApi.Test/             # Testes de integração
    ├── Login/
    └── User/
```

## 🛠️ Ferramentas Utilizadas

- **xUnit** - Framework de testes
- **FluentAssertions** - Asserções legíveis
- **Moq** - Mocking framework
- **Bogus** - Geração de dados fake
- **Microsoft.AspNetCore.Mvc.Testing** - Testes de integração da API
- **Microsoft.EntityFrameworkCore.InMemory** - Banco em memória para testes

## 🎯 Tipos de Testes

### 1. Testes Unitários de Use Cases

Testam a lógica de negócio dos casos de uso isoladamente, usando mocks para dependências.

**Localização:** `tests/UseCase.Test/`

**Exemplo:**
```csharp
[Fact]
public async Task Execute_Success()
{
    // Arrange
    var repositoryMock = UserReadOnlyRepositoryBuilder.Build();
    var useCase = new DoLoginUseCase(repositoryMock, ...);
    var request = RequestLoginJsonBuilder.Build();

    // Act
    var response = await useCase.Execute(request);

    // Assert
    response.Should().NotBeNull();
    response.Token.Should().NotBeEmpty();
}
```

### 2. Testes Unitários de Validators

Testam as regras de validação do FluentValidation.

**Localização:** `tests/Validators.Test/`

**Exemplo:**
```csharp
[Theory]
[InlineData("")]
[InlineData("ab")]
[InlineData(null)]
public void Validate_InvalidName_ShouldReturnError(string name)
{
    // Arrange
    var validator = new RegisterUserValidator();
    var request = RequestRegisterUserJsonBuilder.Build();
    request.Name = name;

    // Act
    var result = validator.Validate(request);

    // Assert
    result.IsValid.Should().BeFalse();
    result.Errors.Should().Contain(e => e.PropertyName == "Name");
}
```

### 3. Testes de Integração da API

Testam os endpoints HTTP end-to-end, incluindo middleware, filtros e controllers.

**Localização:** `tests/WebApi.Test/`

**Exemplo:**
```csharp
[Fact]
public async Task Login_Success()
{
    // Arrange
    var request = RequestLoginJsonBuilder.Build();
    await RegisterUser(request);

    // Act
    var response = await _client.PostAsync("/login", request.ToJsonContent());

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.OK);
    var responseJson = await response.Content.ReadFromJsonAsync<ResponseRegisteredUserJson>();
    responseJson.Should().NotBeNull();
    responseJson.Token.Should().NotBeEmpty();
}
```

## 🚀 Executando Testes

### Executar Todos os Testes

```bash
dotnet test
```

### Executar Testes de um Projeto Específico

```bash
# Testes de Use Cases
dotnet test tests/UseCase.Test/UseCase.Test.csproj

# Testes de Validators
dotnet test tests/Validators.Test/Validators.Test.csproj

# Testes de Integração
dotnet test tests/WebApi.Test/WebApi.Test.csproj
```

### Executar com Cobertura

```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

### Executar Testes Específicos

```bash
# Por nome
dotnet test --filter "FullyQualifiedName~DoLoginUseCaseTest"

# Por categoria (se configurado)
dotnet test --filter "Category=Integration"
```

## 📝 Padrões de Teste

### Arrange-Act-Assert (AAA)

Todos os testes seguem o padrão AAA:

```csharp
[Fact]
public async Task TestName()
{
    // Arrange - Configurar o cenário
    var useCase = CreateUseCase();
    var request = RequestBuilder.Build();

    // Act - Executar a ação
    var response = await useCase.Execute(request);

    // Assert - Verificar o resultado
    response.Should().NotBeNull();
}
```

### Builders Pattern

Utilizamos builders para criar objetos de teste:

```csharp
// Criar request
var request = RequestRegisterUserJsonBuilder.Build();

// Criar request customizado
var request = RequestRegisterUserJsonBuilder.Build()
    .WithEmail("custom@email.com")
    .WithName("Custom Name");
```

### Test Data Builders

Builders disponíveis em `CommonTestUtilities`:

- `RequestLoginJsonBuilder`
- `RequestRegisterUserJsonBuilder`
- `RequestUpdateUserJsonBuilder`
- `UserBuilder`
- `UserReadOnlyRepositoryBuilder`
- `UserWriteOnlyRepositoryBuilder`

## 🔧 Configuração de Testes

### WebApi.Test

Utiliza `CustomWebApplicationFactory` para configurar a aplicação de teste:

```csharp
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");
        // Configurações específicas de teste
    }
}
```

### Banco de Dados em Memória

Os testes de integração utilizam Entity Framework InMemory:

```csharp
services.AddDbContext<MyRecipeBookDbContext>(options =>
    options.UseInMemoryDatabase("TestDatabase"));
```

## 📊 Cobertura de Código

### Visualizar Cobertura

```bash
# Gerar relatório
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura

# Abrir no navegador (requer reportgenerator)
reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:"coverage" -reporttypes:Html
```

### Metas de Cobertura

- Use Cases: > 80%
- Validators: > 90%
- Controllers: > 70%

## 🐛 Debugging de Testes

### Visual Studio

1. Defina pontos de interrupção no código de teste
2. Clique com botão direito no teste → "Debug Test"

### VS Code

1. Configure `.vscode/launch.json`:
```json
{
  "name": ".NET Core Test",
  "type": "coreclr",
  "request": "launch",
  "preLaunchTask": "build",
  "program": "dotnet",
  "args": ["test", "--no-build"],
  "cwd": "${workspaceFolder}"
}
```

## ✅ Checklist de Testes

Ao escrever testes:

- [ ] Teste cobre o caso de sucesso (happy path)
- [ ] Teste cobre casos de erro/validação
- [ ] Teste é independente (não depende de outros testes)
- [ ] Teste é determinístico (sempre produz o mesmo resultado)
- [ ] Nome do teste descreve claramente o que está sendo testado
- [ ] Usa builders para criar objetos de teste
- [ ] Usa FluentAssertions para asserções legíveis

## 📚 Exemplos Práticos

### Teste de Use Case Completo

```csharp
public class RegisterUserUseCaseTest
{
    [Fact]
    public async Task Execute_Success_ShouldReturnUserWithToken()
    {
        // Arrange
        var repositoryMock = UserWriteOnlyRepositoryBuilder.Build();
        var mapper = MapperBuilder.Build();
        var tokenGenerator = JwtTokenGeneratorBuilder.Build();
        var encripter = PasswordEncripterBuilder.Build();
        
        var useCase = new RegisterUserUseCase(
            repositoryMock.Object,
            mapper,
            tokenGenerator.Object,
            encripter.Object
        );
        
        var request = RequestRegisterUserJsonBuilder.Build();

        // Act
        var response = await useCase.Execute(request);

        // Assert
        response.Should().NotBeNull();
        response.Name.Should().Be(request.Name);
        response.Email.Should().Be(request.Email);
        response.Token.Should().NotBeEmpty();
        
        repositoryMock.Verify(r => r.Add(It.IsAny<User>()), Times.Once);
    }
}
```

### Teste de Validação Completo

```csharp
public class RegisterUserValidatorTest
{
    [Theory]
    [InlineData("")]
    [InlineData("ab")]
    [InlineData("a")]
    public void Validate_InvalidName_ShouldReturnError(string name)
    {
        // Arrange
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = name;

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => 
            e.PropertyName == "Name" && 
            e.ErrorMessage.Contains("nome")
        );
    }
}
```

## 🔍 Troubleshooting

### Testes Falhando Aleatoriamente

- Verifique se os testes são independentes
- Certifique-se de que não há estado compartilhado
- Use `[Fact]` ao invés de `[Theory]` se necessário

### Problemas com Banco de Dados

- Certifique-se de usar InMemory para testes
- Limpe o banco entre testes se necessário

### Mocks Não Funcionando

- Verifique se está usando `Moq` corretamente
- Certifique-se de que as interfaces estão corretas
- Use `Verify()` para garantir que métodos foram chamados


