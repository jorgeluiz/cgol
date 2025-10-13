"use client";

import React, { FC } from 'react';
import { BaseBoardCell, BoardCell } from "@/features/game-page/types/board";

import { useGameStore } from "@/stores/game-store";
import { useGameActions } from "@/hooks/game-hooks";

const Cell: FC<BaseBoardCell> = ({ rowNumber, columnNumber }) => {
    const { saveCurrentState } = useGameActions();
    const saveBoardCell = useGameStore((state) => state.saveBoardCell);
    const setIsBoardLocked = useGameStore((state) => state.setIsBoardLocked);

    const board = useGameStore(
        (state) => state.board,
    );

    const cell = board?.cells.find(c => c.rowNumber === rowNumber && c.columnNumber === columnNumber);

    const isAlive = cell?.isAlive || false;

    const handleToggle = async () => {
        const isBoardLocked = useGameStore.getState().isBoardLocked;
        if (!isBoardLocked) {
            setIsBoardLocked(true);

            const newStatus = !isAlive;
            const newCell: BoardCell = { rowNumber: rowNumber, columnNumber: columnNumber, isAlive: newStatus };
            saveBoardCell(newCell);
            await saveCurrentState();

            setIsBoardLocked(false);
        }
    }

    return (
        <div className={`board-cell ${isAlive ? 'active' : ''}`} onClick={handleToggle}></div>
    );
}


export default Cell;