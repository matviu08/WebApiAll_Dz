import { useMutation } from "@tanstack/react-query";
import { useNavigate } from "react-router";
import {QrCodeService} from "../services/qrCode.service.ts";
import type {ICreateQrCode} from "../types/qrCode.types.ts";
import {RouterEnum} from "../config/RouterEnum.ts";

export const useQRCodeCreateMutation = () => {
    const navigate = useNavigate();

    return useMutation({
        mutationKey: ["login-post"],
        mutationFn: (props: { data: ICreateQrCode }) =>
            QrCodeService.post(props).then((res) => res.data),
        onSuccess: () => {
            navigate(RouterEnum.PROFILE);
        },
    });
};