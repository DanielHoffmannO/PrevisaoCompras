🌐 [English](README.en.md) | [Español](README.es.md)

# 📊 Previsão de Compras

[![.NET CI](https://github.com/DanielHoffmannO/PrevisaoCompras/actions/workflows/dotnet.yml/badge.svg)](https://github.com/DanielHoffmannO/PrevisaoCompras/actions/workflows/dotnet.yml)
![.NET 9](https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet)
![SQLite](https://img.shields.io/badge/SQLite-003B57?logo=sqlite&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?logo=docker&logoColor=white)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

> Sistema inteligente de previsão de compras para mercado e feira, utilizando **média móvel ponderada** sobre o histórico de consumo. Ajuda a planejar quanto comprar de cada produto no próximo mês com base nos últimos 6 meses.

<img width="590" height="561" alt="image" src="https://github.com/user-attachments/assets/957727a6-4626-41d9-86ed-08d9d118bee2" />

## 🛠️ Tech Stack

| Camada | Tecnologia |
|--------|-----------|
| Backend API | .NET 9 / ASP.NET Core Minimal APIs |
| Worker | .NET 9 Worker Service (executa a cada 6h) |
| Banco de Dados | SQLite + EF Core |
| Frontend | HTML + Chart.js (single page dashboard) |
| Infra | Docker Compose |
| CI | GitHub Actions |

## 🚀 Como Rodar

### Docker Compose (recomendado)

```bash
docker compose up --build
```

### Local

```bash
dotnet run --project src/PrevisaoCompras.Api
```

Acesse o dashboard em **http://localhost:5000**

## 📡 Endpoints

| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/` | Dashboard (HTML + gráficos) |
| GET | `/api/dashboard` | Dados de previsão (JSON) |

## 🧮 Algoritmo

O sistema utiliza **Média Móvel Ponderada (WMA)** com janela de 6 meses, onde meses mais recentes têm maior peso:

$$
\text{Previsão} = \frac{\sum_{i=1}^{n} w_i \cdot x_i}{\sum_{i=1}^{n} w_i}
$$

**Pesos:** `[6, 5, 4, 3, 2, 1]` (mês mais recente → peso 6)

### Exemplo

| Mês | Quantidade | Peso |
|-----|-----------|------|
| Jun (mais recente) | 10 | 6 |
| Mai | 8 | 5 |
| Abr | 12 | 4 |
| Mar | 6 | 3 |
| Fev | 9 | 2 |
| Jan | 7 | 1 |

$$
\text{Previsão} = \frac{(10 \times 6) + (8 \times 5) + (12 \times 4) + (6 \times 3) + (9 \times 2) + (7 \times 1)}{6 + 5 + 4 + 3 + 2 + 1} = \frac{60+40+48+18+18+7}{21} = 9.1
$$

## 🏗️ Arquitetura

```
┌─────────────────────────────────────────┐
│            Docker Compose               │
├─────────────────┬───────────────────────┤
│   API (.NET 9)  │   Worker (.NET 9)     │
│   Minimal APIs  │   BackgroundService   │
│   + Dashboard   │   Executa a cada 6h   │
├─────────────────┴───────────────────────┤
│           EF Core + SQLite              │
│     (Seed: 10 produtos × 12 meses)     │
└─────────────────────────────────────────┘
```

- **API**: Serve o dashboard e expõe os dados de previsão via JSON
- **Worker**: Recalcula as previsões periodicamente (a cada 6h) analisando o histórico dos últimos 6 meses
- **SQLite**: Banco leve, sem necessidade de servidor externo

## 📄 Licença

Este projeto está sob a licença [MIT](LICENSE).

## 👤 Autor

**Daniel Hoffmann**

[![GitHub](https://img.shields.io/badge/GitHub-DanielHoffmannO-181717?logo=github)](https://github.com/DanielHoffmannO)
