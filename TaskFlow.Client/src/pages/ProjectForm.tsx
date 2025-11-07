import React, { useEffect, useState } from 'react'
import { useNavigate, useParams, useLocation } from 'react-router-dom'
import { useSelector } from 'react-redux'
import { RootState } from '../store'
import { projectsService, Project } from '../api/projectsService'

export default function ProjectForm() {
    const { id } = useParams<{ id: string }>()
    const navigate = useNavigate()
    const location = useLocation()
    const { userId, role } = useSelector((state: RootState) => state.auth)
    const fromDetails = location.state?.fromDetails as boolean | undefined

    const [name, setName] = useState('')
    const [description, setDescription] = useState('')
    const [error, setError] = useState<string | null>(null)
    const [loading, setLoading] = useState(false)
    const [saving, setSaving] = useState(false)

    useEffect(() => {
        if (!id) return
            ; (async () => {
                setLoading(true)
                try {
                    const project: Project = await projectsService.getById(id)
                    setName(project.name)
                    setDescription(project.description || '')
                } catch (err: any) {
                    setError(err?.message || 'Failed to load project')
                } finally {
                    setLoading(false)
                }
            })()
    }, [id])

    if (role !== 'Admin')
        return (
            <div className="card p-6">
                <h2>Unauthorized</h2>
                <p>You are not allowed to manage projects.</p>
            </div>
        )

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault()
        setError(null)

        if (!name.trim() || !userId) {
            setError('Project name is required and user must be logged in.')
            return
        }

        setSaving(true)
        try {
            if (id) {
                await projectsService.update({ id, name, description })
                if (fromDetails) navigate(-1)
                else navigate('/projects')
            } else {
                await projectsService.create({ name, description, ownerId: userId })
                navigate('/projects')
            }
        } catch (err: any) {
            setError(err?.message || 'Failed to save project')
        } finally {
            setSaving(false)
        }
    }

    if (loading)
        return (
            <div className="card">
                <h2>Loading...</h2>
            </div>
        )

    return (
        <div className="card" style={{ maxWidth: '600px', margin: '40px auto' }}>
            <h2 className="project-title">{id ? 'Edit Project' : 'Create New Project'}</h2>

            {error && <div className="error-msg">{error}</div>}

            <form onSubmit={handleSubmit} className="flex flex-col" style={{ gap: '16px' }}>
                <div>
                    <label htmlFor="name" style={{ display: 'block', fontWeight: 500, marginBottom: '6px' }}>
                        Project Name:
                    </label>
                    <input
                        id="name"
                        type="text"
                        value={name}
                        onChange={e => setName(e.target.value)}
                        required
                        style={{
                            width: '100%',
                            padding: '8px 10px',
                            border: '1px solid #d1d5db',
                            borderRadius: '8px',
                            fontSize: '1rem'
                        }}
                        placeholder="Enter project name"
                    />
                </div>

                <div>
                    <label htmlFor="desc" style={{ display: 'block', fontWeight: 500, marginBottom: '6px' }}>
                        Description:
                    </label>
                    <textarea
                        id="desc"
                        value={description}
                        onChange={e => setDescription(e.target.value)}
                        rows={4}
                        style={{
                            width: '100%',
                            padding: '8px 10px',
                            border: '1px solid #d1d5db',
                            borderRadius: '8px',
                            resize: 'vertical',
                            fontSize: '1rem'
                        }}
                        placeholder="Enter short project description"
                    />
                </div>

                <div style={{ display: 'flex', justifyContent: 'space-between', marginTop: '10px' }}>
                    <button type="button" className="btn btn-secondary" onClick={() => navigate(-1)}>
                        ← Back
                    </button>

                    <button type="submit" className="btn btn-primary" disabled={saving}>
                        {saving ? 'Saving...' : id ? 'Save Changes' : 'Create Project'}
                    </button>
                </div>
            </form>
        </div>
    )
}
