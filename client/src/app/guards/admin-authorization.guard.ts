import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { JwtDecoderService } from '../services/jwt-decoder.service';

export const adminAuthorizationGuard: CanActivateFn = (route, state) => {
  const jwt_Service = inject(JwtDecoderService);
  const router = inject(Router);
  let token = jwt_Service.getToken();
  if(token!==null)
  {
  //   console.log("nesto");
    let decode = jwt_Service.decodeToken(token);
    if(decode.role==="Admin")
      return true;
    else if (decode.role==="Project Manager")
    {
      if(route.url[0].path === 'admin')
        router.navigate(['/dashboard'])
      return false;
    }
    else
    {
      if(route.url[0].path === 'admin')
        router.navigate(['/projects'])
      return false;
    }
  }

  return true;
};
