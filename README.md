# 🏆 Sistema Acadêmico PIM — Zero Frame

> Projeto vencedor do **1º lugar na Batalha do PIM da UNIP** — primeiro semestre de 2026.

A **Zero Frame** é um projeto completo de e-commerce desenvolvido como Projeto Integrado Multidisciplinar (PIM) da UNIP, abrangendo desde o planejamento e modelagem do sistema até o desenvolvimento de uma plataforma funcional com frontend, API REST e banco de dados.

---

## 📌 Sobre o Projeto

A Zero Frame é uma plataforma de e-commerce voltada para venda de roupas, calçados e acessórios originais e multimarcas. O projeto foi desenvolvido por uma equipe multidisciplinar e contempla três frentes principais: **Frontend**, **API REST** e **Engenharia de Software**.

A Batalha do PIM foi uma competição entre unidades da UNIP onde os melhores projetos competiam entre si. Nosso trabalho foi reconhecido com o **primeiro lugar**, resultado do esforço, dedicação e aprendizado de toda a equipe ao longo do semestre.

---

## 🗂️ Estrutura do Repositório

Este repositório reúne as partes do projeto:

```
sistema-acad-mico-pim-3/
├── FRONTEND/       # Interface web da plataforma
└── API/            # Backend em ASP.NET Core
```

---

## 🎨 Frontend — Zero Frame

Interface web moderna desenvolvida com HTML, CSS e JavaScript, sem dependência de frameworks pesados.

### Funcionalidades

- Navegação intuitiva com menu para Originais, Multimarcas e Sobre Nós
- Sistema de busca com filtros por categoria, preço e marca
- Carrinho de compras com gerenciamento completo
- Perfil do usuário com menu suspenso
- Alternância entre modo claro e escuro com persistência
- Carrossel 3D de categorias em React
- Slider de imagens de produtos
- Páginas de login e registro com validação
- Gestão de endereços e histórico de pedidos
- Página de suporte com formulário de contato
- Design responsivo para diferentes tamanhos de tela

### Tecnologias

- HTML5, CSS3, JavaScript Vanilla
- React 18 + Babel Standalone (carrossel de categorias)
- Font Awesome 7.0.1 e Google Fonts (Montserrat)
- CSS Custom Properties para sistema de temas

### Como executar o Frontend

1. Clone ou baixe o repositório
2. Abra o arquivo `FRONTEND/index.html` em um navegador moderno
3. Não requer servidor — funciona localmente no navegador

---

## ⚙️ API — Zero Frame Backend

API REST desenvolvida em ASP.NET Core com arquitetura em camadas, responsável por toda a lógica de negócio e comunicação com o banco de dados.

### Arquitetura

| Camada | Responsabilidade |
|--------|-----------------|
| `ZeroFrame.API` | Controladores, rotas, Swagger, autenticação e middleware |
| `ZeroFrame.Application` | Serviços, DTOs e regras de aplicação |
| `ZeroFrame.Domain` | Entidades e interfaces |
| `ZeroFrame.Infra.Data` | Acesso ao banco de dados via Entity Framework Core |
| `ZeroFrame.Infra.IoC` | Injeção de dependência |

### Funcionalidades

- **Usuários** — Cadastro, login com JWT, atualização e remoção
- **Endereços** — Cadastro, listagem, atualização e remoção
- **Categorias** — CRUD completo
- **Produtos** — CRUD completo com busca por ID
- **Variações** — Controle de tamanho, cor e estoque por produto
- **Carrinho** — Criação, adição de itens, validação de estoque
- **Pedidos** — Criação via carrinho ou lista, cálculo de total e cancelamento com reposição de estoque
- **Pagamentos** — Registro de status de pagamento por pedido

### Tecnologias

- ASP.NET Core, C#
- Entity Framework Core
- SQL Server
- JWT Bearer (autenticação)
- Swagger (documentação interativa)

### Como executar a API

1. Configure a string de conexão com o banco de dados
2. Execute as migrations com `dotnet ef database update`
3. Rode o projeto com `dotnet run`
4. Acesse a documentação em `http://localhost:5140/swagger`

### Usar a mesma API no desktop e no mobile

O site e o app Expo usam a mesma API ASP.NET Core e a mesma base SQL Server.
Quem já tem uma conta usa o mesmo e-mail e senha no app. O catálogo, os
endereços, o carrinho, os pedidos e os favoritos são associados ao usuário
da API. O app recarrega o carrinho ao abrir a tela e quando volta ao primeiro
plano.

1. Identifique a conexão `ConnectionStrings__DefaultConnection` usada pela
   instalação atual do desktop. Use essa mesma base para manter as contas e
   os pedidos existentes.
2. Faça um backup antes de aplicar as migrations ao banco em uso. Com a
   conexão configurada, execute da raiz deste repositório:

   ```powershell
   dotnet ef database update --project API/ZeroFrame.Infra.Data --startup-project API/ZeroFrame.API
   ```

3. No app Expo, configure `EXPO_PUBLIC_API_URL` com a URL dessa API seguida
   de `/api`. No aparelho físico, use o IP da máquina na rede local durante
   o teste ou a URL HTTPS do ambiente publicado. O desktop continua usando
   a URL base da API sem o sufixo `/api`.
4. Entre nos dois clientes com a mesma conta. Adicione no site um produto com
   tamanho e cor. Abra o carrinho do app e confira o item. Adicione uma
   segunda variação no app e atualize o carrinho no site. Confira também um
   pedido já existente em **Meus pedidos**.

Os favoritos antigos do site estavam no navegador. Na página **Favoritos**,
use **Importar favoritos deste navegador** na conta correta para salvar
esses itens no servidor. O carrinho e o histórico de pedidos já usam o banco
do desktop e aparecem no app assim que ele estiver conectado à mesma base.

O app usa o fluxo de pedido do desktop, que registra um pedido pendente e
usa o preço, o estoque, o frete e o endereço cadastrados na API.

---

## 🧩 Minha Contribuição — Engenharia de Software

Dentro da equipe, atuei principalmente na área de **Engenharia de Software**, sendo responsável por:

- Organização e estruturação da solução como um todo
- Modelagem do sistema e definição da arquitetura
- Definição de processos e fluxos de desenvolvimento
- Apoio no desenvolvimento da aplicação junto ao time
- Integração entre as partes do projeto (Frontend + API)

Essa experiência me permitiu aprender muito sobre desenvolvimento na prática, trabalho em equipe e a importância de uma boa estrutura por trás de um projeto.

---

## 🏅 Reconhecimento

🥇 **1º lugar — Batalha do PIM, UNIP (2026)**

A Batalha do PIM foi uma dinâmica entre unidades da UNIP onde os melhores Projetos Integrados Multidisciplinares competiam entre si. Ter nosso trabalho reconhecido dessa forma tornou toda a jornada ainda mais especial.

---

## 📬 Contato

Para dúvidas ou sugestões: gutavosouzadevv@gmail.com

---

**Versão:** 1.0.0 | **Status:** ✅ Completo
