import { ReactNode } from "react";

import { RenderBoardCellProps } from "@/features/game-page/types/board-cell";
import BoardCell from "@/features/game-page/components/board-cell";

export const renderCells = ({ rowNumber, cellsNumber }: RenderBoardCellProps): ReactNode => {
    return (
        <>
            {Array.from({ length: cellsNumber }, (_, cellIndex) => {
                let initialIsActive: boolean = false;
                return <BoardCell key={`${rowNumber}-${cellIndex}`} rowNumber={rowNumber} cellIndex={cellIndex} initialIsActive={initialIsActive} />;
            })}
        </>
    );
}

export const renderRows = (rowNumber: number): ReactNode => {
    return (
        <>
            {Array.from({ length: rowNumber }, (_, index) => (
                <div key={index} className="board-row">
                    {renderCells({ rowNumber: index, cellsNumber: 50 })}
                </div>
            ))}
        </>
    );
}