import { useQuery } from "@tanstack/react-query";
import { QrCodeService } from "../services/qrCode.service.ts";

export const useQrCodesQuery = () => {
    return useQuery({
        queryKey: ["qrCodes"],
        queryFn: async () => {
            const response = await QrCodeService.getAll();
            return response.data;
        },
    });
};