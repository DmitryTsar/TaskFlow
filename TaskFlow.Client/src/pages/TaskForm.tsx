import React, { useEffect, useState } from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import { useSelector } from 'react-redux'
import { RootState } from '../store'
import { tasksService } from '../api/tasksService'
import { projectsService, Project } from '../api/projectsService'
import { usersService, User } from '../api/usersService'

type TaskFormProps = {
    onTaskChange?: () => void // колбэк для обновления ProjectsList
}

export default function TaskForm({ onTaskChange }: TaskFormProps) {
    const { taskId, projectId } = useParams<{ taskId?: string; projectId?: string }>()
    const navigate = useNavigate()
    const { userId, role } = useSelector((state: RootState) => state.auth)

    const [title, setTitle] = useState('')
    const [description, setDescription] = useState('')
    const [project, setProject] = useState(projectId || '')
    const [assignedToId, setAssignedToId] = useState('')
    const [status, setStatus] = useState<0 | 1 | 2 | 3 | 4>(0)
    const [priority, setPriority] = useState<0 | 1 | 2>(0)
    const [projects, setProjects] = useState<Project[]>([])
    const [users, setUsers] = useState<User[]>([])
    const [error, setError] = useState<string | null>(null)
    const [loading, setLoading] = useState(true)
    const [saving, setSaving] = useState(false)

    useEffect(() => {
        const loadData = async () => {
            try {
                const [projectsData, usersData] = await Promise.all([
                    projectsService.getAll(),
                    usersService.getAll()
                ])
                setProjects(projectsData)
                setUsers(usersData)

                if (taskId) {
                    const task = await tasksService.getById(taskId)
                    setTitle(task.title || '')
                    setDescription(task.description || '')
                    setProject(String(task.projectId) || '')
                    setAssignedToId(task.assignedToId || '')
                    setStatus(task.status)
                    setPriority(task.priority)
                } else if (!project && projectsData.length > 0) {
                    setProject(String(projectsData[0].id))
                }
            } catch (e: any) {
                setError(e?.message || 'Failed to load data')
            } finally {
                setLoading(false)
            }
        }
        loadData()
    }, [taskId, project])

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault()
        if (!title || !description || !project || !assignedToId || !userId) {
            setError('Please fill all fields')
            return
        }
        setSaving(true)
        setError(null)

        try {
            if (taskId) {
                await tasksService.update({
                    id: taskId,
                    title,
                    description,
                    assignedToId,
                    status,
                    priority,
                    updatedById: userId
                })
            } else {
                await tasksService.create({
                    title,
                    description,
                    projectId: String(project),
                    assignedToId,
                    createdById: userId,
                    status,
                    priority
                })
            }

            // 🔹 вызываем колбэк для обновления ProjectsList
            onTaskChange?.()

            // 🔹 переходим на TasksList
            navigate('/tasks')
        } catch (e: any) {
            setError(e?.message || 'Failed to save task')
        } finally {
            setSaving(false)
        }
    }

    if (loading) return <div className="no-tasks">Loading...</div>

    const assignableUsers = role === 'Admin' ? users : users.filter(u => u.id === userId)

    return (
        <div className="card" style={{ maxWidth: 500, margin: '40px auto', padding: 32, gap: 24 }}>
            <h2 style={{ textAlign: 'center', marginBottom: 24, color: '#1d4ed8' }}>
                {taskId ? 'Edit Task' : 'Create Task'}
            </h2>

            {error && <div className="error-msg">{error}</div>}

            <form onSubmit={handleSubmit} style={{ display: 'flex', flexDirection: 'column', gap: 20 }}>
                <div style={{ display: 'flex', flexDirection: 'column', gap: 6 }}>
                    <label style={{ fontWeight: 500 }}>Title</label>
                    <input
                        type="text"
                        value={title}
                        onChange={e => setTitle(e.target.value)}
                        required
                        style={{ padding: 10, borderRadius: 6, border: '1px solid #d1d5db', fontSize: 14 }}
                    />
                </div>

                <div style={{ display: 'flex', flexDirection: 'column', gap: 6 }}>
                    <label style={{ fontWeight: 500 }}>Description</label>
                    <textarea
                        value={description}
                        onChange={e => setDescription(e.target.value)}
                        rows={5}
                        required
                        style={{ padding: 10, borderRadius: 6, border: '1px solid #d1d5db', fontSize: 14 }}
                    />
                </div>

                <div style={{ display: 'flex', gap: 12 }}>
                    <div style={{ flex: 1, display: 'flex', flexDirection: 'column', gap: 6 }}>
                        <label style={{ fontWeight: 500 }}>Project</label>
                        <select
                            value={project}
                            onChange={e => setProject(e.target.value)}
                            disabled={!!taskId}
                            required
                            style={{ padding: 10, borderRadius: 6, border: '1px solid #d1d5db', fontSize: 14 }}
                        >
                            <option value="">-- Select project --</option>
                            {projects.map(p => (
                                <option key={p.id} value={p.id}>
                                    {p.name}
                                </option>
                            ))}
                        </select>
                    </div>

                    <div style={{ flex: 1, display: 'flex', flexDirection: 'column', gap: 6 }}>
                        <label style={{ fontWeight: 500 }}>Assign To</label>
                        <select
                            value={assignedToId}
                            onChange={e => setAssignedToId(e.target.value)}
                            required
                            style={{ padding: 10, borderRadius: 6, border: '1px solid #d1d5db', fontSize: 14 }}
                        >
                            <option value="">-- Select user --</option>
                            {assignableUsers.map(u => (
                                <option key={u.id} value={u.id}>
                                    {u.fullName || u.userName || u.email}
                                </option>
                            ))}
                        </select>
                    </div>
                </div>

                <div style={{ display: 'flex', gap: 12 }}>
                    <div style={{ flex: 1, display: 'flex', flexDirection: 'column', gap: 6 }}>
                        <label style={{ fontWeight: 500 }}>Status</label>
                        <select
                            value={status}
                            onChange={e => setStatus(Number(e.target.value) as 0 | 1 | 2 | 3 | 4)}
                            style={{ padding: 10, borderRadius: 6, border: '1px solid #d1d5db', fontSize: 14 }}
                        >
                            <option value={0}>New</option>
                            <option value={1}>In Progress</option>
                            <option value={2}>Completed</option>
                            <option value={3}>Blocked</option>
                            <option value={4}>Archived</option>
                        </select>
                    </div>

                    <div style={{ flex: 1, display: 'flex', flexDirection: 'column', gap: 6 }}>
                        <label style={{ fontWeight: 500 }}>Priority</label>
                        <select
                            value={priority}
                            onChange={e => setPriority(Number(e.target.value) as 0 | 1 | 2)}
                            style={{ padding: 10, borderRadius: 6, border: '1px solid #d1d5db', fontSize: 14 }}
                        >
                            <option value={0}>Low</option>
                            <option value={1}>Medium</option>
                            <option value={2}>High</option>
                        </select>
                    </div>
                </div>

                <button
                    type="submit"
                    className="btn btn-primary"
                    disabled={saving}
                    style={{ marginTop: 10 }}
                >
                    {saving ? 'Saving...' : taskId ? 'Update Task' : 'Create Task'}
                </button>
            </form>
        </div>
    )
}
