import instance from "./api/interceptors.api.ts";
import type {IProfile} from "../types/profile.types.ts";
import {getProfileUrl} from "../config/api.config.ts";

export const ProfileService = {
    get: () => instance.get<IProfile>(getProfileUrl()),
};