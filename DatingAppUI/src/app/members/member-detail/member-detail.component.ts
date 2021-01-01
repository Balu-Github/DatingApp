import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TabsetComponent } from 'ngx-bootstrap/tabs';
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

  @ViewChild('memberTabs', {static: true}) memberTabs: TabsetComponent;

  user: User;
  //galleryOptions: NgxGalleryOptions[];
  //galleryImages: NgxGalleryImage[];

  constructor(private userService: UserService, private alertyfy: AlertifyService,
    private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.user = data['user'];
    });

    this.route.queryParams.subscribe(params => {
      const selectedTab = params['tab'];
      this.memberTabs.tabs[selectedTab > 0 ? selectedTab : 0].active = true;
    })
  }

  // loacUser() {
  //   this.userService.getUser(+this.route.snapshot.params['id']).subscribe((user:User) => {
  //     this.user = user;
  //   }, error => {
  //     this.alertyfy.error(error);
  //   })
  // }

  selectTab(tabId: number) {
    this.memberTabs.tabs[tabId].active = true;
  }
}
