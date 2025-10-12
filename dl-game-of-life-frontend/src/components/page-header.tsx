"use client"
import { useGameStore } from "@/stores/game-store"

import { useGameActions } from "@/hooks/game-hooks";

export default function Header() {
    const { newGame, finishGame, nextStage } = useGameActions();

    const board = useGameStore((state) => state.board);
    const isBoardLocked = useGameStore((state) => state.isBoardLocked);
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
                                    <button className="block py-2 px-3 text-gray-900 rounded-sm hover:bg-gray-100 md:hover:bg-transparent md:border-0 md:hover:text-blue-700 md:p-0 dark:text-white md:dark:hover:text-blue-500 dark:hover:bg-gray-700 dark:hover:text-white md:dark:hover:bg-transparent" onClick={nextStage} disabled={isBoardLocked}>Next state</button>
                                </li>
                                <li>
                                    <button className="block py-2 px-3 text-gray-900 rounded-sm hover:bg-gray-100 md:hover:bg-transparent md:border-0 md:hover:text-blue-700 md:p-0 dark:text-white md:dark:hover:text-blue-500 dark:hover:bg-gray-700 dark:hover:text-white md:dark:hover:bg-transparent" disabled={isBoardLocked}>Start Auto</button>
                                </li>
                                <li>
                                    <button className="block py-2 px-3 text-gray-900 rounded-sm hover:bg-gray-100 md:hover:bg-transparent md:border-0 md:hover:text-blue-700 md:p-0 dark:text-white md:dark:hover:text-blue-500 dark:hover:bg-gray-700 dark:hover:text-white md:dark:hover:bg-transparent" onClick={finishGame} disabled={isBoardLocked}>Clear board</button>
                                </li>
                            </>
                        )
                    }

                </ul>
            </div>
        </div>
    </nav>
}