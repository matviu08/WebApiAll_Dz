import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import App from './App.tsx'
import ReactQueryProvider from './providers/ReactQueryProvider.tsx'
import {BrowserRouter} from "react-router";

createRoot(document.getElementById('root')!).render(
    <StrictMode>
        <ReactQueryProvider>
            <BrowserRouter>
                <App />
            </BrowserRouter>
        </ReactQueryProvider>
    </StrictMode>,
)