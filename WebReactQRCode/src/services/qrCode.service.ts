import instance from "./api/interceptors.api.ts";
import { getQrCodesUrl } from "../config/api.config.ts";
import type {IQrCode} from "../types/qrCode.types.ts";

export const QrCodeService = {
    getAll: () => instance.get<IQrCode[]>(getQrCodesUrl()),
};