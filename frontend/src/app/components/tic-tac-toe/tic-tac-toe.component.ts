import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
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
export class TicTacToeComponent implements OnInit {
  inputPlayerName = '';
  playerName = '';
  isLoggedIn = false;

  constructor(
    private ticTacToeService: TicTacToeService,
    private changeDetectorRef: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.ticTacToeService.startConnection();
    this.ticTacToeService.addLoginListener((response: LoginResponse) => {
      this.loginListener(response);
    });
  }

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

  onLogin(): void {
    this.ticTacToeService.login(this.inputPlayerName);
  }

  loginListener(response: LoginResponse): void {
    if (response.result) {
      this.isLoggedIn = true;
      this.playerName = this.inputPlayerName;
      this.changeDetectorRef.markForCheck();
    }
  }
}
