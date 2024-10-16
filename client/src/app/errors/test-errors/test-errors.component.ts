import { HttpClient } from '@angular/common/http';
import { Component, inject } from '@angular/core';

@Component({
  selector: 'app-test-errors',
  standalone: true,
  imports: [],
  templateUrl: './test-errors.component.html',
  styleUrl: './test-errors.component.css'
})
export class TestErrorsComponent {
  baseUrl = "https://localhost:5001/api/";
  private http = inject(HttpClient);
  validationErrors: string[] = [];

  get400error(): void{
    this.http.get(this.baseUrl + "buggy/bad-requets").subscribe({
      next: (response) => console.log(response),
      error: (error) => console.log(error)
    })
  }

  get401error(): void{
    this.http.get(this.baseUrl + "buggy/auth").subscribe({
      next: (response) => console.log(response),
      error: (error) => console.log(error)
    })
  }

  get404error(): void{
    this.http.get(this.baseUrl + "buggy/not-found").subscribe({
      next: (response) => console.log(response),
      error: (error) => console.log(error)
    })
  }

  get500error(): void{
    this.http.get(this.baseUrl + "buggy/server-error").subscribe({
      next: (response) => console.log(response),
      error: (error) => console.log(error)
    })
  }

  get400ValidationError(): void{
    this.http.post(this.baseUrl + "account/registrer",{}).subscribe({
      next: (response) => console.log(response),
      error: (error) => {
        console.log(error);
        this.validationErrors = error;
      }
    })
  }
}
