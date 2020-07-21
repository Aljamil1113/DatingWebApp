import { Injectable } from "@angular/core";
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { User } from '../_models/user';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AuthService } from '../_services/auth.service';

@Injectable()
export class MemberEditResolver implements Resolve<User> {
    constructor(private userSerivce: UserService, private authService: AuthService,
        private route: Router, private alertify: AlertifyService){}

        resolve(route: ActivatedRouteSnapshot) : Observable<User>{
            return this.userSerivce.getUser(this.authService.decodedToken.nameid).pipe(
                catchError(error => {
                    this.alertify.error('Problem retrieving data');
                    this.route.navigate(['/members']);
                    return of(null);
                })
            );
        }
}