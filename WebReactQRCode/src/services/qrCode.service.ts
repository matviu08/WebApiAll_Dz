import instance from "./api/interceptors.api.ts";
import { getQrCodesUrl, getQrCodeUrl, getQrCodeDeactivateUrl } from "../config/api.config.ts";
import type {ICreateQrCode, IUpdateQrCode, IQrCode} from "../types/qrCode.types.ts";

export const QrCodeService = {
    getAll: () => instance.get<IQrCode[]>(getQrCodesUrl()),
    post: ({ data }: { data: ICreateQrCode }) =>
        instance<void>({
            url: getQrCodesUrl(),
            method: "POST",
            data,
        }),
    update: ({ id, data }: { id: number; data: IUpdateQrCode }) =>
        instance<void>({
            url: getQrCodeUrl(id),
            method: "PUT",
            data,
        }),
    deactivate: ({ id }: { id: number }) =>
        instance<void>({
            url: getQrCodeDeactivateUrl(id),
            method: "PATCH",
        }),
};