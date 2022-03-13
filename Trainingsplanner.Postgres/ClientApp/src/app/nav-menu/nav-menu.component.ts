import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { AppRoles, AuthorizeService } from '../../api-authorization/authorize.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit{
  isExpanded = false;
  public isAdmin: Observable<boolean>;
  public isAuthenticated: Observable<boolean>;
  public isTrainer: Observable<boolean>;

  constructor(private authorizeService: AuthorizeService) {}

  async ngOnInit() {
    this.isAdmin = this.authorizeService.isRole(AppRoles.Admin);
    this.isAuthenticated = this.authorizeService.isAuthenticated();
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
