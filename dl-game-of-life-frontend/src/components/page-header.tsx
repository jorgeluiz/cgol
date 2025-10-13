"use client"

import { useState } from "react";
import { useGameStore } from "@/stores/game-store"

import { useGameActions } from "@/hooks/game-hooks";

export default function Header() {
    const { newGame, finishGame, nextState, startAutoPlay, stopAutoPlay, fastForward } = useGameActions();

    const [stateIncrement, setStateIncrement] = useState('1');
    const board = useGameStore((state) => state.board);
    const isBoardLocked = useGameStore((state) => state.isBoardLocked);
    const isAutoPlaying = useGameStore((state) => state.isAutoPlaying);

    const handleStateIncrement = (evt: React.ChangeEvent<HTMLInputElement>) => {
        setStateIncrement(evt.target.value);
    };

    const handleIncrement = () => {
        const forwardCount = parseInt(stateIncrement) || 1;
        fastForward(forwardCount);
    }

    return <nav className="bg-white border-gray-200 dark:bg-gray-900">
        <div className="max-w-screen-xl flex flex-wrap items-center justify-between mx-auto p-4">
            <a href="#" className="flex items-center space-x-3 rtl:space-x-reverse">
                <span className="self-center text-2xl font-semibold whitespace-nowrap dark:text-white">Game Of Life</span>
            </a>
            <div className="hidden w-full md:block md:w-auto" id="navbar-default">
                <ul className="font-medium flex flex-col p-4 md:p-0 mt-4 border border-gray-100 rounded-lg bg-gray-50 md:flex-row md:space-x-8 rtl:space-x-reverse md:mt-0 md:border-0 md:bg-white dark:bg-gray-800 md:dark:bg-gray-900 dark:border-gray-700">
                    {
                        board?.id == null && (
                            <li>
                                <button className="block py-2 px-3 text-gray-900 rounded-sm hover:bg-gray-100 md:hover:bg-transparent md:border-0 md:hover:text-blue-700 md:p-0 dark:text-white md:dark:hover:text-blue-500 dark:hover:bg-gray-700 dark:hover:text-white md:dark:hover:bg-transparent" onClick={newGame}>New game</button>
                            </li>
                        )
                    }
                    {
                        board?.id != null && (
                            <>
                                <li>
                                    <div className="flex items-center space-x-2">
                                        <input
                                            type="number"
                                            min="0"
                                            value={stateIncrement || undefined}
                                            className="w-16 text-center border border-gray-300 rounded py-2 px-3 focus:outline-none focus:ring-2 focus:ring-blue-500"
                                            onChange={handleStateIncrement}
                                        />
                                        <button
                                            className="block py-2 px-3 text-gray-900 rounded-sm hover:bg-gray-100 md:hover:bg-transparent md:border-0 md:hover:text-blue-700 md:p-0 dark:text-white md:dark:hover:text-blue-500 dark:hover:bg-gray-700 dark:hover:text-white md:dark:hover:bg-transparent"
                                            onClick={handleIncrement}
                                        >
                                            Jump states
                                        </button>
                                    </div>
                                </li>
                                <li>
                                    <button className="mt-2 block py-2 px-3 text-gray-900 rounded-sm hover:bg-gray-100 md:hover:bg-transparent md:border-0 md:hover:text-blue-700 md:p-0 dark:text-white md:dark:hover:text-blue-500 dark:hover:bg-gray-700 dark:hover:text-white md:dark:hover:bg-transparent" onClick={nextState} disabled={isBoardLocked || isAutoPlaying}>Next state</button>
                                </li>
                                <li>
                                    <button className="mt-2 block py-2 px-3 text-gray-900 rounded-sm hover:bg-gray-100 md:hover:bg-transparent md:border-0 md:hover:text-blue-700 md:p-0 dark:text-white md:dark:hover:text-blue-500 dark:hover:bg-gray-700 dark:hover:text-white md:dark:hover:bg-transparent" disabled={isBoardLocked} onClick={isAutoPlaying ? stopAutoPlay : startAutoPlay}> {isAutoPlaying ? 'Stop Auto' : 'Start Auto'}</button>
                                </li>
                                <li>
                                    <button className="mt-2 block py-2 px-3 text-gray-900 rounded-sm hover:bg-gray-100 md:hover:bg-transparent md:border-0 md:hover:text-blue-700 md:p-0 dark:text-white md:dark:hover:text-blue-500 dark:hover:bg-gray-700 dark:hover:text-white md:dark:hover:bg-transparent" onClick={finishGame} disabled={isBoardLocked || isAutoPlaying}>Clear board</button>
                                </li>
                            </>
                        )
                    }
                </ul>
            </div>
        </div>
    </nav>
}