import { Component, OnInit } from '@angular/core';
import { AuthorizationService } from 'src/app/services/authorization.service';
import { TrainingsGroupClient, TrainingsGroupDto } from 'src/clients/api.generated.clients';

@Component({
  selector: 'app-group-select',
  templateUrl: './group-select.component.html',
  styleUrls: ['./group-select.component.css']
})
export class GroupSelectComponent implements OnInit {

  groups: TrainingsGroupDto[] = [];
  selctedGroup: TrainingsGroupDto;
  constructor(private trainingsGroupClient: TrainingsGroupClient, private authService: AuthorizationService) { }

  async ngOnInit() {
    this.groups = await this.trainingsGroupClient.getAllGroups().toPromise();
  }

  selectNewGroup() {
    this.selctedGroup = undefined;
  }

  noGroupsFound(): boolean {
    return this.groups.length == 0;
  }
}
