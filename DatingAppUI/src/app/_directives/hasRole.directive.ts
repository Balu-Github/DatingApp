import { Directive, Input, OnInit, TemplateRef, ViewContainerRef } from '@angular/core';
import { AuthService } from '../_services/auth.service';

@Directive({
  selector: '[appHasRole]' 
})
export class HasRoleDirective implements OnInit {

  @Input() appHasRole: string[];
  isViible = false;

  constructor(private viewContainerRef: ViewContainerRef,
                 private templateRef: TemplateRef<any>,
                 private authService: AuthService) { }

  ngOnInit() {
    const userRoles = this.authService.decodedToken.role as Array<string>;

    //if no roles clear the viewContainerRef
    if(!userRoles) {
      this.viewContainerRef.clear();
    }

    //if user has role need then render the element
    if(this.authService.roleMatch(this.appHasRole)) {
      if(!this.isViible) {
        this.isViible = true;
        this.viewContainerRef.createEmbeddedView(this.templateRef);
      } else {
        this.isViible = false;
        this.viewContainerRef.clear();
      }
    }
  }

}
