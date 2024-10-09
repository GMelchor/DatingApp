import { Component, inject, input, output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-registrer',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './registrer.component.html',
  styleUrl: './registrer.component.css',
})
export class RegistrerComponent {
  private accountService = inject(AccountService);
  // usersFromHomeComponent = input.required<any>();
  private toastr = inject(ToastrService);
  cancelRegister = output<boolean>();
  model: any = {};

  register(): void {
    this.accountService.register(this.model).subscribe({
      next: (response) => {
        console.log(response);
        this.cancel();
      },
      error: (error) => {
        this.toastr.error(error.errors)
      }
    });
  }

  cancel(): void {
    this.cancelRegister.emit(false);
  }
}
