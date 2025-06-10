import { Routes } from '@angular/router';
import { ChatComponent } from './components/chat/chat.component';
import { TicTacToeComponent } from './components/tic-tac-toe/tic-tac-toe.component';

export const routes: Routes = [
  { path: 'chat', component: ChatComponent },
  { path: 'tictactoe', component: TicTacToeComponent },
  { path: '**', redirectTo: '' },
];
