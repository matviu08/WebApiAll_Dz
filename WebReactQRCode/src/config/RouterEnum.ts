export const RouterEnum = {
    MAIN: '/',
    LOGIN: '/login',
    REGISTER: '/register',
    PROFILE: '/profile',
    QRCODE_CREATE: '/qr-code/create',
    QRCODE_EDIT: '/qr-code/edit',
} as const;

export type RouterEnum = typeof RouterEnum[keyof typeof RouterEnum];