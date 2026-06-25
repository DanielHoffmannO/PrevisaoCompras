ðŸŒ [English](README.en.md) | [EspaÃ±ol](README.es.md)

# ðŸ“Š PrevisÃ£o de Compras

[![.NET CI](https://github.com/DanielHoffmannO/PrevisaoCompras/actions/workflows/dotnet.yml/badge.svg)](https://github.com/DanielHoffmannO/PrevisaoCompras/actions/workflows/dotnet.yml)
![.NET 9](https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet)
![SQLite](https://img.shields.io/badge/SQLite-003B57?logo=sqlite&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?logo=docker&logoColor=white)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

> Sistema inteligente de previsÃ£o de compras para mercado e feira, utilizando **mÃ©dia mÃ³vel ponderada** sobre o histÃ³rico de consumo. Ajuda a planejar quanto comprar de cada produto no prÃ³ximo mÃªs com base nos Ãºltimos 6 meses.

<img width="590" height="561" alt="image" src="https://github.com/user-attachments/assets/957727a6-4626-41d9-86ed-08d9d118bee2" />

## ðŸ› ï¸ Tech Stack

| Camada | Tecnologia |
|--------|-----------|
| Backend API | .NET 9 / ASP.NET Core Minimal APIs |
| Worker | .NET 9 Worker Service (executa a cada 6h) |
| Banco de Dados | SQLite + EF Core |
| Frontend | HTML + Chart.js (single page dashboard) |
| Infra | Docker Compose |
| CI | GitHub Actions |

## ðŸš€ Como Rodar

### Docker Compose (recomendado)

```bash
docker compose up --build
```

### Local

```bash
dotnet run --project src/PrevisaoCompras.Api
```

Acesse o dashboard em **http://localhost:5000**

## ðŸ“¡ Endpoints

| MÃ©todo | Rota | DescriÃ§Ã£o |
|--------|------|-----------|
| GET | `/` | Dashboard (HTML + grÃ¡ficos) |
| GET | `/api/dashboard` | Dados de previsÃ£o (JSON) |

## ðŸ§® Algoritmo

O sistema utiliza **MÃ©dia MÃ³vel Ponderada (WMA)** com janela de 6 meses, onde meses mais recentes tÃªm maior peso:

$$
\text{PrevisÃ£o} = \frac{\sum_{i=1}^{n} w_i \cdot x_i}{\sum_{i=1}^{n} w_i}
$$

**Pesos:** `[6, 5, 4, 3, 2, 1]` (mÃªs mais recente â†’ peso 6)

### Exemplo

| MÃªs | Quantidade | Peso |
|-----|-----------|------|
| Jun (mais recente) | 10 | 6 |
| Mai | 8 | 5 |
| Abr | 12 | 4 |
| Mar | 6 | 3 |
| Fev | 9 | 2 |
| Jan | 7 | 1 |

$$
\text{PrevisÃ£o} = \frac{(10 \times 6) + (8 \times 5) + (12 \times 4) + (6 \times 3) + (9 \times 2) + (7 \times 1)}{6 + 5 + 4 + 3 + 2 + 1} = \frac{60+40+48+18+18+7}{21} = 9.1
$$

## ðŸ—ï¸ Arquitetura

```
â”Œâ”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”
â”‚            Docker Compose               â”‚
â”œâ”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”¬â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”¤
â”‚   API (.NET 9)  â”‚   Worker (.NET 9)     â”‚
â”‚   Minimal APIs  â”‚   BackgroundService   â”‚
â”‚   + Dashboard   â”‚   Executa a cada 6h   â”‚
â”œâ”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”´â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”¤
â”‚           EF Core + SQLite              â”‚
â”‚     (Seed: 10 produtos Ã— 12 meses)     â”‚
â””â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”˜
```

- **API**: Serve o dashboard e expÃµe os dados de previsÃ£o via JSON
- **Worker**: Recalcula as previsÃµes periodicamente (a cada 6h) analisando o histÃ³rico dos Ãºltimos 6 meses
- **SQLite**: Banco leve, sem necessidade de servidor externo

## ðŸ“„ LicenÃ§a

Este projeto estÃ¡ sob a licenÃ§a [MIT](LICENSE).

## ðŸ‘¤ Autor

**Daniel Hoffmann**

[![GitHub](https://img.shields.io/badge/GitHub-DanielHoffmannO-181717?logo=github)](https://github.com/DanielHoffmannO)
