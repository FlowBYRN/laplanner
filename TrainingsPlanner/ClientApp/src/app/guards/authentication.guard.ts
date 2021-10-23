import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { UserClient } from 'src/clients/is4.generated.clients';
import { AuthorizationService } from '../services/authorization.service';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationGuard implements CanActivate {

  constructor(private authService: AuthorizationService,
    private router: Router) { }

  canActivate(): boolean {
    console.log("authguard", this.authService.isAuthenticated)
    return true;
  }

  // canActivate(): boolean {
  //   if (!this.authService.authenticated.toPromise()) {
  //     this.router.navigate([''])
  //     return false;
  //   }
  //   return true;
  // }

}
