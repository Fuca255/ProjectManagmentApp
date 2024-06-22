import { ChangeDetectionStrategy, Pipe, PipeTransform } from '@angular/core';
import { Component, inject } from '@angular/core';
import { JwtDecoderService } from '../../services/jwt-decoder.service';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';

// @Pipe({
//   name: 'safe'
// })

@Component({
  selector: 'app-helpcentre',
  templateUrl: './helpcentre.component.html',
  styleUrl: './helpcentre.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class HelpcentreComponent {

  private jwtService = inject(JwtDecoderService);

  public role : string = "";

  public role2: string = "";

  public pdfUrls: { [role: string]: string } = {
    Guest: 'assets/help-documents/guest-guide.pdf',
    Developer: 'assets/help-documents/developer-guide.pdf',
    ProjectManager: 'assets/help-documents/project-manager-guide.pdf',
    Admin: 'assets/help-documents/admin-guide.pdf',
    User: 'assets/help-documents/user-guide.pdf'
  };

  constructor(private sanitizer: DomSanitizer) {}

  ngOnInit(): void {
    let token = this.jwtService.getToken();
    if(token!=null)
    {
      let decode = this.jwtService.decodeToken(token);
      this.role = decode.role;
      if(this.role === "Project Manager"){
        this.role2 = "ProjectManager";
      }
      if(this.role === "Project Worker"){
        this.role2 = "Developer"
      }
      if(this.role === "Member"){
        this.role2 = "User";
      }
      console.log(this.role);
    }
  }

  public getPdfUrl(role: string){
    if (this.pdfUrls[role]) {
      return this.sanitizer.bypassSecurityTrustResourceUrl(this.pdfUrls[role]);
    }
    return null;
  }
}
