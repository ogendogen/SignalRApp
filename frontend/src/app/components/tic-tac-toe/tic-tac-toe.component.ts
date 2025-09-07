import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';
import { FieldStatus } from '../../enums/field-status';
import { MatFormFieldModule, MatLabel } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { FormsModule } from '@angular/forms';
import { TicTacToeService } from '../../services/tictactoe.service';
import { LoginResponse } from '../../models/login/login.response';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { InviteResponse } from '../../models/invite/invite.response';
import { Invitation } from '../../models/invitation/invitation';
import { AcceptInviteResponse } from '../../models/accept-invite/accept-invite.response';
import { InviteRejectedResponse } from '../../models/invite-rejected/invite-rejected.response';
import { GameStarted } from '../../models/game-started/game-started';

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
    MatSnackBarModule,
  ],
  templateUrl: './tic-tac-toe.component.html',
  styleUrl: './tic-tac-toe.component.scss',
})
export class TicTacToeComponent implements OnInit {
  inputPlayerName = '';
  playerName = '';
  invitedPlayer = '';
  isLoggedIn = false;

  constructor(
    private ticTacToeService: TicTacToeService,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.ticTacToeService.startConnection();
    this.ticTacToeService.addLoginListener((response: LoginResponse) => {
      this.loginListener(response);
    });
    this.ticTacToeService.addInviteSentListener((response: InviteResponse) => {
      this.inviteSentListener(response);
    });
    this.ticTacToeService.addInvitationListener((invitation: Invitation) => {
      this.invitationListener(invitation);
    });
    this.ticTacToeService.addInviteAcceptedListener(
      (response: AcceptInviteResponse) => {
        this.inviteAcceptedListener(response);
      }
    );
    this.ticTacToeService.addInviteRejectedListener(
      (response: InviteRejectedResponse) => {
        this.inviteRejectedListener(response);
      }
    );
    this.ticTacToeService.addGameStartedListener((gameStarted: GameStarted) => {
      this.addGameStartedListener(gameStarted);
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
    }
  }

  onInvite(): void {
    if (!this.isLoggedIn) {
      this.snackBar.open('You must be logged in to invite a player', 'OK', {
        duration: 3000,
      });
      return;
    }

    if (!this.invitedPlayer) {
      this.snackBar.open('Please enter a player name to invite', 'OK', {
        duration: 3000,
      });
      return;
    }

    this.ticTacToeService.invite(this.invitedPlayer);
  }

  inviteSentListener(response: InviteResponse): void {
    if (response.invitationResult) {
      this.snackBar.open('Invite sent', 'OK', {
        duration: 3000,
      });
    } else {
      this.snackBar.open('Invite not sent: ' + response.error, 'OK', {
        duration: 3000,
      });
    }
  }

  invitationListener(invitation: Invitation): void {
    // todo: show invitations list with extra panel
    this.snackBar
      .open('You have been invited to play by ' + invitation.from, 'Accept', {
        duration: 10000,
      })
      .onAction()
      .subscribe(() => {
        this.ticTacToeService.acceptInvite(invitation.inviteId);
      });
  }

  inviteAcceptedListener(response: AcceptInviteResponse): void {
    if (response.acceptInviteResult) {
      this.snackBar.open(String(response.message), 'OK', {
        duration: 3000,
      });
    } else {
      this.snackBar.open(
        'Invite error: ' + String(response.errorMessage),
        'OK',
        {
          duration: 3000,
        }
      );
    }
  }

  inviteRejectedListener(response: InviteRejectedResponse): void {
    if (response.rejectInviteResult) {
      this.snackBar.open(response.message, 'OK', {
        duration: 3000,
      });
    } else {
      this.snackBar.open('Invite rejection error: ' + response.message, 'OK', {
        duration: 3000,
      });
    }
  }

  addGameStartedListener(response: GameStarted): void {
    this.snackBar.open('Game Started! ID: ' + response.gameId, 'OK', {
      duration: 3000,
    });
    // if (response.gameStarted) {
    //   this.snackBar.open(response.message, 'OK', {
    //     duration: 3000,
    //   });
    // } else {
    //   this.snackBar.open('Game start error: ' + response.message, 'OK', {
    //     duration: 3000,
    //   });
    // }
  }
}
