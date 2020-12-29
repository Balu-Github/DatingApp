import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
//import { NgxGalleryImage, NgxGalleryOptions } from 'ngx-gallery';
import { User } from 'src/app/_models/user';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.scss']
})
export class MemberDetailComponent implements OnInit {

  user: User;
  //galleryOptions: NgxGalleryOptions[];
  //galleryImages: NgxGalleryImage[];

  constructor(private userService: UserService, private alertyfy: AlertifyService,
    private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.user = data['user'];
    });
  }

  // loacUser() {
  //   this.userService.getUser(+this.route.snapshot.params['id']).subscribe((user:User) => {
  //     this.user = user;
  //   }, error => {
  //     this.alertyfy.error(error);
  //   })
  // }
}
