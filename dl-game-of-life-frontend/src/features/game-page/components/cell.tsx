"use client";

import React, { FC } from 'react';
import { BaseBoardCell, BoardCell } from "@/features/game-page/types/board";

import { useGameStore } from "@/stores/game-store";
import { useGameActions } from "@/hooks/game-hooks";

/**
 * Cell component represents a single cell in the Game of Life board.
 * It handles toggling the cell's alive/dead state on click.
 */
const Cell: FC<BaseBoardCell> = ({ rowNumber, columnNumber }) => {
    // Hook for actions like saving the current board state
    const { saveCurrentState } = useGameActions();

    // State setters from the store
    const saveBoardCell = useGameStore((state) => state.saveBoardCell);
    const setIsBoardLocked = useGameStore((state) => state.setIsBoardLocked);

    // Get the current board from the store
    const board = useGameStore((state) => state.board);

    // Find the cell in the board that matches the given row and column
    const cell = board?.cells.find(c => c.rowNumber === rowNumber && c.columnNumber === columnNumber);

    // Determine if the cell is alive
    const isAlive = cell?.isAlive || false;

    /**
     * Toggles the cell's alive/dead status.
     * Locks the board during the update to avoid race conditions.
     */
    const handleToggle = async () => {
        const isBoardLocked = useGameStore.getState().isBoardLocked;
        if (!isBoardLocked) {
            setIsBoardLocked(true);

            // Toggle the current cell state
            const newStatus = !isAlive;
            const newCell: BoardCell = { rowNumber: rowNumber, columnNumber: columnNumber, isAlive: newStatus };

            // Save the updated cell to the store
            saveBoardCell(newCell);

            // Persist the new board state to the backend
            await saveCurrentState();

            // Unlock the board after update
            setIsBoardLocked(false);
        }
    }

    // Render the cell with 'active' class if it is alive
    return (
        <div className={`board-cell ${isAlive ? 'active' : ''}`} onClick={handleToggle}></div>
    );
}

export default Cell;
