import { CanActivateFn, Router } from '@angular/router';
import { JwtDecoderService } from '../services/jwt-decoder.service';
import { inject } from '@angular/core';

export const pmAuthorizationGuard: CanActivateFn = (route, state) => {
  const jwt_Service = inject(JwtDecoderService);
  const router = inject(Router);
  let token = jwt_Service.getToken();
  if(token!==null)
  {

    let decode = jwt_Service.decodeToken(token);
    // if(route.url[1]!==undefined && route.url[1].path === 'project-details' && decode.role!=="Admin")
    // {
    //   console.log("jesu detalji");
    //   let projectsString = "" + decode.projects;
    //   let projects = projectsString.split(",");
    //   for(let i = 0; i< projects.length;i++)
    //   {
    //     const project = projects[i];
    //     // console.log(project===route.url[2].path);
    //     if(project===route.url[2].path)
    //       return true;

    //   }
    //   router.navigate(['/projects']);

    //   return false;
    // }
    
    if(decode.role==="Project Manager")
      return true;
    else if (decode.role==="Admin")
    {
      // console.log("admi")
      router.navigate(['/admin'])
      return false;
    }
    else
    {
      if(route.url[0].path === 'dashboard' || route.url[0].path==="teams")
      {
          router.navigate(['/projects'])
          return false;
      }
      else
        return true;
    }
  }

  return true;
};
