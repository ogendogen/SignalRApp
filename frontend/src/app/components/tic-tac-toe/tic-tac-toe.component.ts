import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';
import { FieldStatus } from '../../enums/field-status';

@Component({
  selector: 'app-tic-tac-toe',
  standalone: true,
  imports: [CommonModule, MatButtonModule],
  templateUrl: './tic-tac-toe.component.html',
  styleUrl: './tic-tac-toe.component.scss',
})
export class TicTacToeComponent {
  board: FieldStatus[][] = [
    [FieldStatus.Empty, FieldStatus.Empty, FieldStatus.Empty],
    [FieldStatus.Empty, FieldStatus.Empty, FieldStatus.Empty],
    [FieldStatus.Empty, FieldStatus.Empty, FieldStatus.Empty],
  ];

  IsX(x: number, y: number): boolean {
    return this.board[x][y] == FieldStatus.X;
  }

  IsCircle(x: number, y: number): boolean {
    return this.board[x][y] == FieldStatus.Circle;
  }
}
