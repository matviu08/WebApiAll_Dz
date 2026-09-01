import { useEffect } from "react";
import { useNavigate } from "react-router";
import { useAuth } from "../../context/AuthContext.tsx";
import { useProfileQuery } from "../../hooks/useProfileQuery.ts";
import { useQrCodesQuery } from "../../hooks/useQrCodesQuery.ts";
import Loader from "../../components/Loader.tsx";
import { RouterEnum } from "../../config/RouterEnum.ts";
import { getImageUrl, SERVER_URL } from "../../config/api.config.ts";
import QRCode from "react-qr-code";

const Profile = () => {
    const { isAuthenticated } = useAuth();
    const navigate = useNavigate();

    const {
        data: profile,
        isLoading: profileLoading,
        isError: profileError,
        error: profileErrorData,
    } = useProfileQuery();

    const {
        data: qrCodes,
        isLoading: qrLoading,
        isError: qrError,
    } = useQrCodesQuery();


    console.log("qrCodes", qrCodes);

    useEffect(() => {
        if (!isAuthenticated) {
            navigate(RouterEnum.LOGIN);
        }
    }, [isAuthenticated, navigate]);

    if (!isAuthenticated) return null;

    if (profileLoading || qrLoading) {
        return <Loader />;
    }

    if (profileError) {
        return (
            <p className="text-center text-red-500 mt-20">
                Помилка: {(profileErrorData as Error).message}
            </p>
        );
    }

    if (!profile) return null;

    const imageUrl = getImageUrl(profile.image, 432);

    const fullName =
        [profile.lastName, profile.firstName]
            .filter(Boolean)
            .join(" ") || "Без імені";

    return (
        <div className="max-w-6xl mx-auto px-4 py-10">
            <div className="flex justify-center">
                <div className="w-full max-w-md p-8 space-y-6 bg-white border border-gray-200 rounded-2xl shadow-sm text-center">

                    <div className="w-28 h-28 mx-auto rounded-full bg-gray-100 border border-gray-200 overflow-hidden flex items-center justify-center">
                        {imageUrl ? (
                            <img
                                src={imageUrl}
                                alt="Фото профілю"
                                className="w-full h-full object-cover"
                            />
                        ) : (
                            <span className="text-xs text-gray-400">
                                Фото
                            </span>
                        )}
                    </div>

                    <div>
                        <h1 className="text-2xl font-bold text-gray-900">
                            {fullName}
                        </h1>

                        <p className="text-gray-500 mt-1">
                            {profile.email}
                        </p>
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

            <div className="mt-10">

                <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4 mb-6">
                    <div>
                        <h2 className="text-2xl font-bold text-gray-900">
                            Мої QR-коди
                        </h2>

                        <p className="text-gray-500 mt-1">
                            Переглядайте свої QR-коди
                        </p>
                    </div>

                    <button
                        onClick={() => navigate(RouterEnum.QRCODE_CREATE)}
                        className="px-5 py-2.5 rounded-xl bg-indigo-600 hover:bg-indigo-700 active:bg-indigo-800 text-white font-medium text-sm transition shadow-sm flex items-center justify-center gap-2 shrink-0"
                    >
                        <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M12 4v16m8-8H4" />
                        </svg>
                        Створити QR-код
                    </button>
                </div>



                {qrError && (
                    <p className="text-center text-red-500">
                        Не вдалося завантажити QR-коди
                    </p>
                )}

                {!qrError && qrCodes?.length === 0 && (
                    <div className="text-center py-12 text-gray-500">
                        У вас ще немає QR-кодів
                    </div>
                )}

                <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">

                    {qrCodes?.map((qr) => {

                        const qrUrl =
                            `${SERVER_URL}/api/QrCodes/view/${qr.code}`;

                        return (
                            <div
                                key={qr.id}
                                className="bg-white border border-gray-200 rounded-2xl shadow-sm p-5"
                            >

                                <div className="flex justify-center mb-5">
                                    <QRCode value={qrUrl}
                                            className="w-52 h-52">
                                    </QRCode>
                                </div>

                                <h3 className="text-lg font-bold text-gray-900">
                                    {qr.name}
                                </h3>

                                <p className="text-sm text-gray-500 mt-1 break-all">
                                    {qr.targetUrl}
                                </p>

                                <div className="mt-4 space-y-2 text-sm">

                                    <div className="flex justify-between">
                                        <span className="text-gray-500">
                                            Створено:
                                        </span>

                                        <span className="font-medium">
                                            {qr.createdAt}
                                        </span>
                                    </div>

                                    <div className="flex justify-between">
                                        <span className="text-gray-500">
                                            Сканувань:
                                        </span>

                                        <span className="font-medium">
                                            {qr.scanCount}
                                        </span>
                                    </div>

                                    <div className="flex justify-between">
                                        <span className="text-gray-500">
                                            Статус:
                                        </span>

                                        <span
                                            className={
                                                qr.isActive
                                                    ? "text-green-600 font-medium"
                                                    : "text-red-600 font-medium"
                                            }
                                        >
                                            {qr.isActive
                                                ? "Активний"
                                                : "Неактивний"}
                                        </span>
                                    </div>

                                </div>

                                <a
                                    href={qrUrl}
                                    target="_blank"
                                    rel="noreferrer"
                                    className="block text-center mt-5 w-full py-2.5 rounded-lg bg-gray-100 hover:bg-gray-200 font-medium"
                                >
                                    Перевірити QR
                                </a>

                            </div>
                        );
                    })}

                </div>
            </div>
        </div>
    );
};

export default Profile;