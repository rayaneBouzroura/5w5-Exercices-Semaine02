import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";


@Injectable()
export class TokenInterceptor implements HttpInterceptor{




  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    //recup token
    const token = sessionStorage.getItem("token");
    if(token){
      //clone la req
      const cloneRequete = req.clone({
        headers : req.headers.set("Authorization",`Bearer ${token}`)
      });
      return next.handle(cloneRequete);
    }else{
      return next.handle(req);
    }


  }
}
