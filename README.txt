========================================================================
PROJETO GOOD HAMBURGER - GUIA DO DESENVOLVEDOR
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
7. DECISÕES DE ARQUITETURA E DESIGN
------------------------------------------------------------------------
O projeto foi estruturado seguindo os princípios da Clean Architecture por 
diversos motivos:
- Separação de Responsabilidades: Garante que a lógica de negócio (Domain) 
  seja independente de detalhes técnicos como UI ou Banco de Dados.
- Testabilidade: A arquitetura permite o uso extensivo de Mocks e Injeção 
  de Dependência, facilitando testes unitários e de integração.
- Escolha da Persistência (JSON): Optou-se pelo uso de arquivos JSON para 
  facilitar a portabilidade do projeto e agilizar o setup de quem for 
  testar, eliminando a necessidade de configurar um banco de dados SQL.
- Result Pattern: Implementado para garantir que a comunicação entre as 
  camadas seja clara, evitando o uso excessivo de exceções para fluxo.

------------------------------------------------------------------------
8. O QUE FICOU DE FORA (BACKLOG / MELHORIAS SUGERIDAS)
------------------------------------------------------------------------
Devido ao escopo e tempo de implementação, os seguintes itens foram 
planejados como melhorias futuras:
- Autenticação JWT: O sistema está preparado para receber segurança, mas 
  atualmente as rotas são abertas para facilitar o teste inicial.
- Banco de Dados Relacional: Futura migração para SQL Server ou PostgreSQL 
  utilizando Entity Framework Core para maior escalabilidade.
- Dockerização: Implementação de Docker e Docker Compose para isolamento 
  completo de ambiente.
- Notificações em Tempo Real: Uso de SignalR para atualizar o status dos 
  pedidos instantaneamente no front-end.
- Logs Estruturados: Implementação de Serilog para monitoramento de erros 
  em produção.

------------------------------------------------------------------------
9. ESTRUTURA DE DADOS
------------------------------------------------------------------------
O sistema utiliza arquivos JSON (`produtos.json` e `pedidos.json`) localizados 
na pasta da WebAPI para persistência simples.

========================================================================
