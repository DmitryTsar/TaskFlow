import api from './axios.js'

// Тип записи истории задачи
export type TaskHistoryItem = {
    propertyName: string
    oldValue?: string
    newValue?: string
    changedByName: string
    changedAt: string
}

export const taskHistoryService = {
    getByTask(taskId: string): Promise<TaskHistoryItem[]> {
        return api.get<TaskHistoryItem[]>(`/TaskHistory/${taskId}`).then(res => res.data)
    }
}


