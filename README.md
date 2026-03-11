# Conexão Solidária - ONG Esperança Solidária

## 📝 O Objetivo
A ONG Esperança Solidária atua há mais de 10 anos acolhendo crianças em situação de vulnerabilidade. Atualmente, a gestão de doadores e campanhas de arrecadação é feita de forma manual, limitando a capacidade da ONG de expandir sua ajuda. Para resolver esse problema, a diretoria decidiu criar a plataforma digital "Conexão Solidária", focada em escalabilidade, observabilidade e automação. A missão é arquitetar e desenvolver o MVP dessa plataforma, entregando uma solução robusta e pronta para crescer.

## 🛠️ Requisitos Funcionais Detalhados
- Autenticação e Autorização (RBAC)

- O sistema deve possuir autenticação baseada em Tokens JWT (JSON Web Tokens).

- Devem existir dois perfis (Roles) distintos: GestorONG e Doador.

- Endpoints de gestão devem ser bloqueados apenas para usuários com a role GestorONG.

### Gestão de Campanhas (Acesso: GestorONG)

- Criar/Editar: Cadastro de campanhas contendo obrigatoriamente: Título (string), Descricao (string), DataInicio (datetime), DataFim (datetime), MetaFinanceira (decimal), e Status (Ativa, Concluida, Cancelada).

- Regra de Negócio: Uma campanha não pode ser criada com a data de término no passado e a meta financeira deve ser maior que zero.

### Cadastro de Doador (Acesso: Público)

- O sistema deve permitir o cadastro de novos usuários com os campos: Nome Completo, Email (único), CPF (validar formato) e Senha (armazenada com hash, ex.: BCrypt).

### Painel de Transparência (Acesso: Público)

- Endpoint de Listagem: API pública que retorne apenas as campanhas com status Ativa.

- Dados Retornados: Título, Meta Financeira e o Valor Total Arrecadado até o momento (calculado com base nas doações processadas).

### Processo de Doação (Acesso: Doador Logado)

- O doador logado deve poder enviar uma intenção de doação informando o IdCampanha e o ValorDoacao.

- Regra de Negócio: A doação não pode ser feita para campanhas encerradas ou canceladas.

## ⚙️ Requisitos Técnicos Obrigatórios
- Arquitetura de Microsserviços: A solução deve conter pelo menos dois microsserviços distintos (ex.: API de Campanhas/Usuários e um Worker de Processamento de Doações).

- Comunicação Assíncrona e Mensageria: * Ao receber uma nova doação, a API NÃO deve atualizar o valor arrecadado diretamente no banco de dados.

    - A API deve publicar um evento (ex.: DoacaoRecebidaEvent) em um broker (RabbitMQ ou Kafka).

    - Um segundo serviço (Worker/Consumer) deve consumir essa fila e atualizar o "Valor Total Arrecadado".

- Orquestração com Kubernetes: A aplicação deve rodar em um cluster (Minikube, Kind, etc.) com entrega dos arquivos .yaml (Deployments, Services, ConfigMaps).

- Observabilidade (Zabbix e Grafana):

    - Exposição de métricas de saúde (/health ou /metrics).

    - Dashboard no Grafana exibindo métricas reais (CPU/Memória dos pods ou contagem de requisições).

- Pipeline de CI/CD Automatizado: Pipeline acionado a cada push na branch principal para compilação do código (.NET build) e geração da imagem Docker.

## 📂 Estrutura do Projeto (.NET Clean Architecture / DDD)

Atualmente, sua estrutura está organizada em um único repositório seguindo a Clean Architecture. No entanto, os requisitos técnicos exigem que a solução final seja distribuída.

```md
Ong/
 ├── src/
 │   ├── Ong.Domain/ # Entidades, Regras de Negócio e Interfaces
 │   ├── Ong.Application/ # Casos de Uso, Mappings e DTOs
 │   ├── Ong.Infra/ # DB Context, Repositórios e Broker de Mensageria
 │   └── Ong.Web/ # Controllers, Middlewares e Configuração de API
 └── tests/
     └── Ong.Tests/ #Testes de Unidade e Integração
```