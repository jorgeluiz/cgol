"use client"
import { Board } from "@/features/game-page/components/board";

export default function GamePage() {
  return (
    <div className="grid items-center justify-items-center pt-20 pb-20">
      <main className="flex flex-col items-center sm:items-start">
        <Board />
      </main>
    </div>
  );
}

