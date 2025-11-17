# Configuração

Este documento descreve as configurações disponíveis no projeto MyRecipeBook.

## 📁 Arquivos de Configuração

O projeto utiliza arquivos `appsettings.json` para configuração:

- `appsettings.json` - Configurações padrão (produção)
- `appsettings.Development.json` - Configurações de desenvolvimento
- `appsettings.Test.json` - Configurações para ambiente de testes
- `appsettings.Test.Development.json` - Configurações de testes em desenvolvimento

## ⚙️ Configurações Disponíveis

### ConnectionStrings

Configuração da string de conexão com o banco de dados:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=MyRecipeBook;User Id=sa;Password=YourPassword;TrustServerCertificate=true"
  }
}
```

### JwtSettings

Configurações do token JWT:

```json
{
  "JwtSettings": {
    "SigningKey": "sua-chave-secreta-aqui-minimo-32-caracteres",
    "ExpirationTimeMinutes": 60
  }
}
```

**Parâmetros:**
- `SigningKey`: Chave secreta para assinar os tokens JWT (mínimo 32 caracteres)
- `ExpirationTimeMinutes`: Tempo de expiração do token em minutos

### PasswordSettings

Configurações de criptografia de senha:

```json
{
  "PasswordSettings": {
    "AdditionalKey": "chave-adicional-para-criptografia"
  }
}
```

**Parâmetros:**
- `AdditionalKey`: Chave adicional usada na criptografia SHA512 das senhas

### Logging

Configuração de logging:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

## 🔧 Variáveis de Ambiente

Você pode sobrescrever configurações usando variáveis de ambiente. O ASP.NET Core utiliza o padrão de nomenclatura com dois pontos (`:`) ou duplo underscore (`__`):

**Windows (PowerShell):**
```powershell
$env:ConnectionStrings__DefaultConnection = "Server=localhost;Database=MyRecipeBook;..."
$env:JwtSettings__SigningKey = "nova-chave-secreta"
```

**Linux/Mac:**
```bash
export ConnectionStrings__DefaultConnection="Server=localhost;Database=MyRecipeBook;..."
export JwtSettings__SigningKey="nova-chave-secreta"
```

## 🗄️ Configuração do Banco de Dados

### SQL Server

Exemplo de connection string para SQL Server:

```
Server=localhost;Database=MyRecipeBook;User Id=sa;Password=YourPassword;TrustServerCertificate=true
```

### SQLite (Desenvolvimento)

Para usar SQLite em desenvolvimento, você pode modificar a connection string:

```
Data Source=MyRecipeBook.db
```

**Nota:** Certifique-se de que o provider SQLite está configurado no projeto de Infrastructure.

## 🔐 Configuração de Segurança

### Gerando Chaves Seguras

Para produção, é recomendado gerar chaves seguras:

**JWT Signing Key:**
```bash
# Usando OpenSSL
openssl rand -base64 32
```

**Password Additional Key:**
```bash
# Usando OpenSSL
openssl rand -base64 16
```

### Configuração em Produção

⚠️ **IMPORTANTE:** Nunca commite chaves secretas no repositório!

Para produção, utilize:
- Variáveis de ambiente
- Azure Key Vault
- AWS Secrets Manager
- Outros serviços de gerenciamento de segredos

## 🌍 Configuração de Cultura

A aplicação suporta múltiplos idiomas através do middleware `CultureMiddleware`. A cultura padrão pode ser configurada:

```json
{
  "CultureSettings": {
    "DefaultCulture": "pt-BR",
    "SupportedCultures": ["pt-BR", "en-US"]
  }
}
```

## 🧪 Configuração para Testes

O projeto detecta automaticamente o ambiente de teste através do método `IsUnitTestEnviroment()`. Em ambiente de teste:

- Migrações de banco de dados não são executadas automaticamente
- Configurações específicas de teste são aplicadas
- Banco de dados em memória pode ser usado

## 📝 Exemplo Completo de appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=MyRecipeBook;User Id=sa;Password=YourPassword;TrustServerCertificate=true"
  },
  "JwtSettings": {
    "SigningKey": "sua-chave-secreta-aqui-minimo-32-caracteres-para-producao",
    "ExpirationTimeMinutes": 60
  },
  "PasswordSettings": {
    "AdditionalKey": "chave-adicional-para-criptografia"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

## 🔄 Migrações

As migrações são executadas automaticamente na inicialização da aplicação (exceto em ambiente de teste).

Para executar migrações manualmente:

```bash
# Usando FluentMigrator CLI
dotnet fm migrate -p sqlserver -c "ConnectionString" -a "MyRecipeBook.Infrastructure.dll"
```

## ✅ Validação de Configuração

A aplicação valida configurações críticas na inicialização:

- Connection string deve estar presente
- JWT Signing Key deve ter pelo menos 32 caracteres
- Password Additional Key deve estar presente

Se alguma configuração estiver ausente ou inválida, a aplicação não iniciará.

## 📚 Referências

- [ASP.NET Core Configuration](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/)
- [Entity Framework Core Connection Strings](https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-strings)
- [JWT Authentication](https://jwt.io/)


