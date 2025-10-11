export interface BoardCell {
    rowNumber: number;
    cellNumber: number;
    isActive: boolean
}

export interface RenderBoardCellProps {
    rowNumber: number;
    totalColumns: number;
}