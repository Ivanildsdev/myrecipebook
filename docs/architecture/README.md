# Arquitetura do MyRecipeBook

Este documento descreve a arquitetura do projeto MyRecipeBook, que segue os princípios da Clean Architecture.

## 📐 Visão Geral da Arquitetura

A aplicação é organizada em camadas bem definidas, cada uma com responsabilidades específicas, seguindo os princípios da Arquitetura Limpa (Clean Archtecture):

```
┌─────────────────────────────────────────────────────────┐
│                    MyRecipeBook.API                      │
│         (Controllers, Middleware, Filters)              │
└────────────────────┬────────────────────────────────────┘
                     │
┌────────────────────▼────────────────────────────────────┐
│              MyRecipeBook.Application                    │
│         (Use Cases, Validators, Services)                │
└────────────────────┬────────────────────────────────────┘
                     │
┌────────────────────▼────────────────────────────────────┐
│                MyRecipeBook.Domain                       │
│      (Entities, Contracts, Domain Services)             │
└────────────────────┬────────────────────────────────────┘
                     │
┌────────────────────▼────────────────────────────────────┐
│            MyRecipeBook.Infrastructure                   │
│    (DataAccess, Security, External Services)             │
└──────────────────────────────────────────────────────────┘
```

## 🏛️ Camadas

### 1. MyRecipeBook.API (Camada de Apresentação)

**Responsabilidades:**

- Receber requisições HTTP
- Validar formato das requisições
- Chamar os Use Cases apropriados
- Retornar respostas HTTP formatadas
- Gerenciar autenticação/autorização via atributos
- Tratamento de erros global

**Componentes Principais:**

- `Controllers/` - Endpoints da API REST
- `Middleware/` - Middleware customizado (ex: CultureMiddleware)
- `Filters/` - Filtros de ação (ExceptionFilter, AuthenticatedUserFilter)
- `Attributes/` - Atributos customizados (AuthenticatedUserAttribute)
- `Token/` - Provedor de token JWT do contexto HTTP

### 2. MyRecipeBook.Application (Camada de Aplicação)

**Responsabilidades:**

- Implementar a lógica de negócio da aplicação
- Orquestrar operações entre diferentes partes do domínio
- Validar regras de negócio
- Mapear entre entidades de domínio e DTOs

**Componentes Principais:**

- `UseCases/` - Casos de uso da aplicação
  - `Login/DoLogin` - Autenticação de usuário
  - `User/Register` - Registro de novo usuário
  - `User/Profile` - Obter perfil do usuário
  - `User/Update` - Atualizar dados do usuário
  - `User/ChangePassword` - Alterar senha
- `SharedValidators/` - Validadores compartilhados
- `Services/` - Serviços de aplicação (ex: AutoMapper)

**Padrão Use Case:**
Cada Use Case segue o padrão:

```csharp
public interface IUseCaseNameUseCase
{
    Task<ResponseType> Execute(RequestType request);
}
```

### 3. MyRecipeBook.Domain (Camada de Domínio)

**Responsabilidades:**

- Definir entidades de domínio
- Definir contratos (interfaces) para repositórios e serviços
- Implementar serviços de domínio puros
- Definir regras de negócio fundamentais

**Componentes Principais:**

- `Entities/` - Entidades de domínio
  - `User` - Entidade de usuário
  - `EntityBase` - Classe base para entidades
- `Contracts/` - Interfaces (contratos)
  - `Repositories/` - Contratos de repositórios
  - `Services/` - Contratos de serviços
- `Security/` - Serviços de segurança
  - `Criptography/` - Contratos de criptografia
  - `Tokens/` - Contratos de geração/validação de tokens
- `Services/` - Serviços de domínio
  - `LoggedUser/` - Serviço para obter usuário logado

### 4. MyRecipeBook.Infrastructure (Camada de Infraestrutura)

**Responsabilidades:**

- Implementar acesso a dados (Entity Framework Core)
- Implementar serviços externos
- Implementar segurança (criptografia, tokens JWT)
- Gerenciar migrações de banco de dados

**Componentes Principais:**

- `DataAccess/` - Acesso a dados
  - `MyRecipeBookDbContext` - Contexto do Entity Framework
  - `Repositories/` - Implementações dos repositórios
- `Security/` - Implementações de segurança
  - `Criptography/` - Implementação de criptografia (SHA512)
  - `Tokens/` - Implementação de tokens JWT
- `Migrations/` - Migrações do FluentMigrator
- `Services/` - Serviços de infraestrutura
  - `LoggedUser/` - Implementação do serviço de usuário logado

## 🔄 Fluxo de Dados

### Exemplo: Registro de Usuário

```
1. Cliente HTTP → POST /user
   ↓
2. UserController.Register()
   ↓
3. IRegisterUserUseCase.Execute()
   ↓
4. RegisterUserValidator.Validate()
   ↓
5. IUserWriteOnlyRepository.Add()
   ↓
6. MyRecipeBookDbContext.SaveChanges()
   ↓
7. ResponseRegisteredUserJson ← Controller
   ↓
8. Cliente HTTP ← 201 Created
```

## 🔐 Segurança

### Autenticação JWT

- Tokens JWT são gerados no login
- Tokens são validados via middleware/atributos
- Token é extraído do header `Authorization: Bearer {token}`

### Criptografia

- Senhas são criptografadas usando SHA512
- Chave adicional configurável via `appsettings.json`

## 📦 Projetos Compartilhados

### MyRecipeBook.Communication

Contém os DTOs (Data Transfer Objects):

- `Requests/` - Objetos de requisição
- `Responses/` - Objetos de resposta

### MyRecipeBook.Exceptions

Contém exceções customizadas:

- Classes base para exceções
- Mensagens de erro localizadas (pt-BR)

## 🗄️ Banco de Dados

- **ORM**: Entity Framework Core
- **Migrações**: FluentMigrator
- **Banco**: SQL Server (configurável)

As migrações são executadas automaticamente na inicialização da aplicação.

## 🔌 Injeção de Dependência

A injeção de dependência é configurada através de métodos de extensão:

- `AddInfrastructure()` - Registra serviços de infraestrutura
- `AddApplication()` - Registra serviços de aplicação

## 📝 Princípios Aplicados

1. **Separation of Concerns** - Cada camada tem responsabilidade única
2. **Dependency Inversion** - Dependências apontam para abstrações
3. **Single Responsibility** - Cada classe tem uma única responsabilidade
4. **Open/Closed** - Aberto para extensão, fechado para modificação
5. **Interface Segregation** - Interfaces específicas e focadas

## 🎯 Benefícios desta Arquitetura

- **Testabilidade**: Fácil de testar cada camada isoladamente
- **Manutenibilidade**: Código organizado e fácil de entender
- **Escalabilidade**: Fácil adicionar novas funcionalidades
- **Flexibilidade**: Fácil trocar implementações (ex: banco de dados)
