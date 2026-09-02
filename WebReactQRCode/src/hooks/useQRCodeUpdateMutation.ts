import { useMutation, useQueryClient } from "@tanstack/react-query";
import { QrCodeService } from "../services/qrCode.service.ts";
import type { IUpdateQrCode } from "../types/qrCode.types.ts";

export const useQRCodeUpdateMutation = () => {
    const queryClient = useQueryClient();

    return useMutation({
        mutationKey: ["qr-code-update"],
        mutationFn: (props: { id: number; data: IUpdateQrCode }) =>
            QrCodeService.update(props).then((res) => res.data),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["qrCodes"] });
        },
    });
};