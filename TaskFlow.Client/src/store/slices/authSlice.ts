import { createSlice, PayloadAction } from '@reduxjs/toolkit'

// --- Тип для JWT payload ---
type JwtPayload = {
    sub: string
    email: string
    username: string
    role?: string // строковая роль
}

// --- Парсим JWT и берём роль как строку ---
function parseJwt(token: string): JwtPayload | null {
    try {
        const base64Url = token.split('.')[1]
        const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/')
        const jsonPayload = decodeURIComponent(
            atob(base64)
                .split('')
                .map(c => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
                .join('')
        )
        const payload = JSON.parse(jsonPayload)
        // берем роль из payload.role или стандартного claim ASP.NET
        payload.role =
            payload.role ||
            payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']
        return payload
    } catch {
        return null
    }
}

// --- Тип состояния auth ---
export type AuthState = {
    token: string | null
    userId: string | null
    userName: string | null
    email: string | null
    role: string | null // строковая роль
    authInitialized: boolean
}

// --- Инициализация состояния ---
const initialState: AuthState = {
    token: null,
    userId: null,
    userName: null,
    email: null,
    role: null,
    authInitialized: false
}

// --- Slice ---
const slice = createSlice({
    name: 'auth',
    initialState,
    reducers: {
        initializeAuth(state) {
            const token = localStorage.getItem('tf_token')
            if (token) {
                const payload = parseJwt(token)
                state.token = token
                state.userId = payload?.sub || null
                state.userName = payload?.username || null
                state.email = payload?.email || null
                state.role = payload?.role || null
            }
            state.authInitialized = true
        },
        setCredentials(state, action: PayloadAction<{ token: string }>) {
            state.token = action.payload.token
            const payload = parseJwt(action.payload.token)
            state.userId = payload?.sub || null
            state.userName = payload?.username || null
            state.email = payload?.email || null
            state.role = payload?.role || null
            localStorage.setItem('tf_token', action.payload.token)
        },
        logout(state) {
            state.token = null
            state.userId = null
            state.userName = null
            state.email = null
            state.role = null
            localStorage.removeItem('tf_token')
        }
    }
})

export const { initializeAuth, setCredentials, logout } = slice.actions
export default slice.reducer
export { parseJwt }

