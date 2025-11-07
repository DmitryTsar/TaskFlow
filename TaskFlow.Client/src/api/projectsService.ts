import api from './axios'

// Тип проекта
export type Project = {
    id: string
    name: string
    description?: string
    ownerId?: string

    // Дополнительные поля для UI
    managerName?: string
    tasksCount?: number
    openTasksCount?: number
    createdAt?: string
    updatedAt?: string
}

// Тип задачи (короткая версия для страниц проектов)
export type Task = {
    id: string
    title: string
    description?: string
    status: 0 | 1 | 2 | 3 | 4
    priority: 0 | 1 | 2
    assignedToId?: string
    projectId?: string
    assignedToName?: string
    updatedAt?: string
}

export const projectsService = {
    getAll(): Promise<Project[]> {
        return api.get<Project[]>('/Projects').then(res => res.data)
    },

    getById(id: string): Promise<Project> {
        return api.get<Project>(`/Projects/${id}`).then(res => res.data)
    },

    create(payload: { name: string; description: string; ownerId: string }): Promise<Project> {
        return api.post<Project>('/Projects', payload).then(res => res.data)
    },

    update(payload: { id: string; name: string; description: string }): Promise<Project> {
        return api.put<Project>(`/Projects/${payload.id}`, payload).then(res => res.data)
    },

    remove(id: string): Promise<void> {
        return api.delete<void>(`/Projects/${id}`).then(res => res.data)
    },

    getTasks(id: string): Promise<Task[]> {
        return api.get<Task[]>(`/Projects/${id}/tasks`).then(res => res.data)
    },

    getTasksByStatus(id: string, status: 0 | 1 | 2 | 3 | 4): Promise<Task[]> {
        return api.get<Task[]>(`/Projects/${id}/tasks/status/${status}`).then(res => res.data)
    }

}
