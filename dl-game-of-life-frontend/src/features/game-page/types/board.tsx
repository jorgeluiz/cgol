import { ErrorModel } from "@/types/api";

//Board types and interfaces
export interface Board {
    id: string | null,
    cells: Array<BoardCell>
}

export interface BoardCell {
    rowNumber: number;
    columnNumber: number;
    isAlive: boolean
}

export interface RenderBoardCellProps {
    rowNumber: number;
    totalColumns: number;
}

export interface BoardRequest {
    cells: BoardCell[] | null;
}

export interface UpdateBoardRequest {
    id: string;
    cells: BoardCell[] | null;
}

export interface BoardResponse {
    id: string | null;
    parentId: string | null;
    cells: BoardCell[] | null;
    errors: ErrorModel[] | null;
}