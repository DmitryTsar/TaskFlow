import api from './axios.js'

// --- Тип пользователя ---
export type User = {
    id: string
    userName: string
    email: string
    role?: 'Admin' | 'Manager' | 'User' // строго строка для фронтенда
    fullName?: string
}

// --- Типы проектов и задач ---
export type Project = { id: string; name: string }
export type Task = { id: string; title: string; projectId: string }

// --- Функция для конвертации роли в строку ---
function normalizeRole(role: number | string | undefined): 'Admin' | 'Manager' | 'User' {
    if (typeof role === 'string') {
        if (role === 'Admin' || role === 'Manager' || role === 'User') return role
    }
    if (typeof role === 'number') {
        switch (role) {
            case 0: return 'Admin'
            case 1: return 'Manager'
            case 2: return 'User'
        }
    }
    return 'User' // по умолчанию
}

// --- Функция для конвертации роли в число для API ---
function roleToNumber(role: 'Admin' | 'Manager' | 'User'): number {
    switch (role) {
        case 'Admin': return 0
        case 'Manager': return 1
        case 'User': return 2
    }
}

export const usersService = {
    getAll(): Promise<User[]> {
        return api.get<User[]>('/Users')
            .then(res => res.data.map(u => ({ ...u, role: normalizeRole(u.role) })))
    },

    getById(id: string): Promise<User> {
        return api.get<User>(`/Users/${id}`)
            .then(res => ({ ...res.data, role: normalizeRole(res.data.role) }))
    },

    create(payload: { userName: string; email: string; password: string; role?: 'Admin' | 'Manager' | 'User' }): Promise<User> {
        const apiPayload = { ...payload, role: payload.role ? roleToNumber(payload.role) : undefined }
        return api.post<User>('/Users', apiPayload)
            .then(res => ({ ...res.data, role: normalizeRole(res.data.role) }))
    },

    update(payload: { id: string; userName: string; email: string; role: 'Admin' | 'Manager' | 'User' }): Promise<User> {
        const apiPayload = { ...payload, role: roleToNumber(payload.role) }
        return api.put<User>(`/Users/${payload.id}`, apiPayload)
            .then(res => ({ ...res.data, role: normalizeRole(res.data.role) }))
    },

    remove(id: string): Promise<void> {
        return api.delete(`/Users/${id}`).then(() => { })
    },

    getProjects(id: string): Promise<Project[]> {
        return api.get<Project[]>(`/Users/${id}/projects`).then(res => res.data)
    },

    getCreatedTasks(id: string): Promise<Task[]> {
        return api.get<Task[]>(`/Users/${id}/tasks/created`).then(res => res.data)
    },

    getAssignedTasks(id: string): Promise<Task[]> {
        return api.get<Task[]>(`/Users/${id}/tasks/assigned`).then(res => res.data)
    }
}
