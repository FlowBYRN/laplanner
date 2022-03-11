import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ConfigurationService {

  private configuration: IServerConfiguration;
  constructor(private http: HttpClient) { }
  loadConfig() {
    return this.http.get<IServerConfiguration>('/api/v1/configuration')
      .toPromise()
      .then(result => {
        this.configuration = <IServerConfiguration>(result);
        console.log(this.configuration);
      }, error => console.error(error));
  }
  get apiAddress() {
    return this.configuration.ApiBaseUrl;
  }
  //get identityServerScopes() {
  //  return this.configuration.IdentityServerScopes;
  //}

}
export interface IServerConfiguration {
  ApiBaseUrl: string;
//IdentityServerScopes: string;
}
