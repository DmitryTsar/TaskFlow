import api from './axios'

// Тип задачи
export type Task = {
    id: string
    title: string
    description?: string
    projectId: string
    createdById: string
    assignedToId: string
    status: 0 | 1 | 2 | 3 | 4
    priority: 0 | 1 | 2

    // Дополнительные поля для UI (опциональные)
    projectName?: string
    assignedToName?: string
    updatedById?: string
    createdAt?: string
    updatedAt?: string
}

export const tasksService = {
    getAll(): Promise<Task[]> {
        return api.get<Task[]>('/Tasks').then(res => res.data)
    },

    getById(id: string): Promise<Task> {
        return api.get<Task>(`/Tasks/${id}`).then(res => res.data)
    },

    create(payload: {
        title: string
        description: string
        projectId: string
        createdById: string
        assignedToId: string
        status: 0 | 1 | 2 | 3 | 4
        priority: 0 | 1 | 2
    }): Promise<Task> {
        return api.post<Task>('/Tasks', payload).then(res => res.data)
    },

    update(payload: {
        id: string
        title: string
        description: string
        assignedToId: string
        status: 0 | 1 | 2 | 3 | 4
        priority: 0 | 1 | 2
        updatedById: string
    }): Promise<Task> {
        return api.put<Task>(`/Tasks/${payload.id}`, payload).then(res => res.data)
    },

    remove(id: string): Promise<void> {
        return api.delete<void>(`/Tasks/${id}`).then(res => res.data)
    },

    getByProject(projectId: string): Promise<Task[]> {
        return api.get<Task[]>(`/Tasks/project/${projectId}`).then(res => res.data)
    },

    getByProjectAndStatus(projectId: string, status: 0 | 1 | 2 | 3 | 4): Promise<Task[]> {
        return api.get<Task[]>(`/Tasks/project/${projectId}/status/${status}`).then(res => res.data)
    }
}
