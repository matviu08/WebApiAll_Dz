import { useEffect } from "react";
import { useNavigate } from "react-router";
import { useAuth } from "../../context/AuthContext.tsx";
import { useProfileQuery } from "../../hooks/useProfileQuery.ts";
import Loader from "../../components/Loader.tsx";
import { RouterEnum } from "../../config/RouterEnum.ts";
import { getImageUrl } from "../../config/api.config.ts";

const Profile = () => {
    const { isAuthenticated } = useAuth();
    const navigate = useNavigate();
    const { data: profile, isLoading, isError, error } = useProfileQuery();

    useEffect(() => {
        if (!isAuthenticated) {
            navigate(RouterEnum.LOGIN);
        }
    }, [isAuthenticated, navigate]);

    if (!isAuthenticated) return null;

    if (isLoading) return <Loader />;

    if (isError) {
        return (
            <p className="text-center text-red-500 mt-20">
                Помилка: {(error as Error).message}
            </p>
        );
    }

    if (!profile) return null;

    const imageUrl = getImageUrl(profile.image, 432);
    const fullName =
        [profile.lastName, profile.firstName].filter(Boolean).join(" ") || "Без імені";

    return (
        <div className="flex items-center justify-center px-4 mt-12">
            <div className="w-full max-w-md p-8 space-y-6 bg-white border border-gray-200 rounded-2xl shadow-sm text-center">
                <div className="w-28 h-28 mx-auto rounded-full bg-gray-100 border border-gray-200 overflow-hidden flex items-center justify-center">
                    {imageUrl ? (
                        <img src={imageUrl} alt="Фото профілю" className="w-full h-full object-cover" />
                    ) : (
                        <span className="text-xs text-gray-400">Фото</span>
                    )}
                </div>

                <div>
                    <h1 className="text-2xl font-bold text-gray-900">{fullName}</h1>
                    <p className="text-gray-500 mt-1">{profile.email}</p>
                </div>

                {profile.roles.length > 0 && (
                    <div className="flex flex-wrap justify-center gap-2">
                        {profile.roles.map((role) => (
                            <span
                                key={role}
                                className="px-3 py-1 text-xs rounded-full bg-indigo-50 text-indigo-700"
                            >
                                {role}
                            </span>
                        ))}
                    </div>
                )}
            </div>
        </div>
    );
};

export default Profile;