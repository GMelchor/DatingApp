import { Component } from '@angular/core';
import { RegistrerComponent } from '../registrer/registrer.component';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RegistrerComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  registerMode = false;

  registerToggle(): void {
    this.registerMode = !this.registerMode;
  }

  cancelRegisterMode(event: boolean): void {
    this.registerMode = event;
  }
}