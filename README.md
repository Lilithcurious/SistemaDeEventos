# Sistema de Eventos – Compra de Tickets

Um sistema de compras de tickets para eventos, permitindo filtrar ingressos pela acessibilidade necessária. Ele oferece cadastro de usuários, eventos, ingressos, avaliações e pedidos.  
A API foi desenvolvida em .NET (C#) utilizando a abordagem **Controller → Service → Repository** e comunica‑se com um banco PostgreSQL.

---

## 🚀 Visão Geral

O objetivo deste projeto é fornecer uma API RESTful para suportar operações básicas da compra de tickets, tais como:

- Criar/editar/excluir eventos
- Consultar localizações e disponibilidades de ingressos
- Registrar pedidos e avaliações
- Gerenciar usuários

---

## 🛠️ Instalação e Dependências

1. **Pré‑requisitos**  
   - [.NET SDK 10.0](https://dotnet.microsoft.com/download) ou superior  
   - PostgreSQL (ou outro provider suportado)
2. Clone o repositório:

   ```bash
   git clone <url-do-repo>
   cd "Sistema de eventos/SistemaDeEventos"
   ```

3. Restaure pacotes:

   ```bash
   dotnet restore
   ```

---

## ▶ Como Rodar

Abra um terminal na pasta do projeto e execute:

```bash
dotnet run
```

A aplicação iniciará em `http://localhost:5000` (ou porta configurada em `launchSettings.json`).

---

## 🔗 Rotas Principais da API

A convenção de rota segue o nome do controller definido em cada classe. Note que a `LocationController` usa rota no singular (`/api/location`), enquanto os demais são plurais.

| Método | Rota                          | Descrição                               |
|--------|-------------------------------|-----------------------------------------|
| GET    | `/api/events`                 | Lista todos os eventos                  |
| GET    | `/api/events/{id}`            | Obtém evento por id                     |
| POST   | `/api/events`                 | Cria um novo evento                     |
| PUT    | `/api/events/{id}`            | Atualiza evento                         |
| DELETE | `/api/events/{id}`            | Exclui evento                           |
| GET    | `/api/location`              | Lista todos os locais (singular)        |
| GET    | `/api/location/{id}`         | Obtém local por id                      |
| POST   | `/api/location`              | Cria um novo local                      |
| PUT    | `/api/location/{id}`         | Atualiza local                          |
| DELETE | `/api/location/{id}`         | Exclui local                            |
| POST   | `/api/orders`                 | Registra um pedido                      |
| GET    | `/api/orders/{id}`            | Consulta pedido por id                  |
| GET    | `/api/tickets`               | Lista ingressos (opcional filtro query) |
| GET    | `/api/tickets/{id}`          | Obtém ingresso por id                   |
| GET    | `/api/tickets/user/{userId}`  | Ingressos de um usuário                 |
| GET    | `/api/tickets/order/{orderId}`| Ingressos de um pedido                  |
| POST   | `/api/tickets`               | Cria ingresso                           |
| PUT    | `/api/tickets/{id}`          | Atualiza ingresso                       |
| DELETE | `/api/tickets/{id}`          | Exclui ingresso                         |
| GET    | `/api/ratings/event/{eventId}`| Avaliações de um evento                 |
| POST   | `/api/ratings`                | Envia uma avaliação                     |
| GET    | `/api/users`                 | Lista todos os usuários                 |
| GET    | `/api/users/{id}`            | Consulta usuário por id                 |
| POST   | `/api/users`                 | Cria usuário                            |
| PUT    | `/api/users/{id}`            | Atualiza usuário                        |

> Consulte os controllers na pasta `Controllers/` para outras rotas e detalhes.
> Consulte os controllers na pasta `Controllers/` para rotas adicionais.

---

## 📁 Estrutura de Pastas

```
/Controllers     - manipuladores HTTP
/Services        - lógica de negócio
/Repositories    - acesso a dados (Entity Framework)
/Models          - entidades do EF e contexto
/DTO             - objetos de transferência
/Interfaces      - contratos de serviço/repos
/Migrations      - migrações EF Core
/Repositories    - implementação dos repositórios
/Services        - implementação dos serviços
SistemaDeEventos.Tests/ - testes unitários
```

---

## 🧩 Arquitetura

A API segue o padrão **Controller‑Service‑Repository**:

1. **Controller**  
   Recebe a requisição, valida parâmetros e chama serviços.

2. **Service**  
   Contém a lógica de negócio e orquestra operações entre repositórios e outros serviços.

3. **Repository**  
   Responsável pelo acesso ao banco (via Entity Framework). Abstrai queries e persistência.

Essa separação facilita manutenção, teste e reuse.

---

## 🗄️ Configuração do Banco de Dados

1. **Connection string**  
   Defina em `appsettings.json` ou `appsettings.Development.json`:

   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Host=localhost;Port=5432;Database=hamburgueria;Username=usuario;Password=senha"
   }
   ```

2. **Contextos**  
   - `EventosContext` e `PostgresContext` no diretório `Models/`.

3. **Migrations**  
   Execute para criar/atualizar esquema:

   ```bash
   dotnet ef database update
   ```

4. **Modifique as configurações** conforme necessário (provider diferente, etc).

---

## ✅ Pronto para uso

Copie este arquivo para o root do repositório como `README.md` e ajuste URLs/strings de conexão conforme seu ambiente. Aproveite o desenvolvimento!

💡 **Dica**: para testes, abra `SistemaDeEventos.Tests/UnitTest1.cs` e adicione novas verificações para serviços e controladores.