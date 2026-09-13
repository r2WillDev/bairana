# BAIRANA

<p align="center">
  <strong>Vitrine digital mobile-first para descobrir produtos e serviços oferecidos por moradores de condomínios.</strong>
</p>

<p align="center">
  <img src="https://img.shields.io/badge/Status-Em%20desenvolvimento-1f6feb?style=flat-square" alt="Status: Em desenvolvimento">
  <img src="https://img.shields.io/badge/C%23-512BD4?style=flat-square&logo=csharp&logoColor=white" alt="C#">
  <img src="https://img.shields.io/badge/.NET%2010-512BD4?style=flat-square&logo=dotnet&logoColor=white" alt=".NET 10">
  <img src="https://img.shields.io/badge/Blazor-WebAssembly-512BD4?style=flat-square&logo=blazor&logoColor=white" alt="Blazor WebAssembly">
  <img src="https://img.shields.io/badge/Testes-xUnit-512BD4?style=flat-square" alt="Testes: xUnit">
</p>

## Visão geral

Produtos e serviços oferecidos por moradores podem ficar dispersos em grupos e conversas do condomínio, dificultando encontrá-los quando surge uma necessidade.

O **BAIRANA** centraliza essa descoberta em uma vitrine digital, permitindo localizar negócios, consultar informações básicas e iniciar o contato diretamente pelo WhatsApp.[^1]

O produto tem um objetivo simples: **descoberta + contato**. Negociação, pagamento e entrega não são intermediados pela aplicação.

## Fluxo principal

> **Morador** → **BAIRANA** → pesquisa ou filtra → encontra um negócio → consulta informações → **WhatsApp**[^1]

## Escopo do MVP

O MVP está sendo desenvolvido para permitir:

* descobrir produtos e serviços;
* pesquisar negócios;
* filtrar negócios;
* consultar informações básicas;
* iniciar contato diretamente pelo WhatsApp.

O BAIRANA é uma vitrine digital, não um marketplace.

## Tecnologias

O projeto utiliza o SDK `.NET 10.0.401`, fixado em `global.json`, e target framework `net10.0`.

| Tecnologia                    | Função no projeto                    |
| ----------------------------- | ------------------------------------ |
| C#                            | Linguagem de desenvolvimento         |
| .NET 10                       | Plataforma                           |
| Blazor WebAssembly Standalone | Aplicação web executada no navegador |
| xUnit                         | Testes automatizados                 |

## Estrutura do repositório

```text
bairana/
├── src/
│   └── Bairana.Web/
├── tests/
│   └── Bairana.Tests/
├── docs/
├── .editorconfig
├── .gitignore
├── global.json
├── README.md
├── SECURITY.md
└── Bairana.slnx
```

`src/Bairana.Web/` contém a aplicação Blazor WebAssembly e `tests/Bairana.Tests/` contém o projeto de testes.

## Pré-requisitos

Para utilizar a configuração atualmente validada do projeto, tenha instalado:

* .NET SDK `10.0.401`.

A versão utilizada pelo repositório é definida em `global.json`.

## Desenvolvimento local

Execute os comandos a partir da raiz do repositório.

### 1. Restaurar as dependências

```powershell
dotnet restore .\Bairana.slnx --locked-mode
```

### 2. Compilar em Release

```powershell
dotnet build .\Bairana.slnx -c Release --no-restore
```

### 3. Executar os testes

```powershell
dotnet test .\Bairana.slnx -c Release --no-build --no-restore
```

### 4. Executar a aplicação

```powershell
dotnet run --project .\src\Bairana.Web\Bairana.Web.csproj -c Release --no-build --no-restore
```

Após iniciar a aplicação, abra no navegador a URL exibida pelo terminal.

## Status do projeto

**Em desenvolvimento.**

A fundação com .NET 10, Blazor WebAssembly Standalone e xUnit já está estabelecida e validada localmente, incluindo restore, build, testes e execução da aplicação base.

As funcionalidades do MVP ainda serão implementadas incrementalmente.

## Dados e privacidade

> [!WARNING]
> Este é um repositório público. Use somente dados fictícios ou adequados para demonstração. Não devem ser publicadas informações pessoais indevidas, credenciais, secrets ou dados reais de empreendedores sem tratamento e autorização apropriados.

## Segurança

Orientações relacionadas à segurança do projeto estão disponíveis em [SECURITY.md](./SECURITY.md).

[^1]: O BAIRANA apenas inicia o contato externo pelo WhatsApp. Conversa, orçamento, negociação, pagamento e entrega acontecem fora da plataforma.
