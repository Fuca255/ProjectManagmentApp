import { NgToastService } from 'ng-angular-popup';

export class Notify{
    constructor(private toast : NgToastService) {
      }

    //show Toast on top center position
    showSuccess(topic:string,message:string) {
        this.toast.success({detail:topic, summary:message, duration:5000, sticky:false, position:'bottomRight'});
    }

    //show Toast on bottom center position
    showError(topic:string,message:string) {
        const mess = `<b>${message}\n\n</b>`;
        this.toast.error({detail:topic, summary:"poruke \n poruka ", duration:5000, sticky:false, position:'bottomRight'});
    }

    //show Toast on top left position
    showInfo(topic:string,message:string) {
        this.toast.info({detail:topic,summary:message, duration:5000, sticky:false, position: 'bottomRight',});
    }

    //show Toast on bottom left position
    showWarn(topic:string,message:string) {
        this.toast.warning({detail:topic,summary:message, duration:5000, sticky:false, position: 'bottomRight'});
    }
}