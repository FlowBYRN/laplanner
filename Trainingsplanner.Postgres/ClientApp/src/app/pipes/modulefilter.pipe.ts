import { Pipe, PipeTransform } from '@angular/core';
import { TrainingsModuleDto } from 'src/clients/api.generated.clients';

@Pipe({
  name: 'modulefilter'
})
export class ModulefilterPipe implements PipeTransform {

  /**
  * Transform
  *
  * @param {any[]} items
  * @param {string} searchText
  * @returns {any[]}
  */
  transform(items: TrainingsModuleDto[], searchText: string): any[] {
    if (!items) {
      return [];
    }
    if (!searchText) {
      return items;
    }
    searchText = searchText.toLocaleLowerCase();

    return items.filter(it => {
      return it.title.toLocaleLowerCase().includes(searchText) || it.description && it.description.toLocaleLowerCase().includes(searchText) || it.trainingsModulesTrainingsModuleTags.some(tag => tag.trainingsModuleTag.title.toLocaleLowerCase().includes(searchText));
    });
  }

}
