import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class TicTacToeService {
  private hubConnection!: signalR.HubConnection;
  private messagesSubject = new BehaviorSubject<string[]>([]);
  public messages$ = this.messagesSubject.asObservable();

  constructor() {}

  public startConnection(hubUrl: string): Promise<void> {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(hubUrl, { withCredentials: false })
      .withAutomaticReconnect()
      .build();

    return this.hubConnection.start();
  }

  public startGame(player1: string, player2: string): Promise<any> {
    return this.hubConnection.invoke('StartGame', player1, player2);
  }

  public addMessageListener(): void {
    this.hubConnection.on('ReceiveMessage', (user: string, message: string) => {
      const currentMessages = this.messagesSubject.value;
      this.messagesSubject.next([...currentMessages, `${user}: ${message}`]);
    });
  }

  public sendMessage(user: string, message: string): Promise<void> {
    return this.hubConnection.invoke('SendMessage', user, message);
  }

  public stopConnection(): Promise<void> {
    return this.hubConnection.stop();
  }
}
