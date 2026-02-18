# Sistema de Eventos – Compra de Tickets

Um sistema de compra de tickets para eventos, permitindo filtrar ingressos conforme acessibilidade necessária (visual, auditiva, motora etc.).  
A API oferece cadastro de usuários, eventos, ingressos, avaliações e pedidos.

A aplicação foi desenvolvida em **.NET (C#)** utilizando a arquitetura **Controller → Service → Repository** e comunica-se com um banco PostgreSQL.

---

## 🚀 Visão Geral

O objetivo deste projeto é fornecer uma API RESTful para suportar operações básicas relacionadas à compra de tickets e gerenciamento de eventos, tais como:

- Criar, editar e excluir eventos
- Consultar localizações e disponibilidade de ingressos
- Registrar pedidos e avaliações
- Gerenciar usuários
- Filtrar eventos por acessibilidade

---

## 🛠️ Instalação e Dependências

1. **Pré-requisitos**
   - [.NET SDK 10.0](https://dotnet.microsoft.com/download) ou superior
   - PostgreSQL (ou outro provider compatível com EF Core)

2. Clone o repositório:

```bash
git clone <url-do-repo>
cd "Sistema de eventos/SistemaDeEventos"

Restaure os pacotes:

dotnet restore

▶ Como Rodar

Abra um terminal na pasta do projeto e execute:

dotnet run


A aplicação iniciará em:

http://localhost:5000


(ou na porta definida em launchSettings.json).

🔗 Rotas Principais da API

A convenção de rota segue o nome do controller definido em cada classe.
A LocationController utiliza rota no singular (/api/location), enquanto os demais utilizam plural.

Método	Rota	Descrição
GET	/api/events	Lista todos os eventos
GET	/api/events/{id}	Obtém evento por id
POST	/api/events	Cria um novo evento
PUT	/api/events/{id}	Atualiza evento
DELETE	/api/events/{id}	Exclui evento
GET	/api/location	Lista todos os locais
GET	/api/location/{id}	Obtém local por id
POST	/api/location	Cria um novo local
PUT	/api/location/{id}	Atualiza local
DELETE	/api/location/{id}	Exclui local
POST	/api/orders	Registra um pedido
GET	/api/orders/{id}	Consulta pedido por id
GET	/api/tickets	Lista ingressos (opcional filtro query)
GET	/api/tickets/{id}	Obtém ingresso por id
GET	/api/tickets/user/{userId}	Lista ingressos de um usuário
GET	/api/tickets/order/{orderId}	Lista ingressos de um pedido
POST	/api/tickets	Cria ingresso
PUT	/api/tickets/{id}	Atualiza ingresso
DELETE	/api/tickets/{id}	Exclui ingresso
GET	/api/ratings/event/{eventId}	Avaliações de um evento
POST	/api/ratings	Envia uma avaliação
GET	/api/users	Lista todos os usuários
GET	/api/users/{id}	Consulta usuário por id
POST	/api/users	Cria usuário
PUT	/api/users/{id}	Atualiza usuário

Consulte os controllers na pasta Controllers/ para rotas adicionais e detalhes de parâmetros.

📁 Estrutura de Pastas
/Controllers              - Manipuladores HTTP
/Services                 - Lógica de negócio
/Repositories             - Acesso a dados (Entity Framework)
/Models                   - Entidades e contexto do banco
/DTO                      - Objetos de transferência de dados
/Interfaces               - Contratos de serviços e repositórios
/Migrations               - Migrações EF Core
SistemaDeEventos.Tests/   - Testes unitários

🧩 Arquitetura

A API segue o padrão Controller → Service → Repository:

1️⃣ Controller

Recebe requisições HTTP, valida parâmetros e chama os serviços.

2️⃣ Service

Contém regras de negócio e orquestra operações entre repositórios.

3️⃣ Repository

Responsável pelo acesso ao banco através do Entity Framework, abstraindo queries e persistência.

Essa separação facilita manutenção, testes e escalabilidade.

🗄️ Configuração do Banco de Dados
1️⃣ Connection String

Defina em appsettings.json ou appsettings.Development.json:

"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=eventos;Username=usuario;Password=senha"
}

2️⃣ Contextos

EventosContext

PostgresContext

(localizados na pasta Models/)

3️⃣ Migrações

Execute para criar ou atualizar o esquema:

dotnet ef database update

✅ Requisitos Atendidos

Este projeto foi desenvolvido considerando os requisitos mínimos obrigatórios do curso:

🧱 Modelagem de dados

6 tabelas:

Events

Locations

Orders

Ratings

Tickets

Users

🔄 CRUD completo

Implementado para:

Eventos

Locais

Tickets

Usuários

📄 Rota de relatório

GET /api/events/relatorio

Gera um arquivo CSV com todos os eventos cadastrados.

🔗 Consulta com JOIN

O relatório utiliza:

.Include(e => e.Location)


Esse comando realiza JOIN entre Events e Locations, trazendo dados completos para o CSV.

🔀 Relacionamento N:N

Tickets funcionam como tabela associativa entre usuários e eventos.

A entidade Ticket contém EventId, permitindo relacionamento muitos-para-muitos (user ↔ event).

📜 Regra de negócio

Validações aplicadas nos serviços, por exemplo:

Nota de avaliação entre 1 e 5

Validação de pedidos

Verificação de existência antes de atualização

🔎 Filtro com parâmetro

Exemplos:

GET /api/events?accessibility=true
GET /api/ratings/event/{eventId}

🔄 Migração Adicionada

Após atualizar o modelo Ticket:

dotnet ef migrations add AddEventIdToTicket
dotnet ef database update


Essa migração adiciona a coluna event_id na tabela tickets.

⚠️ Tratamento de Erros

A API retorna códigos HTTP padronizados com payload JSON:

Código	Situação	Exemplo
400	Dados inválidos	{ "error": "Dados inválidos" }
401	Não autorizado	{ "error": "Login errado" }
404	Recurso não encontrado	{ "error": "Não encontrado" }
500	Erro interno	{ "error": "Erro interno" }