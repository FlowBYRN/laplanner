import { EventEmitter, Injectable } from '@angular/core';
import { User, UserManager } from 'oidc-client';
import { TrainingsGroupApplicationUserDto, TrainingsGroupUserClient } from 'src/clients/api.generated.clients';
import { ClientBase } from 'src/clients/base.service';
import { UserClient } from 'src/clients/is4.generated.clients';
import { ConfigurationService } from './configuration.service';

@Injectable({
  providedIn: 'root'
})
export class AuthorizationService {

  private userManager: UserManager;
  private token: string;
  public isAuthenticated: boolean;
  public authenticated: EventEmitter<boolean> = new EventEmitter<boolean>(true);
  constructor(private apiConfig: ConfigurationService) {
    this.createUserManager();
    this.authenticated.subscribe(bool => this.isAuthenticated = bool);
  }


  createUserManager() {
    const config = {
      authority: this.apiConfig.identityServerAddress,
      client_id: "spa",
      redirect_uri: this.apiConfig.apiAddress + `/callback`,
      response_type: "code",
      scope: this.apiConfig.identityServerScopes,
      post_logout_redirect_uri: this.apiConfig.apiAddress + `/index.html`,
      automaticSilentRenew: true,
      silent_redirect_uri: this.apiConfig.apiAddress + `/silentrenew.html`,
    };
    const um = new UserManager(config);
    um.events.addSilentRenewError(ev => console.log(ev));
    this.userManager = um;
  };

  login() {
    this.userManager.signinRedirect({ state: window.location.href });
  };

  async loginIfNeeded() {
    try {
      let user = await this.getCurrentUser();
      if (!user || user.expired) {
        this.login();
        user = await this.getCurrentUser();
        this.token = user.access_token;
        this.authenticated.emit(true);
        return user;
      }
      this.token = user.access_token
      this.authenticated.emit(true);
      return user;
    } catch (error) {
      this.login();
      return null;
    }
  };

  async getToken() {
    const user = await this.loginIfNeeded();
    if (user) {
      return user.access_token;
    }
    if (this.token) {
      return this.token;
    }
  }
  logout() {
    this.userManager.signoutRedirect({ state: window.location.href })
    this.authenticated.emit(false);
  }

  public async getCurrentUser(): Promise<User> {
    return await this.userManager.getUser();
  }

  public async getCurrentUserName(): Promise<string> {
    const user = await this.getCurrentUser();
    if (user)
      return user.profile.name;
    else
      return "";
  }
}


