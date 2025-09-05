import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';
import { LoginRequest } from '../models/login/loginrequest';
import { LoginResponse } from '../models/login/loginresponse';

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

  public stopConnection(): Promise<void> {
    return this.hubConnection.stop();
  }
}
