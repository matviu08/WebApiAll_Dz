export const SERVER_URL = import.meta.env.VITE_SERVER_URL;
export const API_URL = `${SERVER_URL}/api`;

export const getUsersUrl = () => '/users';
export const postLoginUrl = () => '/Account/Login';
export const postRegisterUrl = () => '/Account/Register';
export const getProfileUrl = () => '/Account/Profile';
export const getQrCodesUrl = () => '/QrCodes';

export const getImageUrl = (
    image?: string | null,
    size: 64 | 432 | 800 | 1280 = 432,
) => (image ? `${SERVER_URL}/myimages/${image}_${size}.webp` : null);