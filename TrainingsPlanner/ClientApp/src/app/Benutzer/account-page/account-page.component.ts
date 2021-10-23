import { Component, OnInit } from '@angular/core';
import { User } from 'oidc-client';
import { AuthorizationService } from 'src/app/services/authorization.service';
import { ConfigurationService } from 'src/app/services/configuration.service';

@Component({
  selector: 'app-account-page',
  templateUrl: './account-page.component.html',
  styleUrls: ['./account-page.component.scss']
})
export class AccountPageComponent implements OnInit {


  user: User = null;

  constructor(private authorizationService: AuthorizationService,
    private config: ConfigurationService) { }

  async ngOnInit() {
    this.user = await this.authorizationService.getCurrentUser();
    if (!this.user || this.user.expired) {
      this.authorizationService.login();
    }
  }
  redirectToPasswordChange() {
    window.location.href = this.config.identityServerAddress + '/Account/ForgotPassword';
  }
}
