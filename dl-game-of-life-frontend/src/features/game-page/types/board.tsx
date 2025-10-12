import { ErrorModel } from "@/types/api";

//Board types and interfaces
export interface Board {
    id: string | null,
    cells: Array<BoardCell>
}

export interface BaseBoardCell {
    rowNumber: number;
    columnNumber: number;
}

export interface BoardCell extends BaseBoardCell {
    isAlive: boolean
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