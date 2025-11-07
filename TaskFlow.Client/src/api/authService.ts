import api from './axios.js'


export type AuthResponse = {
    token: string
    userName: string
    email: string
    role: string
}


export const authService = {
    async login(payload: { email: string; password: string }) {
        const { data } = await api.post<AuthResponse>('/Auth/login', payload)
        return data
    },
    async register(payload: { userName: string; email: string; password: string }) {
        const { data } = await api.post<AuthResponse>('/Auth/register', payload)
        return data
    },
    async profile() {
        const { data } = await api.get('/Auth/profile')
        return data
    }
}


