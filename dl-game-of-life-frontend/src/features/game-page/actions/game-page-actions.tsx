import { ReactNode } from "react";

import { RenderBoardCellProps } from "@/features/game-page/types/board-cell";
import Cell from "@/features/game-page/components/cell";

export const renderCells = ({ rowNumber, totalColumns }: RenderBoardCellProps): ReactNode => {
    return (
        <>
            {Array.from({ length: totalColumns }, (_, cellIndex) => {
                let initialIsActive: boolean = false;
                return <Cell key={`${rowNumber}-${cellIndex}`} rowNumber={rowNumber} cellNumber={cellIndex} isActive={initialIsActive} />;
            })}
        </>
    );
}

export const renderRows = (totalRows: number, totalColumns: number): ReactNode => {
    return (
        <>
            {Array.from({ length: totalRows }, (_, index) => (
                <div key={index} className="board-row">
                    {renderCells({ rowNumber: index, totalColumns: totalColumns })}
                </div>
            ))}
        </>
    );
}