# FIAP TECH CHALLENGE – FASE 4

## Grupo 118

### Participantes

- **Armando José Vieira Dias de Oliveira**  
  GitHub: `@armandojoseoliveira` — User: *Nando*  
  RM361112

- **Marlon dos Santos Limeira**  
  GitHub: `@marlonsantos4509` — User: *Marlon Santos*  
  RM361866

- **Matheus Nascimento Costa**  
  GitHub: `@matheus_coast` — User: *Matheus_coast*  
  RM363404

- **Ricardo Noronha de Menezes**  
  GitHub: `@ricardo_nm` — User: *ricardo_nm*  
  RM363183

---

### Observação Importante

> Embora os repositórios façam referência à **Fase 3**, o código e a solução apresentados correspondem à **Fase 4**, pois o grupo optou por evoluir o mesmo projeto de forma contínua.

---

## Repositórios de Código

- **Payments**  
  https://github.com/ricardonoronha/fcg-fase3-payments

- **Users**  
  https://github.com/ricardonoronha/fcg-fase3-users

- **Games**  
  https://github.com/ricardonoronha/fcg-fase3-games

---

## Vídeo de Demonstração

- 🎥 https://youtu.be/bTZaDeKj-LQ

---

## Documentação DDD (Event Storming)

- 📌 https://miro.com/app/board/uXjVIE9R-Pg=/?share_link_id=308339772603

---

# Documentação Técnica – Tech Challenge

## Visão Geral da Solução

Este projeto foi desenvolvido como parte do **Tech Challenge**, com o objetivo de implementar uma arquitetura de **microsserviços moderna**, executando em **Kubernetes**, com foco em:

- Observabilidade
- Comunicação assíncrona
- Boas práticas de Cloud Native

A solução simula um cenário real de sistemas distribuídos, contemplando microsserviços independentes, integração via mensageria, infraestrutura containerizada e monitoramento contínuo.

O foco principal foi entregar uma solução **funcional**, **coerente** e **alinhada com práticas reais de produção**.

---

## Arquitetura Geral

A arquitetura da solução é composta por **microsserviços backend independentes**, responsáveis por domínios distintos da aplicação, integrados por mensageria.

### Componentes Principais

- Microsserviço **Games**
- Microsserviço **Payments**
- Microsserviço **Users**
- **RabbitMQ** para comunicação assíncrona
- **Kubernetes (Azure Kubernetes Service – AKS)**
- **Prometheus** para coleta de métricas
- **Grafana** para visualização e análise
- **Azure Container Registry (ACR)** para armazenamento de imagens Docker

---

### Fluxo de Comunicação (Simplificado)

- O microsserviço **Payments** atua como **produtor de mensagens**, publicando eventos no RabbitMQ.
- O microsserviço **Games** consome essas mensagens de forma **assíncrona**, processando os eventos recebidos.
- Os serviços e a infraestrutura expõem métricas que são coletadas pelo **Prometheus** e visualizadas no **Grafana**.

---

## Tecnologias Utilizadas

### Backend

- .NET 8
- ASP.NET Core Web API
- Configuração baseada em variáveis de ambiente

### Containers

- Docker
- Dockerfiles com **multi-stage build**
- Imagens otimizadas para execução em Kubernetes

### Orquestração

- Kubernetes (AKS)
- Namespaces para organização de recursos
- Services para comunicação interna
- ConfigMaps e Secrets para configuração
- Liveness e Readiness Probes

### Mensageria

- RabbitMQ
- Comunicação assíncrona entre microsserviços
- Configuração via variáveis de ambiente e Secrets

### Observabilidade

- Prometheus para coleta de métricas
- Grafana para dashboards e visualização

### Cloud

- Microsoft Azure
- Azure Kubernetes Service (AKS)
- Azure Container Registry (ACR)

---

## Observabilidade e Monitoramento

Foi implementada uma camada de observabilidade com foco inicial em **CPU e memória**, utilizando Prometheus e Grafana.

### Métricas Monitoradas

- Consumo de CPU do cluster
- Consumo de memória por nó
- Estado geral dos nós do Kubernetes

As métricas são coletadas a partir dos exporters do cluster Kubernetes e apresentadas em dashboards no Grafana, permitindo análise em tempo real e suporte ao diagnóstico de desempenho.

---

## Deploy e Execução da Solução

O processo de deploy segue as etapas abaixo:

1. Build das imagens Docker dos microsserviços
2. Publicação das imagens no Azure Container Registry
3. Criação dos manifests Kubernetes (YAML)
4. Deploy dos serviços no cluster AKS
5. Exposição dos serviços internamente no cluster
6. Monitoramento contínuo via Prometheus e Grafana

Os manifests Kubernetes foram organizados separando claramente:

- Namespaces
- Services
- ConfigMaps
- Secrets

Essa organização facilita manutenção e entendimento da infraestrutura.

---

## Boas Práticas Aplicadas

Durante o desenvolvimento da solução, foram aplicadas as seguintes boas práticas:

- Separação clara de responsabilidades entre microsserviços
- Comunicação assíncrona para redução de acoplamento
- Containers imutáveis
- Configuração externa via variáveis de ambiente
- Observabilidade desde o início do projeto
- Foco em simplicidade e funcionamento real

As decisões técnicas priorizaram clareza, coerência arquitetural e aderência a padrões modernos de engenharia de software.

---

## Limitações Conhecidas e Próximos Passos

Devido ao escopo e tempo disponível, alguns pontos ficaram como evoluções futuras:

- Implementação de CI/CD automatizado
- Estratégias avançadas de retry e dead-letter no RabbitMQ
- Tracing distribuído com OpenTelemetry
- Autoscaling horizontal com HPA
- Monitoramento de métricas de aplicação

Essas melhorias podem ser implementadas sem necessidade de reestruturação da arquitetura atual.

---

## Conclusão

A solução entregue atende aos objetivos propostos pelo **Tech Challenge**, demonstrando domínio prático em:

- Microsserviços
- Containers
- Kubernetes
- Mensageria
- Observabilidade

O projeto priorizou uma arquitetura funcional, realista e alinhada com cenários profissionais, servindo como base sólida para evoluções futuras.
