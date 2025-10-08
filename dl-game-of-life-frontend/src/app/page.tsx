import BoardCell from "@/_components/board-cell";
import { ReactNode } from "react";

const renderCells = (rowNumber: number, cellsNumber: number): ReactNode => {
  return (
    <>
      {Array.from({ length: cellsNumber }, (_, cellIndex) => (
        <BoardCell key={`${rowNumber}-${cellIndex}`} rowNumber={rowNumber} cellIndex={cellIndex} />
      ))}
    </>
  );
}

const renderRows = (rowNumber: number): ReactNode => {
  return (
    <>
      {Array.from({ length: rowNumber }, (_, index) => (
        <div key={index} className="board-row">
          {renderCells(index, 50)}
        </div>
      ))}
    </>
  );
}

export default function Home() {
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

