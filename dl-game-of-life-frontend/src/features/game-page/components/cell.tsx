"use client";

import React, { FC } from 'react';
import { BaseBoardCell, BoardCell } from "@/features/game-page/types/board";

import { useGameStore } from "@/stores/game-store";


const Cell: FC<BaseBoardCell> = ({ rowNumber, columnNumber }) => {

    const saveBoardCell = useGameStore((state) => state.saveBoardCell);

    const board = useGameStore(
        (state) => state.board,
    );

    const cell = board?.cells.find(c => c.rowNumber === rowNumber && c.columnNumber === columnNumber);

    const isAlive = cell?.isAlive || false;

    const toggle = () => {
        let newStatus = !isAlive;
        let newCell: BoardCell = { rowNumber: rowNumber, columnNumber: columnNumber, isAlive: newStatus };
        saveBoardCell(newCell);
    }

    return (
        <div className={`board-cell ${isAlive ? 'active' : ''}`} onClick={toggle}></div>
    );
}


export default Cell;