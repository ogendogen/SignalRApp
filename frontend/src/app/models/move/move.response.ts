import { MovementResult } from './movement-result.enum';

export interface MoveResponse {
  error: string;
  isSuccess: boolean;
  movementResult: MovementResult;
  playerName: string;
  x: number;
  y: number;
}
