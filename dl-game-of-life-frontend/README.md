# Conway's Game of Life - Frontend

This is the frontend repository for an implementation of John Conway's "Game of Life," developed with Next.js and TypeScript. The application allows users to view and interact with Game of Life simulations directly in the browser.

## 🚀 Overview

The Game of Life is a cellular automaton that requires no players. The "game" evolves with each step (or "generation") based on a set of simple rules, resulting in complex and fascinating patterns. This web interface provides a visual and interactive way to explore these patterns.

## ✨ Technologies Used

The project was built using a set of modern technologies for web development:

* **[Next.js](https://nextjs.org/)**: A React framework for building fast, server-rendered web applications.
* **[React](https://react.dev/)**: A JavaScript library for building user interfaces.
* **[TypeScript](https://www.typescriptlang.org/)**: A superset of JavaScript that adds static typing to the code.
* **[Tailwind CSS](https://tailwindcss.com/)**: A utility-first CSS framework for rapid and responsive styling.
* **[ESLint](https://eslint.org/)**: A tool for linting and standardizing code.

## 🏁 Getting Started

Follow the steps below to set up and run the project in your local development environment.

### Prerequisites

* [Node.js](https://nodejs.org/) (version 18.x or higher)
* [npm](https://www.npmjs.com/) or [yarn](https://yarnpkg.com/)

### Installation and Execution

1.  **Clone the repository:**
    ```bash
    git clone [https://github.com/jorgeluiz/cgol.git](https://github.com/jorgeluiz/cgol.git)
    ```

2.  **Navigate to the project directory:**
    ```bash
    cd cgol/dl-game-of-life-frontend
    ```

3.  **Install the dependencies:**
    ```bash
    npm install
    # or
    # yarn install
    ```

4.  **Run the development server:**
    ```bash
    npm run dev
    # or
    # yarn dev
    ```

5.  **Open your browser:**
    Go to `http://localhost:3000` to see the application running.

## 🛠️ Available Scripts

In the project directory, you can run the following scripts:

* `npm run dev`: Starts the application in development mode.
* `npm run build`: Compiles the application for production.
* `npm run start`: Starts a production server after the build.
* `npm run lint`: Runs ESLint to analyze the code for errors and style issues.

## 📂 Project Structure

The folder structure follows the Next.js standard with the App Router, incorporating a feature-sliced approach:

```
dl-game-of-life-frontend/
├── src/
│   ├── app/                 # Application pages and layouts
│   ├── components/          # Global reusable React components
│   ├── features/            # Feature-sliced structure
│   │   └── game-page/
│   │       ├── components/  # Components specific to the game page
│   │       └── types/       # Type definitions for the game feature (e.g., Board, Cell)
│   ├── services/            # API communication logic
│   │   └── game-service.ts
│   └── utils/               # Global utility functions (e.g., api helpers)
├── public/                  # Static files
├── .eslintrc.json           # ESLint configuration
├── next.config.mjs          # Next.js configuration
├── package.json             # Dependencies and scripts
└── tailwind.config.ts       # Tailwind CSS configuration
```

## 🎮 Game Actions and API Communication

The frontend communicates with a backend API to process and persist the game state. All communication is centralized in the `src/services/game-service.ts` service. Each game state is saved and has a unique `boardId`.

User actions manipulate these states through the following API calls:

### 1. Create a New Game

Initializes a new board on the backend.

* **Function**: `createGame(initialState)`
* **Endpoint**: `POST /GameOfLife`
* **Request Body (`BoardRequest`)**:
    ```json
    {
      "cells": [
        { "rowNumber": 0, "columnNumber": 1, "isAlive": true },
        { "rowNumber": 1, "columnNumber": 2, "isAlive": true }
      ]
    }
    ```
* **Response Body (`BoardResponse`)**: Returns the complete state of the new board, including its unique `id`.
    ```json
    {
      "id": "some-unique-id",
      "parentId": null,
      "cells": [],
      "errors": null
    }
    ```

### 2. Advance to the Next Generation (Next / Start)

Calculates the next state of the board from the current state.

* **Function**: `getNextState(boardId)`
* **Endpoint**: `GET /GameOfLife/next_state/{boardId}`
* **URL Parameters**: `boardId` (string).
* **Response Body (`BoardResponse`)**: Returns the new state of the board, including a new `id` and the `parentId` referencing the previous state.

### 3. Jump Multiple Generations (Jump States)

Advances the simulation by a specified number of generations.

* **Function**: `incrementState(boardId, statesToIncrement)`
* **Endpoint**: `GET /GameOfLife/increment_state/{boardId}/{statesToIncrement}`
* **URL Parameters**: `boardId` (string) and `statesToIncrement` (number).
* **Response Body (`BoardResponse`)**: Returns the board state after jumping the generations.

### 4. Manually Update Cells

Allows changing the state of specific cells on an existing board (e.g., by clicking on them).

* **Function**: `updateGame(board)`
* **Endpoint**: `PUT /GameOfLife/{boardId}`
* **Request Body (`UpdateBoardRequest`)**:
    ```json
    {
      "id": "some-unique-id",
      "cells": [
        { "rowNumber": 5, "columnNumber": 10, "isAlive": false }
      ]
    }
    ```
* **Response Body (`BoardResponse`)**: Returns the updated state of the board.

### Other Operations

* **Load a Game**: `getBoardState(boardId)` -> `GET /GameOfLife/{boardId}`
* **Delete a Game**: `deleteGame(boardId)` -> `DELETE /GameOfLife/{boardId}`
