import React, { useEffect, useState } from 'react'
import { taskHistoryService, TaskHistoryItem } from '../api/taskHistoryService.js'

interface TaskHistoryProps {
    taskId: string
}

const TaskHistory: React.FC<TaskHistoryProps> = ({ taskId }) => {
    const [history, setHistory] = useState<TaskHistoryItem[]>([])
    const [loading, setLoading] = useState<boolean>(true)
    const [error, setError] = useState<string | null>(null)

    useEffect(() => {
        if (!taskId) return

        const fetchHistory = async () => {
            try {
                setLoading(true)
                const data = await taskHistoryService.getByTask(taskId)
                setHistory(data)
                setError(null)
            } catch (err: unknown) {
                console.error(err)
                if (err instanceof Error) setError(err.message)
                else setError('Ошибка при загрузке истории')
            } finally {
                setLoading(false)
            }
        }

        fetchHistory()
    }, [taskId])

    if (loading) return <div>Loading history...</div>
    if (error) return <div style={{ color: 'red' }}>{error}</div>
    if (history.length === 0) return <div>No change history.</div>

    return (
        <div style={{ marginTop: 20 }}>
            <h3>History of changes</h3>
            <table style={{ width: '100%', borderCollapse: 'collapse' }}>
                <thead>
                    <tr>
                        <th style={thStyle}>Property</th>
                        <th style={thStyle}>Old Value</th>
                        <th style={thStyle}>New Value</th>
                        <th style={thStyle}>Changed by</th>
                        <th style={thStyle}>Date</th>
                    </tr>
                </thead>
                <tbody>
                    {history.map((item, index) => (
                        <tr key={index} style={{ borderBottom: '1px solid #ddd' }}>
                            <td style={tdStyle}>{item.propertyName}</td>
                            <td style={tdStyle}>{item.oldValue ?? '-'}</td>
                            <td style={tdStyle}>{item.newValue ?? '-'}</td>
                            <td style={tdStyle}>{item.changedByName}</td>
                            <td style={tdStyle}>{new Date(item.changedAt).toLocaleString()}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    )
}

// 👇 небольшие inline-стили для таблицы
const thStyle: React.CSSProperties = {
    textAlign: 'left',
    padding: '6px 8px',
    borderBottom: '2px solid #999'
}

const tdStyle: React.CSSProperties = {
    padding: '4px 8px'
}

export default TaskHistory

