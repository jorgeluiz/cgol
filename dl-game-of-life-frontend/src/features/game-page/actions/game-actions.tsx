import { Board } from "@/features/game-page/types/board";
import { GameSettings } from "@/constants/game-settings";


export const newBoard = (): Board => {
    let board: Board = { id: "", cells: [] };

    Array.from({ length: GameSettings.BOARD_MAX_ROWS }, (_r, rowIndex) => {
        Array.from({ length: GameSettings.BOARD_MAX_COLUMNS }, (_c, columnIndex) => {
            board.cells.push({ columnNumber: columnIndex, rowNumber: rowIndex, isActive: false });
        });
    });

    return board;
}