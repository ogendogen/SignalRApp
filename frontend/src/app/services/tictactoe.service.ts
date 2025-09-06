import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';
import { LoginRequest } from '../models/login/login.request';
import { LoginResponse } from '../models/login/login.response';
import { InviteRequest } from '../models/invite/invite.request';
import { InviteResponse } from '../models/invite/invite.response';
import { AcceptInviteRequest } from '../models/accept-invite/accept-invite.request';
import { Invitation } from '../models/invitation/invitation';
import { AcceptInviteResponse } from '../models/accept-invite/accept-invite.response';
import { InviteRejectedResponse } from '../models/invite-rejected/invite-rejected.response';

@Injectable({
  providedIn: 'root',
})
export class TicTacToeService {
  private hubConnection!: signalR.HubConnection;
  private messagesSubject = new BehaviorSubject<string[]>([]);
  private readonly hubUrl = 'https://localhost:7171/tictactoehub';
  public messages$ = this.messagesSubject.asObservable();

  constructor() {}

  public startConnection(): Promise<void> {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(this.hubUrl, { withCredentials: false })
      .withAutomaticReconnect()
      .build();

    return this.hubConnection.start();
  }

  public login(playerName: string): Promise<any> {
    const request: LoginRequest = { player: playerName };
    return this.hubConnection.invoke('Login', request);
  }

  public startGame(player1: string, player2: string): Promise<any> {
    return this.hubConnection.invoke('StartGame', player1, player2);
  }

  public addLoginListener(callback: (login: LoginResponse) => void): void {
    this.hubConnection.on('Login', callback);
  }

  public addInviteSentListener(
    callback: (invite: InviteResponse) => void
  ): void {
    this.hubConnection.on('InviteSent', callback);
  }

  public addInviteAcceptedListener(
    callback: (invite: AcceptInviteResponse) => void
  ): void {
    this.hubConnection.on('InviteAccepted', callback);
  }

  public addInviteRejectedListener(
    callback: (invite: InviteRejectedResponse) => void
  ): void {
    this.hubConnection.on('InviteRejected', callback);
  }

  public addInvitationListener(
    callback: (invitation: Invitation) => void
  ): void {
    this.hubConnection.on('Invitation', callback);
  }

  public stopConnection(): Promise<void> {
    return this.hubConnection.stop();
  }

  public invite(invitedPlayer: string): void {
    const request: InviteRequest = { invitedPlayer };
    this.hubConnection.invoke('Invite', request);
  }

  public acceptInvite(inviteId: string): void {
    const request: AcceptInviteRequest = { inviteId };
    this.hubConnection.invoke('AcceptInvite', request);
  }
}
