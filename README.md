# 📦 Encurtador de URL - Monorepo

Um projeto completo de encurtamento de URLs com **Backend** (ASP.NET Core) e **Frontend** (HTML/CSS/JS).

## 📂 Estrutura do Projeto

```
EncurtadorUrl/
├── src/                         
│   ├── Endpoints/
│   │   └── ShortenUrlEndpoint.cs 
│   ├── Models/
│   │   ├── ShortenUrlRequest.cs  
│   │   └── ShortenedUrl.cs       
│   ├── Services/
│   │   └── UrlShorteningService.cs 
│   ├── Data/
│   │   └── ApplicationDbContext.cs 
│   ├── Settings/
│   │   └── ShortLinkSettings.cs  
│   └── Program.cs                
├── frontend/                     
│   ├── index.html               
│   ├── styles.css               
│   ├── script.js                
│   └── README.md                
│
├── EncurtadorUrl.csproj         
├── EncurtadorUrl.sln            
├── appsettings.json             
├── encurtador.db                
├── requests.http                
└── README.md                    
```

## 🚀 Quick Start

### 1️⃣ Backend (Porta 5066)

Na raiz do projeto execute:
```bash
dotnet run
```

A API estará disponível em: `http://localhost:5066`

### 2️⃣ Frontend (Porta 8000)

**Opção A: Navegador direto**
```bash
start frontend/index.html
```

**Opção B: Servidor local (recomendado)**
```bash
# Com Python 3
python -m http.server 8000 --directory frontend

# Com Node.js
npx http-server frontend -p 8000
```

Acesse: `http://localhost:8000`

---

## 📊 Fluxo de Dados

```
┌────────────────────────────────────────────────────────────────┐
│                     FRONTEND (HTML/CSS/JS)                     │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │  Input: URL longa                                        │  │
│  │  Button: Encurtar → POST /shorten                        │  │
│  └──────────────────────────────────────────────────────────┘  │
└──────────────────────────────────────────────────────────────────┘
                              │ HTTP (JSON)
                              │ CORS habilitado
                              ▼
┌────────────────────────────────────────────────────────────────┐
│                   BACKEND (ASP.NET Core)                       │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │  POST /shorten                                           │  │
│  │  ├─ Valida URL                                           │  │
│  │  ├─ Gera código único (7 chars)                          │  │
│  │  ├─ Constrói Short URL                                   │  │
│  │  └─ Salva em ShortenedUrls (SQLite)                      │  │
│  │                                                          │  │
│  │  GET /{code}                                             │  │
│  │  ├─ Busca URL no banco                                   │  │
│  │  └─ Redireciona 302 para URL original                    │  │
│  └──────────────────────────────────────────────────────────┘  │
└────────────────────────────────────────────────────────────────┘
                              │ JSON
                              │ Resposta
                              ▼
┌────────────────────────────────────────────────────────────────┐
│                     FRONTEND (HTML/CSS/JS)                     │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │  Output: URL encurtada                                   │  │
│  │  Button: Copiar → clipboard                              │  │
│  └──────────────────────────────────────────────────────────┘  │
└────────────────────────────────────────────────────────────────┘
```

---

## 🔌 Endpoints da API

### POST `/shorten`
Encurta uma URL

**Request:**
```json
{
  "url": "https://www.google.com/search?q=exemplo"
}
```

**Response (200 OK):**
```json
"http://localhost:5066/AbC1dEf"
```

**Errors:**
- `400 Bad Request`: URL inválida

---

### GET `/{code}`
Redireciona para a URL original

**Request:**
```
GET /AbC1dEf
```

**Response:**
```
302 Found
Location: https://www.google.com/search?q=exemplo
```

**Errors:**
- `404 Not Found`: Código não existe

---

## 🔄 Geração de Código Único

**Algoritmo:**
1. Gera string aleatória de 7 caracteres
2. Alfabeto: `ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789`
3. Verifica se já existe no banco
4. Se sim, tenta novamente
5. Se não, salva

**Exemplo de códigos:**
```
AbC1dEf
XyZ9aBc
mNoPqRs
```

---

## 📋 Tecnologias

### Backend
- ASP.NET Core 10.0
- Entity Framework Core 10.0.6
- SQLite
- CORS habilitado
- Rate Limit

### Frontend
- HTML5 
- CSS3 
- JavaScript (ES6+)
- Zero dependências

---

## 🧪 Testando

### Com REST Client (VS Code)
Abra `requests.http` e clique "Send Request" em cada bloco

### Com cURL
```bash
# Encurtar URL
curl -X POST http://localhost:5066/shorten \
  -H "Content-Type: application/json" \
  -d '{"url":"https://github.com"}'

# Acessar URL encurtada
curl -L http://localhost:5066/AbC1dEf
```
---

## 📈 Próximas Features

- [ ] Histórico de URLs encurtadas
- [ ] Load balance
- [ ] Analytics de cliques

---

## 📄 Licença

Projeto educacional | Livre para usar e modificar