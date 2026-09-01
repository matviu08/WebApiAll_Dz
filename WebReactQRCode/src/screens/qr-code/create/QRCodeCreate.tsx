import { useState, type FormEvent } from "react";
import {useQRCodeCreateMutation} from "../../../hooks/useQRCodeCreateMutation.ts";
// import { UseLoginMutation } from "./hooks/useLoginMutation.ts";

const QRCodeCreate = () => {
    const [name, setName] = useState("");
    const [targetUrl, setTargetUrl] = useState("");
    const [formError, setFormError] = useState<string | null>(null);

    const { mutateAsync, isPending } = useQRCodeCreateMutation();

    const onSubmit = async (e: FormEvent) => {
        e.preventDefault();
        setFormError(null);
        try {
            await mutateAsync({ data: { name, targetUrl } });
        } catch {
            setFormError("Невірний назва або url");
        }
    };

    return (
        <div className="min-h-[calc(100vh-80px)] bg-slate-50/50 px-4 py-12 flex items-center justify-center">
            <div className="w-full max-w-xl mx-auto space-y-6">
                <div className="text-center">
                    <h1 className="text-2xl sm:text-3xl font-bold tracking-tight text-slate-900">
                        Створення QR-коду
                    </h1>
                    <p className="mt-2 text-sm sm:text-base text-slate-500">
                        Створіть QR-код для швидкого переходу за потрібним посиланням
                    </p>
                </div>

                <div className="bg-white border border-slate-200/80 rounded-2xl shadow-sm p-6 sm:p-8">
                    <div className="mb-6 border-b border-slate-100 pb-4">
                        <h2 className="text-lg font-semibold text-slate-900">
                            Дані QR-коду
                        </h2>
                        <p className="mt-1 text-xs sm:text-sm text-slate-500">
                            Вкажіть назву та посилання, яке буде відкриватися після сканування.
                        </p>
                    </div>

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
                                placeholder="Наприклад: Мій сайт"
                                className="w-full px-3.5 py-2.5 bg-slate-50/50 border border-slate-300 rounded-xl text-slate-900 text-sm placeholder:text-slate-400 focus:bg-white focus:ring-2 focus:ring-indigo-500/20 focus:border-indigo-600 focus:outline-none transition-all"
                            />
                            <p className="mt-1.5 text-xs text-slate-400">
                                Назва потрібна для того, щоб ви могли легко знайти QR-код у кабінеті.
                            </p>
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
                                placeholder="https://example.com"
                                className="w-full px-3.5 py-2.5 bg-slate-50/50 border border-slate-300 rounded-xl text-slate-900 text-sm placeholder:text-slate-400 focus:bg-white focus:ring-2 focus:ring-indigo-500/20 focus:border-indigo-600 focus:outline-none transition-all"
                            />
                            <p className="mt-1.5 text-xs text-slate-400">
                                Наприклад: https://google.com
                            </p>
                        </div>

                        {formError && (
                            <div className="p-3.5 rounded-xl bg-red-50 border border-red-200/60">
                                <p className="text-sm text-red-600 font-medium">{formError}</p>
                            </div>
                        )}

                        <button
                            type="submit"
                            disabled={isPending}
                            className="w-full py-2.5 px-4 rounded-xl bg-indigo-600 hover:bg-indigo-700 active:bg-indigo-800 disabled:opacity-60 text-white font-medium text-sm transition-all shadow-sm focus:ring-2 focus:ring-indigo-500/20 focus:outline-none"
                        >
                            {isPending ? "Створення..." : "Створити QR-код"}
                        </button>
                    </form>
                </div>
            </div>
        </div>
    );
};

export default QRCodeCreate;