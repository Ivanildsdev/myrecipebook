# Documentação da API

Esta documentação descreve os endpoints disponíveis na API do MyRecipeBook.

## 🔐 Autenticação

A API utiliza autenticação JWT (JSON Web Token). Para acessar endpoints protegidos, é necessário incluir o token no header da requisição:

```
Authorization: Bearer {seu_token_jwt}
```

O token é obtido através do endpoint de login.

## 📍 Base URL

```
https://localhost:5001
```

Em desenvolvimento, o Swagger está disponível em:
```
https://localhost:5001/swagger
```

## 🌐 Endpoints

### Autenticação

#### POST /login

Realiza login e retorna token JWT.

**Request:**
```json
{
  "email": "usuario@example.com",
  "password": "senha123"
}
```

**Response (200 OK):**
```json
{
  "name": "Nome do Usuário",
  "email": "usuario@example.com",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

**Response (401 Unauthorized):**
```json
{
  "errors": [
    "Credenciais inválidas"
  ]
}
```

---

### Usuário

#### POST /user

Registra um novo usuário no sistema.

**Request:**
```json
{
  "name": "Nome do Usuário",
  "email": "usuario@example.com",
  "password": "senha123"
}
```

**Validações:**
- `name`: Obrigatório, mínimo 3 caracteres
- `email`: Obrigatório, formato de email válido
- `password`: Obrigatório, mínimo 6 caracteres

**Response (201 Created):**
```json
{
  "name": "Nome do Usuário",
  "email": "usuario@example.com",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

**Response (400 Bad Request):**
```json
{
  "errors": [
    "O campo nome é obrigatório",
    "O email informado já está em uso"
  ]
}
```

---

#### GET /user

Obtém o perfil do usuário autenticado.

**Headers:**
```
Authorization: Bearer {token}
```

**Response (200 OK):**
```json
{
  "name": "Nome do Usuário",
  "email": "usuario@example.com"
}
```

**Response (401 Unauthorized):**
```json
{
  "errors": [
    "Token inválido ou expirado"
  ]
}
```

---

#### PUT /user

Atualiza os dados do usuário autenticado.

**Headers:**
```
Authorization: Bearer {token}
```

**Request:**
```json
{
  "name": "Novo Nome"
}
```

**Validações:**
- `name`: Obrigatório, mínimo 3 caracteres

**Response (204 No Content):**
```
(sem conteúdo)
```

**Response (400 Bad Request):**
```json
{
  "errors": [
    "O campo nome é obrigatório"
  ]
}
```

---

#### PUT /user/change-password

Altera a senha do usuário autenticado.

**Headers:**
```
Authorization: Bearer {token}
```

**Request:**
```json
{
  "currentPassword": "senhaAtual123",
  "newPassword": "novaSenha456"
}
```

**Validações:**
- `currentPassword`: Obrigatório
- `newPassword`: Obrigatório, mínimo 6 caracteres

**Response (204 No Content):**
```
(sem conteúdo)
```

**Response (400 Bad Request):**
```json
{
  "errors": [
    "Senha atual incorreta"
  ]
}
```

---

## 📊 Códigos de Status HTTP

| Código | Descrição |
|--------|-----------|
| 200 | OK - Requisição bem-sucedida |
| 201 | Created - Recurso criado com sucesso |
| 204 | No Content - Operação bem-sucedida sem conteúdo |
| 400 | Bad Request - Erro de validação ou requisição inválida |
| 401 | Unauthorized - Token inválido ou ausente |
| 404 | Not Found - Recurso não encontrado |
| 500 | Internal Server Error - Erro interno do servidor |

## 🔄 Formato de Resposta de Erro

Todos os erros seguem o formato padrão:

```json
{
  "errors": [
    "Mensagem de erro 1",
    "Mensagem de erro 2"
  ]
}
```

## 🌍 Internacionalização

A API suporta múltiplos idiomas através do header `Accept-Language`:

```
Accept-Language: pt-BR
Accept-Language: en-US
```

As mensagens de erro são retornadas no idioma solicitado.

## 📝 Exemplos de Uso

### cURL

**Login:**
```bash
curl -X POST https://localhost:5001/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "usuario@example.com",
    "password": "senha123"
  }'
```

**Obter Perfil:**
```bash
curl -X GET https://localhost:5001/user \
  -H "Authorization: Bearer {seu_token}"
```

**Atualizar Usuário:**
```bash
curl -X PUT https://localhost:5001/user \
  -H "Authorization: Bearer {seu_token}" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Novo Nome"
  }'
```

### JavaScript (Fetch)

```javascript
// Login
const loginResponse = await fetch('https://localhost:5001/login', {
  method: 'POST',
  headers: {
    'Content-Type': 'application/json'
  },
  body: JSON.stringify({
    email: 'usuario@example.com',
    password: 'senha123'
  })
});

const { token } = await loginResponse.json();

// Obter Perfil
const profileResponse = await fetch('https://localhost:5001/user', {
  headers: {
    'Authorization': `Bearer ${token}`
  }
});

const profile = await profileResponse.json();
```

## 🔒 Segurança

- Todas as senhas são criptografadas antes de serem armazenadas
- Tokens JWT têm tempo de expiração configurável
- Endpoints protegidos validam o token em cada requisição
- URLs são convertidas para lowercase automaticamente

## 📚 Swagger/OpenAPI

Em ambiente de desenvolvimento, a documentação interativa está disponível no Swagger UI. Acesse `/swagger` após iniciar a aplicação.


