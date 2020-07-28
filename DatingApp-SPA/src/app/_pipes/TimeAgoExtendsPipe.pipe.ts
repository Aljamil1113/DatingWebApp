import { TimeAgoPipe } from 'time-ago-pipe';
import { PipeTransform, Pipe } from '@angular/core';

@Pipe({
    name: 'timeAgo',
    pure: false
})

export class TimeAgoExtendsPipe extends TimeAgoPipe implements PipeTransform {
    transform(value: string): string {
        return super.transform(value);
    }
}
