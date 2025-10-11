import { Board } from "@/features/game-page/types/board";

export interface GameState {
    board: Board | null;
    boardTotalRows: number | null;
    boardTotalColumns: number | null;
    save: (board: Board, totalColumns: number, totalRows: number) => void;
    clear: () => void;
}