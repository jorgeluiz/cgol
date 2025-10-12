import { create } from 'zustand';
import { GameState } from '@/types/game';
import { BoardCell } from '@/features/game-page/types/board';

export const handleSavecell = (
    cell: BoardCell,
    setter: {
        (partial: GameState | Partial<GameState> | ((state: GameState) => GameState | Partial<GameState>), replace?: false): void;
        (state: GameState | ((state: GameState) => GameState), replace: true): void;
    }) => {

    setter(state => {
        if (!state.board) return {};

        const newCells = state.board.cells.map(c =>
            (c.rowNumber === cell.rowNumber && c.columnNumber === cell.columnNumber) ? cell : c
        );

        return {
            board: {
                ...state.board,
                cells: newCells,
            }
        };
    });
}

export const useGameStore = create<GameState>((set) => ({
    board: null,
    boardTotalRows: null,
    boardTotalColumns: null,
    isBoardLocked: false,
    areActionsLocked: false,
    save: (newBoard, boardTotalRows, boardTotalColumns) => set({ board: newBoard, boardTotalColumns: boardTotalColumns, boardTotalRows: boardTotalRows }),
    saveBoardCell: (cell: BoardCell) => handleSavecell(cell, set),
    setIsBoardLocked: (isBoardLocked: boolean) => set({ isBoardLocked: isBoardLocked }),
    clear: () => set({ board: null, boardTotalColumns: null, boardTotalRows: null, isBoardLocked: false }),
}));