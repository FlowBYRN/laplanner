import { ChangeDetectorRef, Component, Directive, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { RegisterViewModel, RegistrationClient, UserClient } from 'src/clients/is4.generated.clients';
import { AuthorizationService } from '../services/authorization.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.scss']
})
export class NavMenuComponent implements OnInit {
  isExpanded = false;
  isAuthenticated: boolean = false;

  constructor(private authService: AuthorizationService, private cdRef: ChangeDetectorRef, private userClient: UserClient,
    private router: Router) { }

  async ngOnInit() {
    this.authService.authenticated.subscribe(bool => {
      this.isAuthenticated = bool;
      this.cdRef.detectChanges();
    });
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
