# 🧬 Conway’s Game of Life  
* Full-stack implementation with .NET, React, MongoDB and Docker.*

[![.NET](https://img.shields.io/badge/.NET-7.0-blue)](https://dotnet.microsoft.com/)
![React](https://img.shields.io/badge/React-19-61DAFB)
![MongoDB](https://img.shields.io/badge/MongoDB-7.0-brightgreen)
![Docker](https://img.shields.io/badge/Docker-ready-blue)
![License](https://img.shields.io/badge/license-MIT-lightgrey)

---

## Overview

This project implements **Conway’s Game of Life**, a zero-player cellular automaton where cells evolve based on simple rules of life and death.  
It demonstrates **clean architecture practices** with a **.NET backend**, **React/Next.js frontend**, **MongoDB** persistence, and **Docker** for easy orchestration.

---

## Project Structure

```bash
.
├── backend/
│   ├── DL.GameOfLife.Api/           # .NET Web API
│   ├── DL.GameOfLife.Tests/         # Unit tests
│   └── ...
├── dl-game-of-life-frontend/        # React / Next.js frontend
├── docker-compose.yml               # Production setup
├── docker-compose.dev.yml           # Development setup
└── README.md
```

---

##  Running the Project

###  Option 1 — Production Mode (recommended for demo)

```bash
docker compose up --build
```

This will start the **frontend**, **backend**, and **MongoDB** containers.  
Once everything is running, open your browser and go to:

[http://localhost:3000](http://localhost:3000)

---

###  Option 2 — Development Mode (for local coding)

```bash
docker compose -f docker-compose.dev.yml up --build
```

This mode enables **live reload** for backend and frontend.

- Swagger (backend API docs): [http://localhost:5217/swagger/index.html](http://localhost:5217/swagger/index.html)  
- To start the frontend manually:
  ```bash
  cd dl-game-of-life-frontend
  npm install
  npm run dev
  ```

The app will be available at [http://localhost:3000](http://localhost:3000).

---

### Option 3 — Run only MongoDB

If you want to run the backend and frontend manually:

```bash
docker compose -f docker-compose.dev.yml up database
```

Then start the backend:
```bash
cd backend/DL.GameOfLife.Api
dotnet run --configuration Debug
```

And the frontend:
```bash
cd dl-game-of-life-frontend
npm run dev
```

---

## Running Tests and Generating Coverage

Make sure you have the `reportgenerator` tool installed globally:

```bash
dotnet tool install -g dotnet-reportgenerator-globaltool
```

Then run:

```bash
dotnet test --collect:"XPlat Code Coverage"
reportgenerator "-reports:**/coverage.cobertura.xml" "-targetdir:coveragereport" -reporttypes:Html
```

The coverage report will be generated at:
```
backend/DL.GameOfLife.Tests/coveragereport/index.html
```

---

## About Conway’s Game of Life

The Game of Life follows four simple rules for cell survival:

1. Any live cell with fewer than two live neighbors dies (underpopulation).  
2. Any live cell with two or three live neighbors lives on.  
3. Any live cell with more than three live neighbors dies (overpopulation).  
4. Any dead cell with exactly three live neighbors becomes alive (reproduction).

---

## License

This project is licensed under the **MIT License** — see the [LICENSE](LICENSE) file for details.

---

## Author

**Jorge Luiz**  
[GitHub](https://github.com/jorgeluiz) • [LinkedIn](https://www.linkedin.com/in/luizsilvajj)
