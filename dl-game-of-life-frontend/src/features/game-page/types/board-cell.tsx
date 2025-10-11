export interface BoardCell {
    rowNumber: number;
    columnNumber: number;
    isActive: boolean
}

export interface RenderBoardCellProps {
    rowNumber: number;
    totalColumns: number;
}