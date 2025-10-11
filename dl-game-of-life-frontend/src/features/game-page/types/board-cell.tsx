export interface BoardCellProps {
    rowNumber: number;
    cellIndex: number;
    initialIsActive: boolean
}

export interface RenderBoardCellProps {
    rowNumber: number;
    cellsNumber: number;
}