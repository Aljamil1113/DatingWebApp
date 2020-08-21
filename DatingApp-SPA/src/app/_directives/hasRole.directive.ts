import { AuthService } from 'src/app/_services/auth.service';
import { Directive, ViewContainerRef, TemplateRef, Input, OnInit } from '@angular/core';

@Directive({
  selector: '[appHasRole]'
})
export class HasRoleDirective implements OnInit{
  @Input() appHasRole: string[];
  isInvisible = false;

  constructor(private viewContainerRef: ViewContainerRef, private templateRef: TemplateRef<any>,
              private authService: AuthService) { }

  ngOnInit(){
    const userRoles = this.authService.decodedToken.role as Array<string>;
    // if no roles clear the viewContainerRef
    if(!userRoles) {
      this.viewContainerRef.clear();
    }

    // if user has role need then render the element
    if(this.authService.roleMatch(this.appHasRole)) {
      if(!this.isInvisible) {
        this.isInvisible = true;
        this.viewContainerRef.createEmbeddedView(this.templateRef);
      } else {
        this.isInvisible = false;
        this.viewContainerRef.clear();
      }
    }
  }

}
