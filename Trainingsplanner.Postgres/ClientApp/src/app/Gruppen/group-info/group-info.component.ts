import { Component, Input, OnInit } from '@angular/core';
import { TrainingsGroupClient, TrainingsGroupDto } from 'src/clients/api.generated.clients';

@Component({
  selector: 'app-group-info',
  templateUrl: './group-info.component.html',
  styleUrls: ['./group-info.component.css']
})
export class GroupInfoComponent implements OnInit {

  @Input() group: TrainingsGroupDto;
  currentlyUpdating: boolean = false;

  constructor(private trainingsGroupClient: TrainingsGroupClient) { }

  ngOnInit(): void {
  }

  async updateGroup() {
    await this.trainingsGroupClient.updateGroup(this.group).toPromise();
    this.currentlyUpdating = false;
  }
}
