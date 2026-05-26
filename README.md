# 📦 Encurtador de URL - Monorepo

Um projeto completo de encurtamento de URLs com **Backend** (ASP.NET Core 10.0) e **Frontend** (HTML5/CSS3/JavaScript).

## 🌟 Destaques

- ✅ **Autenticação JWT** - Segurança com tokens JWT
- ✅ **Rate Limiting** - Proteção contra abuso (5 requisições/10 segundos)
- ✅ **CORS habilitado** - Comunicação entre frontend e backend
- ✅ **SQLite** - Banco de dados leve e portável
- ✅ **API RESTful** - Endpoints com autentificação.

## 📂 Estrutura do Projeto

```
EncurtadorUrl/
├── src/                         
│   ├── Auth/                           
│   │   ├── AuthEndpoints.cs           
│   │   ├── AuthService.cs            
│   │   └── UserRequest.cs             
│   ├── Endpoints/
│   │   └── ShortenUrlEndpoint.cs      
│   ├── Models/
│   │   ├── ShortenUrlRequest.cs       
│   │   ├── ShortenedUrl.cs            
│   │   └── User.cs                    
│   ├── Services/
│   │   └── UrlShorteningService.cs    
│   ├── Data/
│   │   └── ApplicationDbContext.cs    
│   ├── Migrations/                     
│   ├── Settings/
│   │   └── ShortLinkSettings.cs       
│   └── Program.cs                     
│
├── frontend/                     
│   ├── index.html                    
│   ├── styles.css                    
│   └── script.js                     
│
├── EncurtadorUrl.csproj              
├── EncurtadorUrl.sln                 
├── appsettings.json                  
├── encurtador.db                     
├── requests.http                     
└── README.md                         
```

## 🚀 Quick Start

### Prerequisites
- **.NET 10.0 SDK** 
- **Python 3.8+** ou **Node.js 14+** - Para servir o frontend (opcional)

### 1️⃣ Backend (Porta 5066)

Na raiz do projeto:

```bash
dotnet run
```

A API estará disponível em: `http://localhost:5066`

**Estrutura de configuração (appsettings.json):**
```json
{
  "Jwt": {
    "Key": "sua-chave-secreta-aqui",
    "Issuer": "http://localhost:5066",
    "Audience": "http://localhost:5066"
  },
  "ConnectionStrings": { 
    "DefaultConnection": "Data Source=encurtador.db" 
  }
}
```

### 2️⃣ Frontend (Porta 8000)

**Opção A: Navegador direto**
```bash
start frontend/index.html
```

**Opção B: Servidor local com Python**
```bash
python -m http.server 8000 --directory frontend
```

**Opção C: Servidor local com Node.js**
```bash
npx http-server frontend -p 8000
```

Acesse: `http://localhost:8000`

---

## 📊 Fluxo de Dados

```
┌────────────────────────────────────────────────────────────────┐
│                    FRONTEND (HTML/CSS/JS)                      │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │  1. Registro/Login                                       │  │
│  │  POST /auth/register ou /auth/login                      │  │
│  │  ├─ Recebe JWT token                                     │  │
│  │  └─ Armazena token no localStorage                       │  │
│  └──────────────────────────────────────────────────────────┘  │
└────────────────────────────────────────────────────────────────┘
                              │ HTTP (JSON)
                              │ CORS habilitado
                              ▼
┌────────────────────────────────────────────────────────────────┐
│                   BACKEND (ASP.NET Core 10)                    │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │  POST /shorten (com JWT token)                           │  │
│  │  ├─ Valida autenticação JWT                              │  │
│  │  ├─ Verifica rate limit (5 req/10s)                      │  │
│  │  ├─ Valida URL                                           │  │
│  │  ├─ Gera código único (7 chars)                          │  │
│  │  ├─ Constrói Short URL                                   │  │
│  │  └─ Salva em ShortenedUrls (SQLite)                      │  │
│  │                                                          │  │
│  │  GET /{code} (sem autenticação)                          │  │
│  │  ├─ Verifica rate limit                                  │  │
│  │  ├─ Busca URL no banco                                   │  │
│  │  └─ Redireciona 302 para URL original                    │  │
│  └──────────────────────────────────────────────────────────┘  │
└────────────────────────────────────────────────────────────────┘
                              │ JSON
                              │ Resposta
                              ▼
┌────────────────────────────────────────────────────────────────┐
│                    FRONTEND (HTML/CSS/JS)                      │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │  2. Input URL longa → POST /shorten com JWT token        │  │
│  │  3. Output URL encurtada                                 │  │
│  │  4. Botão: Copiar → clipboard                            │  │
│  │  5. Botão: Usar → Abre em nova aba                       │  │
│  └──────────────────────────────────────────────────────────┘  │
└────────────────────────────────────────────────────────────────┘
```

---

## 🔄 Geração de Código Único

**Algoritmo:**
1. Gera string aleatória de **7 caracteres**
2. Alfabeto: `ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789`
3. Verifica se já existe no banco de dados
4. Se existe, tenta novamente (até encontrar único)
5. Se não existe, salva na tabela `ShortenedUrls`

**Probabilidade de colisão:** Extremamente baixa com 7 caracteres
- Total de combinações possíveis: **62^7 = 3.6 trilhões**

**Exemplo de códigos gerados:**
```
AbC1dEf
XyZ9aBc
mNoPqRs
K7lMnOp
```

---

## 📋 Tecnologias

### Backend
- **ASP.NET Core 10.0** - Framework web de alta performance
- **Entity Framework Core 10.0.6** - ORM para banco de dados
- **SQLite** - Banco de dados relacional
- **JWT Bearer** - Autenticação com tokens
- **BCrypt.Net** - Hashing de senhas
- **Rate Limiting** - Proteção contra abuso
- **CORS** - Comunicação cross-origin

### Frontend
- **HTML5** - Estrutura semântica
- **CSS3** - Estilos modernos com gradients e animations
- **JavaScript (ES6+)** - Lógica interativa
- **Zero dependências** - Sem frameworks ou bibliotecas

### DevOps
- **.NET CLI** - Build e run
- **Git** - Versionamento
- **SQLite** - Banco portável

---

## 🧪 Testando

### Com REST Client (VS Code)
Abra `requests.http` e clique "Send Request" em cada bloco

Exemplo de workflow:
1. Register um novo usuário
2. Login para obter o token JWT
3. Use o token para criar URLs encurtadas

### Com cURL

**Registrar usuário:**
```bash
curl -X POST http://localhost:5066/auth/register \
  -H "Content-Type: application/json" \
  -d '{"email":"usuario@example.com","password":"senha123"}'
```

**Fazer login:**
```bash
curl -X POST http://localhost:5066/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"usuario@example.com","password":"senha123"}'
```

**Encurtar URL (com token JWT):**
```bash
curl -X POST http://localhost:5066/shorten \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer SEU_TOKEN_AQUI" \
  -d '{"url":"https://github.com"}'
```

**Acessar URL encurtada:**
```bash
curl -L http://localhost:5066/AbC1dEf
```

---

## 🔐 Segurança

### Autenticação JWT
- Token com **expiração** (validado com timestamp)
- Chave simétrica configurável em `appsettings.json`
- Validação de **Issuer** e **Audience**

### Rate Limiting
- **5 requisições** por **10 segundos** por IP
- Fila de espera com até **2 requisições**
- Status `429 Too Many Requests` se excedido

### Proteção
- CORS configurado para aceitar qualquer origem (ajustar em produção)
- Validação de URL (apenas http/https)
- Hashing de senhas com **BCrypt**

---

## 📈 Próximas Features

- [ ] **Dashboard de usuário** - Histórico de URLs encurtadas
- [ ] **Estatísticas** - Contador de cliques por URL
- [ ] **Expiração de URLs** - Links temporários (7 dias, 30 dias, etc)
- [ ] **URLs customizadas** - Permitir slug personalizado
- [ ] **QR Code** - Gerar QR codes para URLs encurtadas
- [ ] **Autenticação OAuth** - Login com Google/GitHub
- [ ] **Docker** - Containerização da aplicação
- [ ] **CI/CD** - Automação de testes e deploy
- [ ] **API Documentation** - Swagger/OpenAPI
- [ ] **Admin Panel** - Gerenciar URLs e usuários
- [ ] **Redis** - Armazenamento em cache de urls mais usadas.
