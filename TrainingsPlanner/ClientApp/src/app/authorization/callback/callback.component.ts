import { Component, OnInit } from '@angular/core';
import * as Oidc from 'oidc-client';
import { AuthorizationService } from 'src/app/services/authorization.service';

@Component({
  selector: 'app-callback',
  templateUrl: './callback.component.html',
  styleUrls: ['./callback.component.css']
})
export class CallbackComponent implements OnInit {

  constructor(private authService: AuthorizationService) {
    new Oidc.UserManager({ response_mode: "query" }).signinRedirectCallback().then(function (user) {
      window.location = user.state || "/";
      authService.authenticated.emit(true);
    }).catch(function (e) {
      console.error(e);
    });
  }

  ngOnInit(): void {
  }

}
