import api from './axios.js'

// Тип комментария
export type CommentItem = {
    id: string
    content: string
    createdAt: string
    authorId: string
    authorName: string
    taskId: string
    taskTitle: string
}

export const commentsService = {
    getAll(): Promise<CommentItem[]> {
        return api.get<CommentItem[]>('/Comments').then(res => res.data)
    },
    getById(id: string): Promise<CommentItem> {
        return api.get<CommentItem>(`/Comments/${id}`).then(res => res.data)
    },
    create(payload: { content: string; authorId: string; taskId: string }): Promise<CommentItem> {
        return api.post<CommentItem>('/Comments', payload).then(res => res.data)
    },
    update(payload: { id: string; content: string }): Promise<CommentItem> {
        return api.put<CommentItem>(`/Comments/${payload.id}`, payload).then(res => res.data)
    },
    remove(id: string): Promise<void> {
        return api.delete(`/Comments/${id}`).then(() => { })
    },
    getByTask(taskId: string): Promise<CommentItem[]> {
        return api.get<CommentItem[]>(`/Comments/task/${taskId}`).then(res => res.data)
    }
}


