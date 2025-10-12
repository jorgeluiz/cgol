// features/game-page/components/Board.tsx
"use client";
import { useGameStore } from "@/stores/game-store";
import Cell from "@/features/game-page/components/cell";

export const Board = () => {
    const boardTotalRows = useGameStore((state) => state.boardTotalRows);
    const boardTotalColumns = useGameStore((state) => state.boardTotalColumns);

    if (!boardTotalRows || !boardTotalColumns) {
        return null;
    }

    return (
        <div className="board-container">
            {Array.from({ length: boardTotalRows }, (_r, rowIndex) => (
                <div key={rowIndex} className="board-row">
                    {Array.from({ length: boardTotalColumns }, (_c, colIndex) => (
                        <Cell key={`${rowIndex}-${colIndex}`} rowNumber={rowIndex} columnNumber={colIndex} />
                    ))}
                </div>
            ))}
        </div>
    );
};