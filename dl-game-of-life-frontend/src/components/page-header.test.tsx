import { jest, describe, it, expect, afterEach, beforeEach } from '@jest/globals';
import { render, screen, cleanup } from '@testing-library/react';

import { useGameStore } from '@/stores/game-store';

import Header from '@/components/page-header';

const initialStoreState = useGameStore.getState();

describe('Header Component', () => {
    // This hook cleans up the environment AFTER each test to ensure full isolation.
    afterEach(() => {
        cleanup(); // Unmount the component.
        jest.clearAllMocks(); // Clear the call history of ALL mocks.
    });

    describe('When no game is active', () => {
        beforeEach(() => {
            useGameStore.setState(initialStoreState, true);
            useGameStore.setState({
                board: { id: null, cells: [] },
                isBoardLocked: false,
                isAutoPlaying: false,
            });
            render(<Header />);
        });

        it('should render the title', () => {
            expect(screen.getByText('Game Of Life')).toBeInTheDocument();
        });

        it('should show the "New game" button', () => {
            expect(screen.getByText('New game')).toBeInTheDocument();
        });

        it('should not show game control buttons', () => {
            expect(screen.queryByText('Jump states')).not.toBeInTheDocument();
        });

    });

    describe('When a game is active', () => {
        beforeEach(() => {

            useGameStore.setState(initialStoreState, true);
            useGameStore.setState({
                board: { id: "id-123", cells: [] },
                isBoardLocked: false,
                isAutoPlaying: false,
            });
            render(<Header />);
        });

        it('should not show the "New game" button', () => {
            expect(screen.queryByText('New game')).not.toBeInTheDocument();
        });

        it('should show all game control buttons', () => {
            expect(screen.getByText('Jump states')).toBeInTheDocument();
            expect(screen.getByText('Next state')).toBeInTheDocument();
        });

    });

    describe('When auto-playing', () => {
        beforeEach(() => {

            useGameStore.setState(initialStoreState, true);
            useGameStore.setState({
                board: { id: "id-123", cells: [] },
                isBoardLocked: true,
                isAutoPlaying: true,
            });
            render(<Header />);
        });

        it('should show "Stop Auto" button text', () => {
            expect(screen.getByText('Stop Auto')).toBeInTheDocument();
        });

    });
});
