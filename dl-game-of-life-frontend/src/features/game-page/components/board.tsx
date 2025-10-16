// features/game-page/components/Board.tsx
"use client";

import { useGameStore } from "@/stores/game-store";
import Cell from "@/features/game-page/components/cell";

/**
 * Board component renders the entire Game of Life board as a grid of cells.
 */
export const Board = () => {
    // Get total number of rows and columns from the global store
    const boardTotalRows = useGameStore((state) => state.boardTotalRows);
    const boardTotalColumns = useGameStore((state) => state.boardTotalColumns);

    // If board dimensions are not set, do not render anything
    if (!boardTotalRows || !boardTotalColumns) {
        return null;
    }

    return (
        <div className="board-container">
            {/*
                Loop through each row and render a div for the row.
                For each row, loop through columns and render a Cell component.
            */}
            {Array.from({ length: boardTotalRows }, (_r, rowIndex) => (
                <div key={rowIndex} className="board-row">
                    {Array.from({ length: boardTotalColumns }, (_c, colIndex) => (
                        <Cell
                            key={`${rowIndex}-${colIndex}`} // Unique key for each cell
                            rowNumber={rowIndex}           // Pass row index
                            columnNumber={colIndex}        // Pass column index
                        />
                    ))}
                </div>
            ))}
        </div>
    );
};
