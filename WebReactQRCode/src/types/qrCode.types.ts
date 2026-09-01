export interface IQrCode {
    id: number;
    name: string;
    code: string;
    targetUrl: string;
    isActive: boolean;
    createdAt: string;
    scanCount: number;
}

export interface ICreateQrCode {
    name: string;
    targetUrl: string;
}