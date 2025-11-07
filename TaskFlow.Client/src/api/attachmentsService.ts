import api from './axios.js'

export interface Attachment {
    id: string
    fileName: string
    filePath: string
    fileSize: number
    uploadedAt: string
    taskId: string
    taskTitle: string
}

export const attachmentsService = {
    getAll(): Promise<Attachment[]> {
        return api.get<Attachment[]>('/Attachments').then(res => res.data)
    },
    getById(id: string): Promise<Attachment> {
        return api.get<Attachment>(`/Attachments/${id}`).then(res => res.data)
    },
    create(formData: FormData): Promise<Attachment> {
        return api.post<Attachment>('/Attachments', formData, {
            headers: { 'Content-Type': 'multipart/form-data' }
        }).then(res => res.data)
    },
    update(id: string, payload: { fileName: string; filePath: string; fileSize: number }): Promise<Attachment> {
        return api.put<Attachment>(`/Attachments/${id}`, payload).then(res => res.data)
    },
    remove(id: string): Promise<void> {
        return api.delete(`/Attachments/${id}`).then(() => { })
    },
    getByTask(taskId: string): Promise<Attachment[]> {
        return api.get<Attachment[]>(`/Attachments/task/${taskId}`).then(res => res.data)
    }
}


