import { useMutation, useQueryClient } from "@tanstack/react-query";
import { QrCodeService } from "../services/qrCode.service.ts";

export const useQRCodeDeactivateMutation = () => {
    const queryClient = useQueryClient();

    return useMutation({
        mutationKey: ["qr-code-deactivate"],
        mutationFn: (props: { id: number }) =>
            QrCodeService.deactivate(props).then((res) => res.data),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["qrCodes"] });
        },
    });
};