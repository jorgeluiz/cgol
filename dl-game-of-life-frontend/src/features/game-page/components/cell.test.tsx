import { jest, describe, it, expect, beforeEach, afterEach } from '@jest/globals';
import { render, fireEvent, cleanup, act } from '@testing-library/react';
import Cell from './cell';
import { useGameStore } from '@/stores/game-store';

const initialStoreState = useGameStore.getState();

describe('Cell Component', () => {
    // Create mocks for the functions we expect to be called.
    const mockSaveBoardCell = jest.fn();
    const mockSetIsBoardLocked = jest.fn();
    const mockSaveCurrentState = jest.fn(() => Promise.resolve()); // Simulate an async function

    beforeEach(() => {
        // Clear all mocks before each test to ensure isolation.
        jest.clearAllMocks();

        useGameStore.setState(initialStoreState, true);
        useGameStore.setState({
            board: { id: null, cells: [{ rowNumber: 0, columnNumber: 0, isAlive: true }] },
        });
    });

    afterEach(cleanup); // Clean up the virtual DOM after each test.

    it('should render with "active" class when the cell is alive', () => {
        const { container } = render(<Cell rowNumber={0} columnNumber={0} />);

        // Check if the 'active' class is present on the cell element.
        expect(container.firstChild).toHaveClass('board-cell active');
    });

    it('should render without "active" class when the cell is dead', () => {
        const { container } = render(<Cell rowNumber={1} columnNumber={1} />);
        expect(container.firstChild).not.toHaveClass('active');
    });

    describe('when clicked', () => {
        beforeEach(() => {
            // Clear all mocks before each test to ensure isolation.
            jest.clearAllMocks();

            useGameStore.setState(initialStoreState, true);
            useGameStore.setState({
                board: { id: null, cells: [{ rowNumber: 0, columnNumber: 0, isAlive: false }] },
                isBoardLocked: false
            });
        });

        it('should do nothing if the board is already locked', async () => {
            const { container } = render(<Cell rowNumber={0} columnNumber={0} />);
            const cellElement = container.firstChild as HTMLElement;

            await act(async () => {
                fireEvent.click(cellElement);
            });

            // None of the state-changing functions should have been called.
            expect(mockSetIsBoardLocked).not.toHaveBeenCalled();
            expect(mockSaveBoardCell).not.toHaveBeenCalled();
            expect(mockSaveCurrentState).not.toHaveBeenCalled();
        });
    });
});
