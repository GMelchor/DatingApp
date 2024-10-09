import { NgFor } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavComponent } from "./nav/nav.component";
import { AccountService } from './_services/account.service';
 
@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, NgFor, NavComponent],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'] // Corrección: styleUrls en plural
})
export class AppComponent implements OnInit {
  http = inject(HttpClient);
  private accountService = inject(AccountService);
  title = 'Date me';
  users: any;

  ngOnInit(): void {
    this.getUsers(); 
    this.setCureentUser();
  }

  setCureentUser(){
    const userString = localStorage.getItem("user");
      if(!userString) return;
      const user = JSON.parse(userString);
      this.accountService.currentUser.set(user);
  }
 
  getUsers(): void {
    this.http.get("https://localhost:5001/api/users").subscribe({
      next: response => this.users = response,
      error: error => console.log(error),
      complete: () => console.log("Request completed")
    });
  }
 
}