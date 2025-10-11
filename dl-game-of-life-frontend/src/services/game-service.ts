import { BoardRequest, BoardResponse } from "@/features/game-page/types/board";
import { post, get, deleteRequest } from "@/utils/api";


//API Controller
const baseRoute: string = "/GameOfLife";


//Endpoints
/**
 * Create a new board
 * POST 
 */
export const createGame = (initialState: BoardRequest): Promise<BoardResponse> => {
  return post<BoardResponse, BoardRequest>(baseRoute, initialState);
};

/**
 * Load board current state
 * GET /{boardId}
 */
export const getBoardState = (boardId: string): Promise<BoardResponse> => {
  return get<BoardResponse>(`${baseRoute}/${boardId}`);
};

/**
 * Delete a game
 * DELETE /{boardId}
 */
export const deleteGame = (boardId: string): Promise<number> => {
  return deleteRequest<number>(`${baseRoute}/${boardId}`);
};

/**
 * Go to the next stage.
 * GET /next_state/{boardId}
 */
export const getNextState = (boardId: string): Promise<BoardResponse> => {
  return get<BoardResponse>(`${baseRoute}/next_state/${boardId}`);
};

/**
 * Keep moving trought the states based in the state increment requested
 * GET /increment_state/{boardId}/{statesToIncrement}
 */
export const incrementState = (boardId: string, statesToIncrement: number): Promise<BoardResponse> => {
  return get<BoardResponse>(`${baseRoute}/increment_state/${boardId}/${statesToIncrement}`);
};