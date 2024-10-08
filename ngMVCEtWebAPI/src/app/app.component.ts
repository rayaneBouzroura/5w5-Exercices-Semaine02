import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component } from '@angular/core';
import { lastValueFrom } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
testDataDTO: any;

  title = 'ngMVCEtWebAPI';
  loginDto = {
    username : "test",
    password : "Passw0rd!"
  }

  registerDto = {
    username : "test",
    password : "Passw0rd!",
    passwordConfirm : "Passw0rd!",
    email : "test"
  };
  //testDataDTO is an object with just a string called Name


  uriEndpoint = "https://localhost:7154/api/Account/";
  responseCall : String[]= [];
  testData : String = "";






  constructor(private http : HttpClient) {}

  // async getPublicData() : Promise<String[]> {
  async getPublicData()  {
  console.log("getPublicData");
  const response = await lastValueFrom(this.http.get('https://localhost:7154/api/Account/PublicTest'));
  console.log(response);
  this.responseCall = response as String[];
  }
  async getPrivateData()  {
    console.log("getPrivateData");
    try {


      const response = await lastValueFrom(this.http.get('https://localhost:7154/api/Account/PrivateTest'));
      console.log(response);
      this.responseCall = response as String[];
    } catch (err) {
      console.log(err);
    }
  }


  async login(){
    console.log("login");
    let result = await lastValueFrom(this.http.post<any>("https://localhost:7154/api/Account/Login",this.loginDto));
    sessionStorage.setItem("token",result.token);
    console.log(result);

  }


  async register(){

    console.log("register");


    try {
      let result = await lastValueFrom(this.http.post<any>("https://localhost:7154/api/Account/Register",this.registerDto));
      //save token in sessionStorage
      sessionStorage.setItem("token",result.token);
      console.log(result);
    } catch (err) {
      console.log(err);
    }

  }
  //logout method that clears the sessionStorage
  logout(){
    sessionStorage.clear();
  }

  async sendData() {

    try{
      //create dto object CreateTestDataDTO avec comme seul champ Name

      let CreateTestDataDTO = {
        Name : this.testData
      }
      //
      const link = "https://localhost:7154/api/TestData";
      let result = await lastValueFrom(this.http.post(link,CreateTestDataDTO));
      console.log(result);


    }
    catch(err){
      console.log(err);
    }

    }





}
