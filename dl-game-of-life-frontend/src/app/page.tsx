"use client"
import { renderRows } from "@/features/game-page/actions/game-page-actions";

import { useGameStore } from "@/stores/game-store";

export default function GamePage() {

  const boardTotalRows = useGameStore((state) => state.boardTotalRows);
  const boardTotalColumns = useGameStore((state) => state.boardTotalColumns);

  return (
    <div className="grid items-center justify-items-center pt-20 pb-20">
      <main className="flex flex-col items-center sm:items-start">
        {boardTotalColumns && boardTotalRows && (
          <div className="board-container">
            {renderRows(boardTotalRows, boardTotalColumns)}
          </div>
        )}
      </main>
    </div>
  );
}

