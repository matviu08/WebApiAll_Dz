export interface ILogin {
    token: string;
}

export interface IAuthTokenInfo {
    email: string;
    exp: string;
    role: string;
}

export interface ILogin_Data {
    email: string;
    password: string;
}