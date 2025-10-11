import { Board } from "@/features/game-page/types/board";
import { BoardCell } from "@/features/game-page/types/board-cell";
import { GameSettings } from "@/constants/game-settings";

import { newBoard } from "@/features/game-page/actions/game-actions";

import { useGameStore } from "@/stores/game-store";

export const useGameActions = () => {
    const save = useGameStore((state) => state.save);
    const clear = useGameStore((state) => state.clear);

    const newGame = () => {
        let newGame: Board = newBoard();

        const lastCell: BoardCell = newGame.cells.reduce((prev, current) => {
            return (prev.columnNumber > current.columnNumber && prev.rowNumber > current.rowNumber) ? prev : current
        }, { columnNumber: GameSettings.BOARD_MAX_COLUMNS, rowNumber: GameSettings.BOARD_MAX_ROWS, isActive: false });

        save(newGame, lastCell.rowNumber, lastCell.columnNumber);
    }

    const finishGame = () => {
        clear();
    }

    return { newGame, finishGame }
}
