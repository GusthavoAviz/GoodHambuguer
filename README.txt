========================================================================
🍔 PROJETO GOOD HAMBURGER - GUIA DO DESENVOLVEDOR 🍔
========================================================================

Este documento fornece as instruções necessárias para compilar, testar e 
entender o funcionamento do sistema Good Hamburger.

------------------------------------------------------------------------
1. VISÃO GERAL DO SISTEMA
------------------------------------------------------------------------
O Good Hamburger é um sistema de gerenciamento de lanchonete desenvolvido 
com foco em Clean Architecture, utilizando .NET 8. O sistema permite o 
gerenciamento de produtos, controle de pedidos e monitoramento financeiro.

Arquitetura:
- Domain: Entidades puras, Enums e Regras de Negócio.
- Application: Casos de uso, DTOs, Validações (FluentValidation) e Interfaces.
- Infrastructure: Repositórios e Persistência de dados (JSON).
- WebAPI: Endpoints RESTful e Injeção de Dependência.
- Blazor: Interface de usuário moderna e responsiva.

------------------------------------------------------------------------
2. TECNOLOGIAS E FRAMEWORKS (FRONT-END)
------------------------------------------------------------------------
A interface do usuário foi construída utilizando:
- Blazor WebAssembly: Framework SPA (Single Page Application) da Microsoft 
  que permite C# no navegador.
- Bootstrap 5.3: Framework CSS para design responsivo e componentes UI.
- Bootstrap Icons: Biblioteca de ícones vetoriais.
- Inter Font: Tipografia moderna via Google Fonts para melhor legibilidade.
- Glassmorphism & Custom CSS: Estilização personalizada para temas claro/escuro.

------------------------------------------------------------------------
3. PRÉ-REQUISITOS
------------------------------------------------------------------------
- .NET 8 SDK instalado.
- Visual Studio 2022, VS Code ou JetBrains Rider.
- Navegador moderno (Chrome, Edge ou Firefox).

------------------------------------------------------------------------
4. COMO EXECUTAR O PROJETO (VIA CMD)
------------------------------------------------------------------------
Para rodar o projeto completo, você deve abrir dois terminais:

Terminal 1 (Backend - API):
   cd src/GoodHamburger.WebAPI
   dotnet run

Terminal 2 (Frontend - Blazor):
   cd src/GoodHamburger.Blazor
   dotnet run

URLs Padrão:
- API: http://localhost:5000 (ou porta definida em launchSettings.json)
- Blazor: http://localhost:5100 (ou porta definida em launchSettings.json)
- Swagger (Documentação API): http://localhost:5000/swagger

------------------------------------------------------------------------
5. COMO EXECUTAR OS TESTES
------------------------------------------------------------------------
O projeto utiliza xUnit e Moq para garantir a qualidade do código.
Para rodar todos os testes (Unitários e Integração):

   dotnet test

------------------------------------------------------------------------
6. FUNCIONALIDADES PRINCIPAIS
------------------------------------------------------------------------
- Gerenciamento de Produtos: Cadastro, Edição, Exclusão e Listagem.
- Controle de Disponibilidade: Ativação/Desativação rápida de itens.
- Menu Interativo: Cardápio organizado por categorias (Lanches, Bebidas).
- Painel Financeiro: Visualização de vendas e totais.
- Suporte a Temas: Alternância entre Modo Claro, Escuro e Automático.

------------------------------------------------------------------------
7. REGRAS DE NEGÓCIO E VALIDAÇÕES
------------------------------------------------------------------------
- Produtos:
  * O preço deve ser sempre positivo e maior que zero.
  * O nome é obrigatório e não pode ser vazio.
  * Itens inativos não aparecem no cardápio de vendas.
- Pedidos:
  * Cálculo automático de subtotal e total final na camada Application.
  * Validação de integridade para evitar produtos duplicados em um mesmo combo.
- Arquitetura:
  * Uso obrigatório de Injeção de Dependência.
  * Implementação de Result Pattern para retornos consistentes da API.

------------------------------------------------------------------------
8. ESTRUTURA DE DADOS
------------------------------------------------------------------------
O sistema utiliza arquivos JSON (`produtos.json` e `pedidos.json`) localizados 
na pasta da WebAPI para persistência simples, dispensando a configuração 
inicial de um banco de dados SQL para testes rápidos.

------------------------------------------------------------------------
9. MELHORIAS SUGERIDAS (BACKLOG)
------------------------------------------------------------------------
- Autenticação e Autorização (JWT): Implementar um sistema de login para 
  proteger as rotas de gerenciamento de produtos e financeiro.
- Identity Integration: Uso de tokens JWT assinados para comunicação segura 
  entre Blazor e WebAPI.
- Banco de Dados SQL: Transição do armazenamento JSON para SQL Server ou 
  PostgreSQL utilizando Entity Framework Core.
- Dockerização: Criação de arquivos Dockerfile e Docker Compose para 
  facilitar o deployment em containers.

------------------------------------------------------------------------
Bom apetite e bom desenvolvimento! 🍔🚀
========================================================================
