import { useState, type FormEvent, useEffect } from "react";
import { useNavigate, useParams } from "react-router";
import { useQrCodesQuery } from "../../../hooks/useQrCodesQuery.ts";
import { useQRCodeUpdateMutation } from "../../../hooks/useQRCodeUpdateMutation.ts";
import { RouterEnum } from "../../../config/RouterEnum.ts";
import Loader from "../../../components/Loader.tsx";

const QRCodeEdit = () => {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();

    const { data: qrCodes, isLoading } = useQrCodesQuery();
    const qrCode = qrCodes?.find((qr) => qr.id === Number(id));

    const [name, setName] = useState("");
    const [targetUrl, setTargetUrl] = useState("");
    const [formError, setFormError] = useState<string | null>(null);

    const { mutateAsync, isPending } = useQRCodeUpdateMutation();

    useEffect(() => {
        if (qrCode) {
            setName(qrCode.name);
            setTargetUrl(qrCode.targetUrl);
        }
    }, [qrCode]);

    const onSubmit = async (e: FormEvent) => {
        e.preventDefault();
        setFormError(null);
        try {
            await mutateAsync({ id: Number(id), data: { name, targetUrl } });
            navigate(RouterEnum.PROFILE);
        } catch {
            setFormError("Невірна назва або url");
        }
    };

    if (isLoading) return <Loader />;

    if (!qrCode) {
        return (
            <p className="text-center text-red-500 mt-20">
                QR-код не знайдено
            </p>
        );
    }

    if (!qrCode.isActive) {
        return (
            <p className="text-center text-red-500 mt-20">
                Деактивований QR-код не можна редагувати
            </p>
        );
    }

    return (
        <div className="min-h-[calc(100vh-80px)] bg-slate-50/50 px-4 py-12 flex items-center justify-center">
            <div className="w-full max-w-xl mx-auto space-y-6">
                <div className="text-center">
                    <h1 className="text-2xl sm:text-3xl font-bold tracking-tight text-slate-900">
                        Редагування QR-коду
                    </h1>
                    <p className="mt-2 text-sm sm:text-base text-slate-500">
                        Змініть назву або посилання QR-коду
                    </p>
                </div>

                <div className="bg-white border border-slate-200/80 rounded-2xl shadow-sm p-6 sm:p-8">
                    <form className="space-y-5" onSubmit={onSubmit}>
                        <div>
                            <label className="block text-sm font-medium text-slate-700 mb-1.5">
                                Назва QR-коду <span className="text-red-500">*</span>
                            </label>
                            <input
                                type="text"
                                required
                                value={name}
                                onChange={(e) => setName(e.target.value)}
                                className="w-full px-3.5 py-2.5 bg-slate-50/50 border border-slate-300 rounded-xl text-slate-900 text-sm focus:bg-white focus:ring-2 focus:ring-indigo-500/20 focus:border-indigo-600 focus:outline-none transition-all"
                            />
                        </div>

                        <div>
                            <label className="block text-sm font-medium text-slate-700 mb-1.5">
                                Посилання <span className="text-red-500">*</span>
                            </label>
                            <input
                                type="url"
                                required
                                value={targetUrl}
                                onChange={(e) => setTargetUrl(e.target.value)}
                                className="w-full px-3.5 py-2.5 bg-slate-50/50 border border-slate-300 rounded-xl text-slate-900 text-sm focus:bg-white focus:ring-2 focus:ring-indigo-500/20 focus:border-indigo-600 focus:outline-none transition-all"
                            />
                        </div>

                        {formError && (
                            <div className="p-3.5 rounded-xl bg-red-50 border border-red-200/60">
                                <p className="text-sm text-red-600 font-medium">{formError}</p>
                            </div>
                        )}

                        <div className="flex gap-3">
                            <button
                                type="button"
                                onClick={() => navigate(RouterEnum.PROFILE)}
                                className="w-full py-2.5 px-4 rounded-xl bg-gray-100 hover:bg-gray-200 text-slate-700 font-medium text-sm transition-all"
                            >
                                Скасувати
                            </button>
                            <button
                                type="submit"
                                disabled={isPending}
                                className="w-full py-2.5 px-4 rounded-xl bg-indigo-600 hover:bg-indigo-700 active:bg-indigo-800 disabled:opacity-60 text-white font-medium text-sm transition-all shadow-sm"
                            >
                                {isPending ? "Збереження..." : "Зберегти"}
                            </button>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    );
};

export default QRCodeEdit;