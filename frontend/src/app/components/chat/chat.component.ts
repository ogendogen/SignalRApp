import { Component, OnDestroy, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { Subject, takeUntil } from 'rxjs';
import { SignalRService } from '../../services/signalr.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-chat',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatListModule,
  ],
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss'],
})
export class ChatComponent implements OnInit, OnDestroy {
  chatForm: FormGroup;
  messages: string[] = [];
  username = 'User_' + Math.floor(Math.random() * 1000);
  private destroy$ = new Subject<void>();

  constructor(private fb: FormBuilder, private signalRService: SignalRService) {
    this.chatForm = this.fb.group({
      message: ['', Validators.required],
    });
  }

  ngOnInit(): void {
    // Connect to SignalR hub
    this.signalRService
      .startConnection('https://localhost:7171/chathub')
      .then(() => {
        console.log('Connection started');
        this.signalRService.addMessageListener();
      })
      .catch((err) => console.error('Error while starting connection: ' + err));

    // Subscribe to messages
    this.signalRService.messages$
      .pipe(takeUntil(this.destroy$))
      .subscribe((messages) => {
        this.messages = messages;
      });
  }

  sendMessage(): void {
    if (this.chatForm.valid) {
      const message = this.chatForm.get('message')?.value;
      this.signalRService
        .sendMessage(this.username, message)
        .then(() => {
          this.chatForm.reset();
        })
        .catch((err) => console.error('Error sending message: ' + err));
    }
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
    this.signalRService.stopConnection();
  }
}
