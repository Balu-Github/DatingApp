import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, Router } from "@angular/router";
import { Observable, of } from "rxjs";
import { catchError } from "rxjs/operators";
import { Message } from "../_models/message";
import { User } from "../_models/user";
import { AlertifyService } from "../_services/alertify.service";
import { AuthService } from "../_services/auth.service";
import { UserService } from "../_services/user.service";


@Injectable()
export class MessagesResolver implements Resolve<Message[]> {
    pageNumber = 1;
    pageSize = 5; 
    messageContainer = "Unread";
    
    constructor(private userService: UserService, private authService: AuthService, private router: Router
        , private alertify: AlertifyService) {

    }

    resolve(route: ActivatedRouteSnapshot): Observable<Message[]> {
       return this.userService.getMessages(this.authService.decodedToken.nameid, this.pageNumber, this.pageSize, this.messageContainer).pipe(
           catchError(error => {
               this.alertify.error('Problem retriving data');
               this.router.navigate(['/home']);
               return of(null);
           })
       )
    }
}