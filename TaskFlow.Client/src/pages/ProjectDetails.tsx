import React, { useEffect, useState } from 'react'
import { useParams, useNavigate } from 'react-router-dom'
import { projectsService, Project, Task } from '../api/projectsService'

export default function ProjectDetails() {
    const { projectId } = useParams<{ projectId: string }>()
    const navigate = useNavigate()

    const [project, setProject] = useState<Project | null>(null)
    const [tasks, setTasks] = useState<Task[]>([])
    const [error, setError] = useState<string | null>(null)
    const [loading, setLoading] = useState(true)

    useEffect(() => {
        if (!projectId) return

        const normalizedId = String(projectId) // гарантированно строка

        const loadData = async () => {
            try {
                setLoading(true)
                const proj = await projectsService.getById(normalizedId)
                const tasksData = await projectsService.getTasks(normalizedId)

                setProject(proj)
                setTasks(tasksData)
                setError(null)
            } catch (e: any) {
                console.error('Failed to load project details', e)
                setError('Project not found')
            } finally {
                setLoading(false)
            }
        }

        loadData()
    }, [projectId])

    if (loading) {
        return <div className="card p-6">Loading...</div>
    }

    if (error || !project) {
        return <div className="card p-6 text-red-600">{error || 'Project not found.'}</div>
    }

    return (
        <div className="card p-6 shadow-lg rounded-lg bg-white">
            <div className="flex justify-between items-center mb-6">
                <h2 className="text-2xl font-semibold">{project.name}</h2>
                <button
                    className="btn btn-secondary"
                    onClick={() => navigate('/projects')}
                >
                    ← Back to Projects
                </button>
            </div>

            <p className="text-gray-700 mb-6">
                {project.description || 'No description provided.'}
            </p>

            <h3 className="text-xl font-semibold mb-3">Tasks</h3>

            {tasks.length === 0 ? (
                <div className="text-gray-500">No tasks found for this project.</div>
            ) : (
                <div className="grid gap-4">
                    {tasks.map(task => (
                        <div
                            key={task.id}
                            className="border rounded-lg p-4 hover:shadow transition bg-gray-50"
                        >
                            <div className="flex justify-between items-center mb-2">
                                <h4 className="font-medium">{task.title}</h4>
                                <span className="text-sm text-gray-500">
                                    Priority: {['Low', 'Medium', 'High'][task.priority] || '—'}
                                </span>
                            </div>
                            <p className="text-gray-700 mb-2">
                                {task.description || 'No description'}
                            </p>
                            <div className="text-sm text-gray-600 flex justify-between">
                                <span>Status: {['New', 'In Progress', 'Completed', 'Blocked', 'Archived'][task.status]}</span>
                                {task.assignedToName && <span>Assigned: {task.assignedToName}</span>}
                            </div>
                        </div>
                    ))}
                </div>
            )}
        </div>
    )
}
