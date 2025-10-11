import { Board, BoardCell } from "@/features/game-page/types/board";
import { GameSettings } from "@/constants/game-settings";


import { newBoard } from "@/features/game-page/actions/game-actions";
import { createGame, deleteGame, getNextState, getBoardState, incrementState } from "@/services/game-service";

import { useGameStore } from "@/stores/game-store";

export const useGameActions = () => {
    const currentGame = useGameStore((state) => state.board);
    const save = useGameStore((state) => state.save);
    const clear = useGameStore((state) => state.clear);

    const newGame = async () => {

        let board: Board = newBoard();

        const createdGame = await createGame({ cells: board.cells });

        if (createdGame.id) {
            board.id = createdGame.id;

            const lastCell: BoardCell = board.cells.reduce((prev, current) => {
                return (prev.columnNumber > current.columnNumber && prev.rowNumber > current.rowNumber) ? prev : current
            }, { columnNumber: GameSettings.BOARD_MAX_COLUMNS, rowNumber: GameSettings.BOARD_MAX_ROWS, isAlive: false });

            save(board, lastCell.rowNumber, lastCell.columnNumber);
        }

    }

    const nextStage = async () => {
        if (currentGame?.id) {
            const nextStage = await getNextState(currentGame.id);
            if (nextStage.cells) {
                let newBoard: Board = { id: nextStage.id, cells: nextStage.cells };

                const lastCell: BoardCell = newBoard.cells.reduce((prev, current) => {
                    return (prev.columnNumber > current.columnNumber && prev.rowNumber > current.rowNumber) ? prev : current
                }, { columnNumber: GameSettings.BOARD_MAX_COLUMNS, rowNumber: GameSettings.BOARD_MAX_ROWS, isAlive: false });

                save(newBoard, lastCell.rowNumber, lastCell.columnNumber);
            }

        }

    }

    const finishGame = async () => {
        if (currentGame?.id) {
            await deleteGame(currentGame?.id);
            clear();
        }
    }


    return { newGame, nextStage, finishGame }
}
