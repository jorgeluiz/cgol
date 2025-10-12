# Implementation of Conway's Game Of Life
The goal of this project is implement [Conway's Game of Life on Wikipedia](https://en.wikipedia.org/wiki/Conway%27s_Game_of_Life)

## Technologies used
C# (Backend)
MongoDB (Database)
React/NextJS (Frontend)
Docker

## How to run the project
This project could run locally with docker compose. At the root folder, run the following commands depending on the desired behavior

**- PROD**
```
docker compose up
```
Then access `http://localhost:3000` at your browser



**- DEV**
```
docker-compose -f docker-compose.dev.yml up
```

A swagger will be available at `http://localhost:5217/swagger/index.html`

You will need to run a `React` server, so, in another command terminal, go to the `dl-game-of-life-frontend` and run
```
npm run dev
```



**- Database only**
```
docker-compose -f docker-compose.dev.yml up database
```

All that this command do its to provide a MongoDB, so, you have to start the backend startup project at `backend/DL.GameOfLife.Api/` and the front and at `dl-game-of-life-frontend` folders

For the backend, use the `Debug mode` of your desired tool, or run `dotnet run debug` at any command line tool. To the frontend, execute the  `npm run dev` command.




## Test commands
Certify that you are in the  `backend` folder and have `reportgenerator` installed on your computer
If you dont have it installed run the following code

```
dotnet tool install -g dotnet-reportgenerator-globaltool
```

After that, run the following commands

```
dotnet test --collect:"XPlat Code Coverage"
dotnet reportgenerator "-reports:**/coverage.cobertura.xml" "-targetdir:coveragereport" -reporttypes:Html
```

The result output will be at the Test project (`backend/DL.GameOfLife.Tests/`)