import React, { useEffect, useState } from 'react'
import { useSelector } from 'react-redux'
import { RootState } from '../store'
import { commentsService, CommentItem } from '../api/commentsService.js'

interface TaskCommentsProps {
    taskId: string
    taskTitle: string
}

const TaskComments: React.FC<TaskCommentsProps> = ({ taskId, taskTitle }) => {
    const auth = useSelector((state: RootState) => state.auth)
    const authorId = auth?.userId

    const [comments, setComments] = useState<CommentItem[]>([])
    const [newContent, setNewContent] = useState('')
    const [editingId, setEditingId] = useState<string | null>(null)
    const [editingContent, setEditingContent] = useState('')
    const [error, setError] = useState<string | null>(null)

    // Загрузка комментариев
    const loadComments = async () => {
        try {
            const data = await commentsService.getByTask(taskId)
            setComments(data.map(c => ({ ...c, taskTitle }))) // вставляем актуальный taskTitle
            setError(null)
        } catch (err: unknown) {
            console.error(err)
            setError(err instanceof Error ? err.message : 'Не удалось загрузить комментарии')
        }
    }

    useEffect(() => {
        loadComments()
    }, [taskId, taskTitle]) // обновляем при изменении taskTitle

    // Создание нового комментария
    const createComment = async () => {
        if (!newContent || !authorId) {
            setError('Невозможно создать комментарий: отсутствует текст или автор')
            return
        }
        setError(null)
        try {
            const comment = await commentsService.create({ content: newContent, authorId, taskId })
            setComments(prev => [...prev, { ...comment, taskTitle }])
            setNewContent('')
        } catch (err: unknown) {
            console.error(err)
            setError(err instanceof Error ? err.message : 'Не удалось создать комментарий')
        }
    }

    // Удаление комментария
    const removeComment = async (id: string) => {
        if (!window.confirm('Вы уверены, что хотите удалить комментарий?')) return
        try {
            await commentsService.remove(id)
            setComments(prev => prev.filter(c => c.id !== id))
        } catch (err: unknown) {
            console.error(err)
            setError(err instanceof Error ? err.message : 'Не удалось удалить комментарий')
        }
    }

    // Начало редактирования комментария
    const startEdit = (comment: CommentItem) => {
        setEditingId(comment.id)
        setEditingContent(comment.content)
    }

    // Отмена редактирования
    const cancelEdit = () => {
        setEditingId(null)
        setEditingContent('')
    }

    // Сохранение изменений
    const saveEdit = async (id: string) => {
        if (!editingContent) {
            setError('Комментарий не может быть пустым')
            return
        }
        try {
            const updated = await commentsService.update({ id, content: editingContent })
            setComments(prev => prev.map(c => (c.id === id ? { ...updated, taskTitle } : c)))
            cancelEdit()
        } catch (err: unknown) {
            console.error(err)
            setError(err instanceof Error ? err.message : 'Не удалось обновить комментарий')
        }
    }

    return (
        <div style={{ marginTop: 20 }}>
            <h3>Comments for: {taskTitle}</h3>
            {error && <div style={{ color: 'red', marginBottom: 8 }}>{error}</div>}

            {/* Создание нового комментария */}
            <div style={{ marginBottom: 12 }}>
                <input
                    type="text"
                    placeholder="New comment"
                    value={newContent}
                    onChange={e => setNewContent(e.target.value)}
                    style={{ width: '80%', padding: 4, marginRight: 8 }}
                />
                <button onClick={createComment}>Add</button>
            </div>

            {/* Список комментариев */}
            <ul>
                {comments.map(c => (
                    <li key={c.id} style={{ marginBottom: 6 }}>
                        <b>{c.authorName}</b> ({new Date(c.createdAt).toLocaleString()}):{' '}
                        {editingId === c.id ? (
                            <>
                                <input
                                    value={editingContent}
                                    onChange={e => setEditingContent(e.target.value)}
                                    style={{ marginRight: 8 }}
                                />
                                <button onClick={() => saveEdit(c.id)}>Save</button>
                                <button onClick={cancelEdit} style={{ marginLeft: 4 }}>
                                    Cancel
                                </button>
                            </>
                        ) : (
                            <>
                                {c.content}
                                {authorId === c.authorId && (
                                    <>
                                        <button onClick={() => startEdit(c)} style={{ marginLeft: 10 }}>
                                            Edit
                                        </button>
                                        <button onClick={() => removeComment(c.id)} style={{ marginLeft: 4 }}>
                                            Delete
                                        </button>
                                    </>
                                )}
                            </>
                        )}
                    </li>
                ))}
            </ul>
        </div>
    )
}

export default TaskComments

