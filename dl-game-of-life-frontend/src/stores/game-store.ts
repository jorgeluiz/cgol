import { create } from 'zustand';
import { GameState } from '@/types/game';

export const useGameStore = create<GameState>((set) => ({
    board: null,
    boardTotalRows: null,
    boardTotalColumns: null,
    save: (newBoard, boardTotalRows, boardTotalColumns) => set({ board: newBoard, boardTotalColumns: boardTotalColumns, boardTotalRows: boardTotalRows }),
    clear: () => set({ board: null, boardTotalColumns: null, boardTotalRows: null }),
}));