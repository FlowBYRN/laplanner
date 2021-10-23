import { Component, OnInit } from '@angular/core';
import { User, UserManager } from 'oidc-client';
import { AuthorizationService } from 'src/app/services/authorization.service';
import { RegisterViewModel, RegistrationClient } from 'src/clients/is4.generated.clients';

@Component({
  selector: 'app-user-dropdown',
  templateUrl: './user-dropdown.component.html',
  styleUrls: ['./user-dropdown.component.css']
})
export class UserDropdownComponent implements OnInit {
  public username: string = "User";

  isAuthenticated: boolean = false;
  constructor(
    private authorizationService: AuthorizationService,
    private registrationClient: RegistrationClient) { }

  async ngOnInit() {
    var tmpUsername = await this.authorizationService.getCurrentUserName();
    if (tmpUsername != "") {
      this.username = tmpUsername;
    }
    this.authorizationService.authenticated.subscribe(bool => {
      this.isAuthenticated = bool;
    });
  }


  async login() {
    this.authorizationService.loginIfNeeded();
    this.username = await this.authorizationService.getCurrentUserName();
    console.log("nav.bar-name: ", this.username);
  }

  logout() {
    this.authorizationService.logout();
  }


}
