Participantes :
Caio Freitas - RM553190
Enzzo Monteiro - RM552616 
Lana Andrade -  RM552596


                                                        README — Nexus Career API

A Nexus Career é uma plataforma desenvolvida para conectar pessoas a oportunidades profissionais usando inteligência artificial e análise de dados.
A aplicação recebe currículos, processa competências, experiências e preferências comportamentais dos candidatos, e cruza essas informações com vagas cadastradas por empresas.
A partir dessas análises, o sistema recomenda carreiras adequadas ao perfil do candidato e identifica vagas compatíveis dentro das empresas parceiras.

Do lado das empresas, a API permite cadastrar vagas e encontrar automaticamente os currículos ideais para cada oportunidade, analisando requisitos e competências.

A proposta da Nexus Career é unir dados objetivos — como experiências, formações e competências técnicas — com aspectos subjetivos — como interesses e perfil comportamental —
criando assim um mecanismo inteligente de correspondência entre currículos e vagas, 
servindo tanto como ferramenta de apoio ao candidato quanto solução de recrutamento otimizada para empresas.

   -> Recursos Utilizados

        .NET 8 / ASP.NET Core Web API

        Entity Framework Core

        SQL Server

        LINQ

        Padrão DTO (Data Transfer Object)

        Injeção de Dependência

        Controllers versionados (api/v1)

        Swagger/OpenAPI para documentação automática

   ->Configuração

        Para rodar o projeto:

        Configure a string de conexão no arquivo appsettings.json apontando para o SQL Server:

          "ConnectionStrings": {
            "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=NexusCareerDB;Trusted_Connection=True;"
          },


        Aplique as migrations do Entity Framework:

        dotnet ef database update


        Execute a API:

        dotnet run ou caso esteja executando no Visual Studio, execute pelo botão de "Iniciar Depuração" (F5).


        Acesse o Swagger na rota padrão:

        http://localhost:{porta}/swagger/index.html

  ->Rotas da API

   ->Empresas

    GET /api/v1/empresas — lista todas as empresas

    GET /api/v1/empresas/{nome} — obtém empresa pelo nome

    POST /api/v1/empresas — cria empresa

    PUT /api/v1/empresas/{id} — atualiza empresa

    DELETE /api/v1/empresas/{id} — remove empresa

    GET /api/v1/empresas/{nome}/curriculos-ideais — retorna currículos compatíveis com as vagas da empresa

   -> Vagas

    GET /api/v1/vagas — lista vagas

    GET /api/v1/vagas/{id} — retorna vaga

    POST /api/v1/vagas — cria vaga

    PUT /api/v1/vagas/{id} — edita vaga

    DELETE /api/v1/vagas/{id} — exclui vaga

   ->Currículos

    GET /api/v1/curriculos — lista currículos

    GET /api/v1/curriculos/{nome} — busca currículo por nome

    POST /api/v1/curriculos — cria currículo

    PUT /api/v1/curriculos/{id} — atualiza currículo

    DELETE /api/v1/curriculos/{id} — exclui currículo

   -> Candidatos

    GET /api/v1/candidatos — lista candidatos

    GET /api/v1/candidatos/{id} — busca candidato

    POST /api/v1/candidatos — cria candidato

    PUT /api/v1/candidatos/{id} — atualiza candidato

    DELETE /api/v1/candidatos/{id} — remove candidato

