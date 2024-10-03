import { Component, input, output } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-registrer',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './registrer.component.html',
  styleUrl: './registrer.component.css',
})
export class RegistrerComponent {
  usersFromHomeComponent = input.required<any>();
  cancelRegister = output<boolean>();
  model: any = {};

  register(): void {
    console.log(this.model);
  }

  cancel(): void {
    this.cancelRegister.emit(false);
  }
}
