export class User {
  email: string;
  password: string;
  token?: string;

  constructor(email: string, password: string) {
    this.email = email;
    this.password = password;
  }

}
