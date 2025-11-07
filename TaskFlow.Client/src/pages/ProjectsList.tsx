import React, { useEffect, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { useSelector } from 'react-redux'
import { RootState } from '../store'
import { projectsService, Project } from '../api/projectsService'
import { tasksService, Task } from '../api/tasksService'

export default function ProjectsList() {
    const [projects, setProjects] = useState<Project[]>([])
    const [tasksMap, setTasksMap] = useState<Record<string, Task[]>>({})
    const navigate = useNavigate()
    const { role } = useSelector((state: RootState) => state.auth)

    const statusLabels = ['New', 'In Progress', 'Completed', 'Blocked', 'Archived']
    const statusColors = [
        'bg-blue-100 text-blue-800',
        'bg-yellow-100 text-yellow-800',
        'bg-green-100 text-green-800',
        'bg-gray-100 text-gray-800',
        'bg-purple-100 text-purple-800'
    ]

    const loadProjects = async () => {
        try {
            const allProjects = await projectsService.getAll()
            setProjects(allProjects)

            const tasksPromises = allProjects.map(async (p) => {
                const tasks = await tasksService.getByProject(p.id)
                return { projectId: p.id, tasks }
            })
            const results = await Promise.all(tasksPromises)
            const map: Record<string, Task[]> = {}
            results.forEach(r => { map[r.projectId] = r.tasks })
            setTasksMap(map)
        } catch (e) {
            console.error('Failed to load projects:', e)
        }
    }

    useEffect(() => {
        loadProjects()
    }, [])

    const createProject = () => navigate('/projects/new')
    const editProject = (id: string) => navigate(`/projects/edit/${id}`)
    const deleteProject = async (id: string) => {
        if (!window.confirm('Delete this project?')) return
        try {
            await projectsService.remove(id)
            await loadProjects() // актуализируем состояние
        } catch (e) {
            console.error(e)
        }
    }

    // 🔹 Вызываем reload после редактирования или добавления задачи
    const handleTaskChange = () => {
        loadProjects()
    }

    return (
        <div className="tasks-container">
            <div className="tasks-header flex justify-between items-center mb-4">
                <h2>Projects</h2>
                {role === 'Admin' && (
                    <button className="btn btn-primary" onClick={createProject}>➕ New Project</button>
                )}
            </div>

            {projects.length === 0 && <div className="no-tasks">No projects found.</div>}

            <div className="tasks-grid">
                {projects.map(p => {
                    const tasks = tasksMap[p.id] || []
                    const statusCount: Record<number, number> = tasks.reduce((acc, t) => {
                        acc[t.status] = (acc[t.status] || 0) + 1
                        return acc
                    }, {} as Record<number, number>)

                    return (
                        <div key={p.id} className="task-card">
                            <h3 className="task-title">{p.name}</h3>
                            <p className="task-project text-gray-600">{p.description || 'No description'}</p>

                            <div className="task-meta flex flex-wrap gap-2 mt-2">
                                {statusLabels.map((label, idx) => (
                                    <span key={idx} className={`task-status ${statusColors[idx]}`}>
                                        {label}: {statusCount[idx] || 0}
                                    </span>
                                ))}
                            </div>

                            {role === 'Admin' && (
                                <div className="mt-2 flex gap-2 flex-wrap">
                                    <button className="btn btn-secondary" onClick={() => editProject(p.id)}>✏️ Edit</button>
                                    <button className="btn btn-danger" onClick={() => deleteProject(p.id)}>🗑 Delete</button>
                                </div>
                            )}
                        </div>
                    )
                })}
            </div>
        </div>
    )
}
