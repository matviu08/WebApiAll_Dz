import instance from "../../../services/api/interceptors.api.ts";
import { postRegisterUrl } from "../../../config/api.config.ts";
import type { IRegister, IRegister_Data } from "../types/IRegister.ts";

export const RegisterService = {
    post: ({ data }: { data: IRegister_Data }) =>
        instance<IRegister>({
            url: postRegisterUrl(),
            method: "POST",
            headers: {"Content-Type": "multipart/form-data"},
            data: data
        }),
};