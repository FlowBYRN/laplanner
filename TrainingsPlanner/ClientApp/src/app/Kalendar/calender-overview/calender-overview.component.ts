import { Component, OnInit } from '@angular/core';
import { AuthorizationService } from 'src/app/services/authorization.service';
import { ContextService } from 'src/app/services/context.service';
import { TrainingsGroupClient, TrainingsGroupDto, TrainingsAppointmentClient, TrainingsAppointmentDto } from 'src/clients/api.generated.clients';

@Component({
  selector: 'app-calender-overview',
  templateUrl: './calender-overview.component.html',
  styleUrls: ['./calender-overview.component.css']
})
export class CalenderOverviewComponent implements OnInit {

  groups: TrainingsGroupDto[] = [];
  selctedGroup: TrainingsGroupDto;

  constructor(private trainingsGroupClient: TrainingsGroupClient,
    private contextService: ContextService,
    private authService: AuthorizationService) { }

  async ngOnInit() {
    this.groups = await this.trainingsGroupClient.getAllGroups().toPromise();
    this.selctedGroup = this.groups.find(g => g.id == this.contextService.getGroupId())
  }

  selectNewGroup() {
    this.selctedGroup = undefined;
  }

  noGroupsFound(): boolean {
    return this.groups.length == 0;
  }
}
