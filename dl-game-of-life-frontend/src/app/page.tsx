import { renderRows } from "@/features/game-page/actions/game-page-actions";

export default function GamePage() {
  return (
    <div className="grid items-center justify-items-center pt-20 pb-20">
      <main className="flex flex-col items-center sm:items-start">
        <div className="board-container">
          {renderRows(50)}
        </div>
      </main>
    </div>
  );
}

