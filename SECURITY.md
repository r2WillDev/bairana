# Política de Segurança

![Security Policy](https://img.shields.io/badge/security-policy-0969da)
![Public Repository](https://img.shields.io/badge/repository-public-2ea44f)
![Security Reporting](https://img.shields.io/badge/reporting-private%20channel%20pending-d29922)

O BAIRANA é um projeto público em estágio inicial. Esta política define práticas essenciais para proteger o repositório, credenciais, dados e futuros usuários, mantendo controles de segurança proporcionais ao MVP.

Como princípio geral, todo conteúdo versionado neste repositório deve ser considerado **potencialmente público e replicável**.

## Relato de vulnerabilidades

Vulnerabilidades que possam expor dados, credenciais, configurações sensíveis ou permitir exploração do sistema devem ser tratadas de forma responsável.

> [!IMPORTANT]
> **Não publique vulnerabilidades sensíveis, provas de conceito, detalhes de exploração, credenciais ou informações que facilitem um ataque em Issues públicas.**

Ao relatar uma vulnerabilidade, evite divulgar publicamente informações como:

- passos detalhados para exploração;
- tokens, chaves ou credenciais;
- dados pessoais;
- configurações internas sensíveis;
- exemplos que permitam reproduzir um ataque contra ambientes reais.

### Canal de segurança

O BAIRANA ainda não possui um canal privado oficial para recebimento de relatos de segurança.

> [!NOTE]
> Um mecanismo privado de comunicação será definido antes do piloto público do BAIRANA. Até lá, detalhes sensíveis de vulnerabilidades não devem ser divulgados publicamente.

Esta seção será atualizada quando um canal apropriado estiver disponível.

## Credenciais e segredos

**Credenciais e secrets nunca devem ser versionados**, mesmo que sejam destinados apenas a desenvolvimento, testes, CI/CD ou infraestrutura.

Não adicione ao repositório:

- senhas;
- tokens de acesso;
- API keys;
- access keys;
- client secrets;
- connection strings contendo credenciais;
- chaves privadas;
- certificados privados;
- credenciais de serviços;
- arquivos `.env` ou `.env.local` contendo valores reais;
- arquivos `appsettings.*` contendo secrets reais;
- qualquer outro segredo utilizado localmente, em automações, CI/CD ou infraestrutura.

Arquivos de configuração podem ser versionados quando necessário, desde que utilizem **valores fictícios, placeholders ou referências seguras**, e não credenciais reais.

> [!WARNING]
> Se uma credencial for publicada, considere-a **comprometida**, mesmo que o commit seja corrigido imediatamente.[^1]

### Em caso de exposição de uma credencial

Se uma credencial for adicionada acidentalmente ao repositório:

1. **Considere a credencial comprometida.**
2. **Revogue, invalide ou rotacione** a credencial no serviço responsável.
3. Gere uma nova credencial quando necessário e configure-a por um meio adequado, sem versioná-la.
4. Avalie o histórico do Git e outras possíveis cópias da informação exposta.
5. Verifique se a credencial comprometida foi utilizada indevidamente, quando o serviço disponibilizar meios apropriados para essa verificação.

**Apagar a linha, remover o arquivo ou criar um novo commit não desfaz o comprometimento.** A rotação ou revogação da credencial continua sendo necessária.

## Dados pessoais e dados reais

O repositório público do BAIRANA não deve armazenar dados pessoais, documentos privados ou informações reais de empreendedores sem tratamento e autorização adequados.

Isso inclui, entre outros:

- telefone e WhatsApp;
- endereço;
- apartamento ou bloco;
- CPF;
- documentos pessoais;
- fotografias pessoais;
- dados privados;
- contratos;
- consentimentos;
- comprovantes;
- informações comerciais não autorizadas;
- qualquer informação que não tenha sido destinada à publicação.

> [!CAUTION]
> Antes de adicionar qualquer dado real ao repositório, confirme que ele pode ser publicado e que existe autorização adequada para esse uso.

Dados necessários ao funcionamento futuro da aplicação devem ser avaliados separadamente daquilo que pode permanecer em um **repositório Git público**.

## Exemplos e dados de desenvolvimento

Durante o desenvolvimento, testes, documentação e demonstrações públicas, dê preferência a **dados fictícios**.

Exemplos devem evitar utilizar dados pertencentes a pessoas ou negócios reais.

Quando forem necessários registros de demonstração, utilize informações claramente fictícias, como:

- nomes inventados;
- números de telefone não reais;
- descrições fictícias;
- imagens apropriadas para demonstração e com uso autorizado;
- valores que não representem informações privadas de terceiros.

O objetivo é permitir que o projeto seja desenvolvido e apresentado publicamente sem expor dados desnecessários.

## Evolução desta política

Esta política acompanha o estágio atual do BAIRANA e poderá evoluir conforme o projeto amadurecer.

Novas orientações poderão ser adicionadas quando surgirem, por exemplo:

- um canal privado oficial para relatos de vulnerabilidades;
- novos componentes ou serviços;
- processos de CI/CD;
- infraestrutura de produção;
- mecanismos adicionais de proteção;
- necessidades de segurança identificadas durante o desenvolvimento.

As alterações devem continuar seguindo o princípio de aplicar **controles de segurança proporcionais ao risco e à maturidade do projeto**, sem adicionar processos ou garantias que ainda não existam.

[^1]: Mesmo após a remoção do conteúdo atual, uma credencial pode permanecer acessível em commits anteriores, clones, forks, caches ou outras cópias do repositório. Por isso, a revogação ou rotação é necessária.