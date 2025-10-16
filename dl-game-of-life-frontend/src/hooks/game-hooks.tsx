import { useRef, useEffect, useCallback } from "react";
import { Board, BoardCell } from "@/features/game-page/types/board";
import { GameSettings } from "@/constants/game-settings";
import { newBoard } from "@/features/game-page/actions/game-actions";
import { createGame, updateGame, deleteGame, getNextState, incrementState } from "@/services/game-service";
import { useGameStore } from "@/stores/game-store";

export const useGameActions = () => {

    // Retrieve necessary state and setters from the global store.
    const isAutoPlaying = useGameStore(state => state.isAutoPlaying);
    const setIsAutoPlaying = useGameStore(state => state.setIsAutoPlaying);
    const clear = useGameStore(state => state.clear);
    const save = useGameStore(state => state.save);

    /**
     * Advances the game by one state.
     * Locks the board during the update to prevent race conditions.
     */
    const nextState = useCallback(async () => {
        const board = useGameStore.getState().board;
        const setIsBoardLocked = useGameStore.getState().setIsBoardLocked;

        // Do nothing if no board exists or board is already locked.
        if (!board?.id || useGameStore.getState().isBoardLocked) return;

        setIsBoardLocked(true);
        try {
            const nextStageData = await getNextState(board.id);
            if (nextStageData?.cells) {
                const newBoard: Board = { id: nextStageData.id, cells: nextStageData.cells };
                save(newBoard, GameSettings.BOARD_MAX_ROWS, GameSettings.BOARD_MAX_COLUMNS);
            }
        } catch (error) {
            console.error("Error fetching next state:", error);
            setIsAutoPlaying(false); // Stop autoplay if an error occurs
        } finally {
            setIsBoardLocked(false); // Always unlock the board
        }
    }, [save, setIsAutoPlaying]);

    /**
     * Advances the game by multiple states at once (fast forward).
     * Similar to nextState but allows incrementing by a given number.
     */
    const fastForward = useCallback(async (increment: number) => {
        const board = useGameStore.getState().board;
        const setIsBoardLocked = useGameStore.getState().setIsBoardLocked;

        if (!board?.id || useGameStore.getState().isBoardLocked) return;

        setIsBoardLocked(true);
        try {
            const nextStageData = await incrementState(board.id, increment);
            if (nextStageData?.cells) {
                const newBoard: Board = { id: nextStageData.id, cells: nextStageData.cells };
                save(newBoard, GameSettings.BOARD_MAX_ROWS, GameSettings.BOARD_MAX_COLUMNS);
            }
        } catch (error) {
            console.error("Error fetching next state:", error);
            setIsAutoPlaying(false);
        } finally {
            setIsBoardLocked(false);
        }
    }, [save, setIsAutoPlaying]);

    // Keep a ref to the latest nextState callback to use inside setInterval.
    const latestNextState = useRef(nextState);
    useEffect(() => {
        latestNextState.current = nextState;
    }, [nextState]);

    /**
     * Automatically advances the game at a fixed interval when autoplay is active.
     */
    useEffect(() => {
        if (!isAutoPlaying) {
            return;
        }
        const interval = setInterval(() => {
            latestNextState.current();
        }, 1000); // Update every 1 second
        return () => clearInterval(interval);
    }, [isAutoPlaying]);

    // Start and stop auto-playing
    const startAutoPlay = useCallback(() => setIsAutoPlaying(true), [setIsAutoPlaying]);
    const stopAutoPlay = useCallback(() => setIsAutoPlaying(false), [setIsAutoPlaying]);

    /**
     * Starts a new game.
     * Creates a new board and saves it to the store and backend.
     */
    const newGame = useCallback(async () => {
        const board: Board = newBoard();
        const createdGame = await createGame({ cells: board.cells });
        if (createdGame.id) {
            board.id = createdGame.id;
            // Determine the last cell to define board dimensions dynamically
            const lastCell: BoardCell = board.cells.reduce((prev, current) => {
                return (prev.columnNumber > current.columnNumber && prev.rowNumber > current.rowNumber) ? prev : current
            }, { columnNumber: GameSettings.BOARD_MAX_COLUMNS, rowNumber: GameSettings.BOARD_MAX_ROWS, isAlive: false });
            save(board, lastCell.rowNumber, lastCell.columnNumber);
        }
    }, [save]);

    /**
     * Saves the current board state to the backend.
     */
    const saveCurrentState = useCallback(async () => {
        const currentGame = useGameStore.getState().board;
        if (currentGame?.id) {
            await updateGame({ id: currentGame.id, cells: currentGame.cells });
        }
    }, []);

    /**
     * Finishes the current game by stopping autoplay, deleting from backend, and clearing the store.
     */
    const finishGame = useCallback(async () => {
        stopAutoPlay();
        const currentGame = useGameStore.getState().board;
        if (currentGame?.id) {
            await deleteGame(currentGame.id);
            clear();
        }
    }, [clear, stopAutoPlay]);

    return {
        newGame,
        saveCurrentState,
        nextState,
        fastForward,
        finishGame,
        startAutoPlay,
        stopAutoPlay
    };
};
