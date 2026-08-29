export const RouterEnum = {
    MAIN: '/',
    LOGIN: '/login',
    REGISTER: '/register',
    PROFILE: '/profile',
} as const;

export type RouterEnum = typeof RouterEnum[keyof typeof RouterEnum];