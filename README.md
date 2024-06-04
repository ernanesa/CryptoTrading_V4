# Crypto Trading Platform

[![Coverage](http://152.67.54.41:9001/api/project_badges/measure?project=CryptoTrading&metric=coverage&token=sqb_f26184c7caa31f9f5ebbef4fd74068522f677225)](http://152.67.54.41:9001/dashboard?id=CryptoTrading)
[![Duplicated Lines (%)](http://152.67.54.41:9001/api/project_badges/measure?project=CryptoTrading&metric=duplicated_lines_density&token=sqb_f26184c7caa31f9f5ebbef4fd74068522f677225)](http://152.67.54.41:9001/dashboard?id=CryptoTrading)
[![Lines of Code](http://152.67.54.41:9001/api/project_badges/measure?project=CryptoTrading&metric=ncloc&token=sqb_f26184c7caa31f9f5ebbef4fd74068522f677225)](http://152.67.54.41:9001/dashboard?id=CryptoTrading)
[![Maintainability Rating](http://152.67.54.41:9001/api/project_badges/measure?project=CryptoTrading&metric=sqale_rating&token=sqb_f26184c7caa31f9f5ebbef4fd74068522f677225)](http://152.67.54.41:9001/dashboard?id=CryptoTrading)
[![Quality Gate Status](http://152.67.54.41:9001/api/project_badges/measure?project=CryptoTrading&metric=alert_status&token=sqb_f26184c7caa31f9f5ebbef4fd74068522f677225)](http://152.67.54.41:9001/dashboard?id=CryptoTrading)
[![Reliability Rating](http://152.67.54.41:9001/api/project_badges/measure?project=CryptoTrading&metric=reliability_rating&token=sqb_f26184c7caa31f9f5ebbef4fd74068522f677225)](http://152.67.54.41:9001/dashboard?id=CryptoTrading)
[![Security Rating](http://152.67.54.41:9001/api/project_badges/measure?project=CryptoTrading&metric=security_rating&token=sqb_f26184c7caa31f9f5ebbef4fd74068522f677225)](http://152.67.54.41:9001/dashboard?id=CryptoTrading)
[![Technical Debt](http://152.67.54.41:9001/api/project_badges/measure?project=CryptoTrading&metric=sqale_index&token=sqb_f26184c7caa31f9f5ebbef4fd74068522f677225)](http://152.67.54.41:9001/dashboard?id=CryptoTrading)

## Visão Geral

Crypto Trading Platform é um projeto desenvolvido para gerenciar negociações de criptomoedas de forma automatizada. A plataforma é composta por múltiplos serviços que trabalham em conjunto para fornecer uma solução completa para negociação de criptomoedas.

## Estrutura do Projeto

- **Scheduler**: Serviço responsável pelo agendamento de chamadas para as rotas.
- **DataCollection**: Serviço responsável pela coleta de dados.
- **Recommendation**: Serviço responsável pela sugestão de valores de compra e venda.
- **Trading**: Serviço responsável pela negociação em si.
- **FrontEnd**: Interface do usuário desenvolvida em Blazor.

## Tecnologias Utilizadas

- .NET 8.0
- Docker
- GitHub Actions
- SonarQube
- Blazor

## Configuração e Execução

### Pré-requisitos

- .NET 8.0 SDK
- Docker
- Docker Compose

### Clonar o Repositório

```sh
git clone git@github.com:ernanesa/CryptoTrading_V4.git
cd CryptoTrading_V4
