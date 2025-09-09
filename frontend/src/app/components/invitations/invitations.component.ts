import { Component, Input, OnInit, OnChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { Invitation } from '../../models/invitation/invitation';

@Component({
  selector: 'app-invitations',
  standalone: true,
  imports: [CommonModule, MatTableModule, MatButtonModule],
  templateUrl: './invitations.component.html',
  styleUrl: './invitations.component.scss',
})
export class InvitationsComponent implements OnInit, OnChanges {
  @Input() invitations: Invitation[] = [];
  displayedColumns: string[] = ['message', 'actions'];
  dataSource = new MatTableDataSource<Invitation>([]);
  
  ngOnInit() {
    this.dataSource.data = this.invitations;
  }
  
  ngOnChanges() {
    this.dataSource.data = this.invitations;
  }
}
