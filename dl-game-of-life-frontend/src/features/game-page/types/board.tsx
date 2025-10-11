import { BoardCell } from "./board-cell";

export interface Board {
    id: string | null,
    cells: Array<BoardCell>
}