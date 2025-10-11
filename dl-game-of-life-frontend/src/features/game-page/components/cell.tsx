"use client";

import React, { useState, FC } from 'react';
import { BoardCell } from "@/features/game-page/types/board-cell";

const Cell: FC<BoardCell> = ({ rowNumber, cellNumber, isActive = false }) => {

    const [cellIsActive, setActive] = useState<boolean>(isActive);

    const toggle = () => {
        setActive(!cellIsActive);
    }

    return (
        <div className={`board-cell ${cellIsActive ? 'active' : ''}`} onClick={toggle}></div>
    );
}


export default Cell;