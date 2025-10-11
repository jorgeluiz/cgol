"use client";

import React, { useState, FC } from 'react';
import { BoardCellProps } from "@/features/game-page/types/board-cell";

const BoardCell: FC<BoardCellProps> = ({ rowNumber, cellIndex, initialIsActive = false }) => {

    const [isActive, setActive] = useState<boolean>(initialIsActive);

    const toggle = () => {
        setActive(!isActive);
    }

    return (
        <div className={`board-cell ${isActive ? 'active' : ''}`} onClick={toggle}></div>
    );
}


export default BoardCell;