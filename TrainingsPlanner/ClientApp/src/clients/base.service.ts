import { tokenize } from '@angular/compiler/src/ml_parser/lexer';
import { Injectable } from '@angular/core';
import { AuthorizationService } from 'src/app/services/authorization.service';

@Injectable({
  providedIn: 'root'
})
export class ClientBase {

  constructor(private authorizationService: AuthorizationService) {
  }

  public async transformOptions(options: any) {
    // TODO: Change options if required
    const token: string = await this.authorizationService.getToken();
    if (token) {
      options.headers = options.headers.append('authorization', 'Bearer ' + await this.authorizationService.getToken());
    }
    else {

    }
    return Promise.resolve(options);
  }
}
