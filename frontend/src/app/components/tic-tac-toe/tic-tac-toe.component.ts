import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';
import { FieldStatus } from '../../enums/field-status';
import { MatFormFieldModule, MatLabel } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { FormsModule } from '@angular/forms';
import { TicTacToeService } from '../../services/tictactoe.service';
import { LoginResponse } from '../../models/login/loginresponse';

@Component({
  selector: 'app-tic-tac-toe',
  standalone: true,
  imports: [
    CommonModule,
    MatButtonModule,
    MatFormFieldModule,
    MatLabel,
    MatInputModule,
    FormsModule,
  ],
  templateUrl: './tic-tac-toe.component.html',
  styleUrl: './tic-tac-toe.component.scss',
})
export class TicTacToeComponent {
  playerName = '';
  isLoggedIn = false;

  constructor(private ticTacToeService: TicTacToeService) {}

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

  onLogin() {
    const loginResponse = this.ticTacToeService
      .login(this.playerName)
      .then((response: LoginResponse) => {
        this.isLoggedIn = response.result;
      });
    console.log('Player name:', this.playerName);
    console.log(JSON.stringify(loginResponse));
  }
}
