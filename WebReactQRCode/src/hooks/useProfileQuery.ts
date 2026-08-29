import { useQuery } from '@tanstack/react-query';
import {useAuth} from "../context/AuthContext.tsx";
import {ProfileService} from "../services/profile.service.ts";
import type {IProfile} from "../types/profile.types.ts";

export const useProfileQuery = () => {
    const { isAuthenticated } = useAuth();

    return useQuery({
        queryKey: ['profile'],
        queryFn: () => ProfileService.get().then((res) => res.data as IProfile),
        enabled: isAuthenticated,
    });
};