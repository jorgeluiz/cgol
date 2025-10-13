import { useRef, useEffect, useCallback } from "react";
import { Board, BoardCell } from "@/features/game-page/types/board";
import { GameSettings } from "@/constants/game-settings";
import { newBoard } from "@/features/game-page/actions/game-actions";
import { createGame, updateGame, deleteGame, getNextState, incrementState } from "@/services/game-service";
import { useGameStore } from "@/stores/game-store";

export const useGameActions = () => {

    const isAutoPlaying = useGameStore(state => state.isAutoPlaying);
    const setIsAutoPlaying = useGameStore(state => state.setIsAutoPlaying);
    const clear = useGameStore(state => state.clear);
    const save = useGameStore(state => state.save);

    const nextState = useCallback(async () => {
        const board = useGameStore.getState().board;
        const setIsBoardLocked = useGameStore.getState().setIsBoardLocked;

        if (!board?.id || useGameStore.getState().isBoardLocked) return;

        setIsBoardLocked(true);
        try {
            const nextStageData = await getNextState(board.id);
            if (nextStageData?.cells) {
                const newBoard: Board = { id: nextStageData.id, cells: nextStageData.cells };
                save(newBoard, GameSettings.BOARD_MAX_ROWS, GameSettings.BOARD_MAX_COLUMNS);
            }
        } catch (error) {
            console.error("Erro ao buscar próximo estado:", error);
            setIsAutoPlaying(false);
        } finally {
            setIsBoardLocked(false);
        }
    }, [save, setIsAutoPlaying]);


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
            console.error("Erro ao buscar próximo estado:", error);
            setIsAutoPlaying(false);
        } finally {
            setIsBoardLocked(false);
        }
    }, [save, setIsAutoPlaying]);


    const latestNextState = useRef(nextState);
    useEffect(() => {
        latestNextState.current = nextState;
    }, [nextState]);

    useEffect(() => {
        if (!isAutoPlaying) {
            return;
        }
        const interval = setInterval(() => {
            latestNextState.current();
        }, 1000);
        return () => clearInterval(interval);
    }, [isAutoPlaying]);

    const startAutoPlay = useCallback(() => setIsAutoPlaying(true), [setIsAutoPlaying]);
    const stopAutoPlay = useCallback(() => setIsAutoPlaying(false), [setIsAutoPlaying]);

    const newGame = useCallback(async () => {
        let board: Board = newBoard();
        const createdGame = await createGame({ cells: board.cells });
        if (createdGame.id) {
            board.id = createdGame.id;
            const lastCell: BoardCell = board.cells.reduce((prev, current) => {
                return (prev.columnNumber > current.columnNumber && prev.rowNumber > current.rowNumber) ? prev : current
            }, { columnNumber: GameSettings.BOARD_MAX_COLUMNS, rowNumber: GameSettings.BOARD_MAX_ROWS, isAlive: false });
            save(board, lastCell.rowNumber, lastCell.columnNumber);
        }
    }, [save]);

    const saveCurrentState = useCallback(async () => {
        const currentGame = useGameStore.getState().board;
        if (currentGame?.id) {
            await updateGame({ id: currentGame.id, cells: currentGame.cells });
        }
    }, []);

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