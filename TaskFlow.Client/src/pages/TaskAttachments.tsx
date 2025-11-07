import React, { useEffect, useState } from 'react'
import { useSelector } from 'react-redux'
import { RootState } from '../store'
import { attachmentsService, Attachment } from '../api/attachmentsService.js'

interface TaskAttachmentsProps {
    taskId: string
    taskTitle: string
}

const TaskAttachments: React.FC<TaskAttachmentsProps> = ({ taskId, taskTitle }) => {
    const auth = useSelector((state: RootState) => state.auth)
    const userId = auth?.userId

    const [attachments, setAttachments] = useState<Attachment[]>([])
    const [newFile, setNewFile] = useState<File | null>(null)
    const [editingId, setEditingId] = useState<string | null>(null)
    const [editingName, setEditingName] = useState('')
    const [loading, setLoading] = useState<boolean>(true)
    const [error, setError] = useState<string | null>(null)

    // Загрузка вложений
    useEffect(() => {
        const fetchAttachments = async () => {
            try {
                setLoading(true)
                const data = await attachmentsService.getByTask(taskId)
                setAttachments(data)
                setError(null)
            } catch (err: unknown) {
                console.error(err)
                setError(err instanceof Error ? err.message : 'Не удалось загрузить вложения')
            } finally {
                setLoading(false)
            }
        }
        fetchAttachments()
    }, [taskId])

    // Обновление taskTitle в существующих вложениях при изменении заголовка задачи
    useEffect(() => {
        setAttachments(prev => prev.map(att => ({ ...att, taskTitle })))
    }, [taskTitle])

    // Создание нового вложения
    const createAttachment = async () => {
        if (!newFile) {
            setError('Выберите файл для загрузки')
            return
        }
        setError(null)
        try {
            const formData = new FormData()
            formData.append('file', newFile)
            formData.append('taskId', taskId)
            const att = await attachmentsService.create(formData)
            setAttachments(prev => [...prev, att])
            setNewFile(null)
        } catch (err: unknown) {
            console.error(err)
            setError(err instanceof Error ? err.message : 'Не удалось создать вложение')
        }
    }

    // Начало редактирования имени файла
    const startEdit = (att: Attachment) => {
        setEditingId(att.id)
        setEditingName(att.fileName)
    }

    // Отмена редактирования
    const cancelEdit = () => {
        setEditingId(null)
        setEditingName('')
    }

    // Сохранение изменений
    const saveEdit = async (id: string) => {
        if (!editingName) {
            setError('Имя файла не может быть пустым')
            return
        }
        try {
            const attToUpdate = attachments.find(a => a.id === id)
            if (!attToUpdate) return
            const updated = await attachmentsService.update(id, {
                fileName: editingName,
                filePath: attToUpdate.filePath,
                fileSize: attToUpdate.fileSize
            })
            setAttachments(prev => prev.map(a => (a.id === id ? updated : a)))
            cancelEdit()
        } catch (err: unknown) {
            console.error(err)
            setError(err instanceof Error ? err.message : 'Не удалось обновить вложение')
        }
    }

    // Удаление вложения
    const removeAttachment = async (id: string) => {
        if (!window.confirm('Вы уверены, что хотите удалить вложение?')) return
        try {
            await attachmentsService.remove(id)
            setAttachments(prev => prev.filter(a => a.id !== id))
        } catch (err: unknown) {
            console.error(err)
            setError(err instanceof Error ? err.message : 'Не удалось удалить вложение')
        }
    }

    if (loading) return <div>Loading attachments...</div>

    return (
        <div style={{ marginTop: 20 }}>
            <h3>Attachments</h3>
            {error && <div style={{ color: 'red', marginBottom: 8 }}>{error}</div>}

            {/* Загрузка нового файла */}
            <div style={{ marginBottom: 12 }}>
                <input type="file" onChange={e => setNewFile(e.target.files?.[0] || null)} />
                <button onClick={createAttachment} disabled={!newFile} style={{ marginLeft: 8 }}>
                    Upload
                </button>
            </div>

            {attachments.length === 0 && <div>No attachments</div>}

            <ul>
                {attachments.map(att => (
                    <li key={att.id} style={{ marginBottom: 6 }}>
                        {editingId === att.id ? (
                            <>
                                <input
                                    value={editingName}
                                    onChange={e => setEditingName(e.target.value)}
                                    style={{ marginRight: 8 }}
                                />
                                <button onClick={() => saveEdit(att.id)}>Save</button>
                                <button onClick={cancelEdit} style={{ marginLeft: 4 }}>
                                    Cancel
                                </button>
                            </>
                        ) : (
                            <>
                                <a href={att.filePath} target="_blank" rel="noopener noreferrer">
                                    {att.fileName} ({att.fileSize} bytes)
                                </a>{' '}
                                <button onClick={() => startEdit(att)} style={{ marginLeft: 10 }}>
                                    Rename
                                </button>
                                <button onClick={() => removeAttachment(att.id)} style={{ marginLeft: 4 }}>
                                    Delete
                                </button>
                            </>
                        )}
                    </li>
                ))}
            </ul>
        </div>
    )
}

export default TaskAttachments

