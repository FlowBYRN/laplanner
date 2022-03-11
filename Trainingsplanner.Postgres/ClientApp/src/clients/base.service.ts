import { tokenize } from '@angular/compiler/src/ml_parser/lexer';
import { Injectable } from '@angular/core';
import { AuthorizeService } from '../api-authorization/authorize.service';

@Injectable({
  providedIn: 'root'
})
export class ClientBase {

  constructor(private authorizeService: AuthorizeService) {
  }

  public async transformOptions(options: any) {
    // TODO: Change options if required
    const token: string = await this.authorizeService.getAccessToken().toPromise();
    if (token) {
      options.headers = options.headers.append('authorization', 'Bearer ' + token);
    }
    else {

    }
    return Promise.resolve(options);
  }
}
