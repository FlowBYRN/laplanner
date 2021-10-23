import { OnInit } from '@angular/core';
import { Component } from '@angular/core';
import { RegistrationClient } from 'src/clients/is4.generated.clients';
import { AuthorizationService } from '../services/authorization.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {


  constructor(private authorizationService: AuthorizationService,
    private registrationClient: RegistrationClient) { }
  isAuthenticated: boolean = false;

  ngOnInit(): void {
    this.authorizationService.authenticated.subscribe(bool => {
      this.isAuthenticated = bool;
    });
  }

  login() {
    this.authorizationService.loginIfNeeded();
  }

  logout() {
    this.authorizationService.logout();
  }
}
