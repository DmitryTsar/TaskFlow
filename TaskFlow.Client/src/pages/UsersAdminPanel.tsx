import React, { useEffect, useState } from 'react'
import { useSelector } from 'react-redux'
import { RootState } from '../store'
import { usersService, User, Project, Task } from '../api/usersService.js'
import api from '../api/axios.js'

export default function UsersAdminPanel() {
    const auth = useSelector((state: RootState) => state.auth)
    const isAdmin = auth.role === 'Admin'

    const [users, setUsers] = useState<User[]>([])
    const [selectedUser, setSelectedUser] = useState<User | null>(null)
    const [loading, setLoading] = useState(true)
    const [error, setError] = useState<string | null>(null)
    const [success, setSuccess] = useState<string | null>(null)

    const [userName, setUserName] = useState('')
    const [email, setEmail] = useState('')
    const [role, setRole] = useState<'Admin' | 'Manager' | 'User'>('User')
    const [password, setPassword] = useState('')
    const [confirmPassword, setConfirmPassword] = useState('')
    const [fieldErrors, setFieldErrors] = useState<{ [key: string]: string }>({})

    const [projects, setProjects] = useState<Project[]>([])
    const [createdTasks, setCreatedTasks] = useState<Task[]>([])
    const [assignedTasks, setAssignedTasks] = useState<Task[]>([])

    const mapRoleToString = (r: string | number | undefined): 'Admin' | 'Manager' | 'User' => {
        if (r === 'Admin' || r === 0) return 'Admin'
        if (r === 'Manager' || r === 1) return 'Manager'
        return 'User'
    }

    const fetchUsers = async () => {
        try {
            setLoading(true)
            if (isAdmin) {
                const allUsers = await usersService.getAll()
                const usersWithStringRoles = allUsers.map(u => ({ ...u, role: mapRoleToString(u.role) }))
                setUsers(usersWithStringRoles.filter(u => u.id !== auth.userId))
            } else if (auth.userId) {
                const me = await usersService.getById(auth.userId)
                me.role = mapRoleToString(me.role)
                setUsers([me])
                await selectUser(me)
            }
        } catch (err: any) {
            setError(err?.message || 'Failed to load users')
        } finally {
            setLoading(false)
        }
    }

    useEffect(() => { fetchUsers() }, [auth.userId, isAdmin])

    const selectUser = async (user: User | null) => {
        setSelectedUser(user)
        setFieldErrors({})
        setError(null)
        setSuccess(null)

        if (!user) {
            setUserName(''); setEmail(''); setRole('User'); setPassword(''); setConfirmPassword('')
            setProjects([]); setCreatedTasks([]); setAssignedTasks([])
            return
        }

        setUserName(user.userName)
        setEmail(user.email)
        setRole(mapRoleToString(user.role ?? 'User'))
        setPassword(''); setConfirmPassword('')

        if (isAdmin || user.id === auth.userId) {
            try {
                const [proj, created, assigned] = await Promise.all([
                    usersService.getProjects(user.id),
                    usersService.getCreatedTasks(user.id),
                    usersService.getAssignedTasks(user.id),
                ])
                setProjects(proj)
                setCreatedTasks(created)
                setAssignedTasks(assigned)
            } catch (err: any) {
                console.error(err)
                setError('Failed to load user projects/tasks')
            }
        }
    }

    const handleSaveUser = async () => {
        if (!selectedUser) return
        setFieldErrors({}); setError(null); setSuccess(null)

        try {
            if (!selectedUser.id) {
                if (!userName) return setFieldErrors({ userName: 'Required' })
                if (!email) return setFieldErrors({ email: 'Required' })
                if (!password) return setFieldErrors({ password: 'Required' })
                if (password !== confirmPassword) return setFieldErrors({ confirmPassword: 'Passwords do not match' })

                await usersService.create({ userName, email, password, role })
                setSuccess('User created successfully')
                await selectUser(null)
                await fetchUsers()
                return
            }

            if (!isAdmin && selectedUser.id === auth.userId) {
                if (password && password !== confirmPassword) return setFieldErrors({ confirmPassword: 'Passwords do not match' })
                if (password) await api.put(`/Users/${selectedUser.id}/password`, { password })
                setSuccess(password ? 'Password updated' : 'No changes made')
                setPassword(''); setConfirmPassword('')
                return
            }

            if (!userName) return setFieldErrors({ userName: 'Required' })
            if (!email) return setFieldErrors({ email: 'Required' })

            await usersService.update({ id: selectedUser.id, userName, email, role })
            if (password) await api.put(`/Users/${selectedUser.id}/password`, { password })
            setSuccess('User updated successfully')
            await fetchUsers()
        } catch (err: any) {
            console.error(err)
            setError(err?.message || 'Failed to save user')
        }
    }

    const handleDeleteUser = async (userId: string) => {
        if (!window.confirm('Are you sure?')) return
        try {
            await usersService.remove(userId)
            setSuccess('User deleted')
            if (selectedUser?.id === userId) selectUser(null)
            await fetchUsers()
        } catch (err: any) {
            console.error(err)
            setError(err?.message || 'Failed to delete user')
        }
    }

    if (loading) return <div>Loading...</div>

    const roleLabel = (role: string) => role || 'User'

    return (
        <div className="content" style={{ gap: 32 }}>
            {/* Users List */}
            <div style={{ flex: 1, minWidth: 320 }}>
                <div className="card">
                    <h2>{isAdmin ? 'Users List' : 'Your Profile'}</h2>

                    {error && <div className="error-msg">{error}</div>}
                    {success && <div style={{ color: 'green', marginBottom: 12 }}>{success}</div>}

                    <ul style={{ listStyle: 'none', padding: 0 }}>
                        {users.map(u => (
                            <li
                                key={u.id}
                                onClick={() => isAdmin && selectUser(u)}
                                className={`card ${selectedUser?.id === u.id ? 'bg-blue-100' : ''}`}
                                style={{ marginBottom: 8, cursor: isAdmin ? 'pointer' : 'default', padding: 12, display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}
                            >
                                <span>{u.userName} — {u.email} ({roleLabel(u.role ?? 'User')})</span>
                                {isAdmin && u.id !== auth.userId && (
                                    <button className="btn btn-danger" onClick={() => handleDeleteUser(u.id)}>Delete</button>
                                )}
                            </li>
                        ))}
                    </ul>

                    {isAdmin && (
                        <button className="btn btn-primary" onClick={() => selectUser({ id: '', userName: '', email: '', role: 'User' })}>
                            + Create New User
                        </button>
                    )}
                </div>
            </div>

            {/* User Form */}
            {selectedUser && (
                <div style={{ flex: 1, minWidth: 320 }}>
                    <div className="card">
                        <h3>{selectedUser.id ? 'Edit User' : 'Create User'}</h3>

                        <div className="task-form-field">
                            <label>Username:</label>
                            <input value={userName} onChange={e => setUserName(e.target.value)} />
                            {fieldErrors.userName && <div className="error-msg">{fieldErrors.userName}</div>}
                        </div>

                        <div className="task-form-field">
                            <label>Email:</label>
                            <input value={email} onChange={e => setEmail(e.target.value)} />
                            {fieldErrors.email && <div className="error-msg">{fieldErrors.email}</div>}
                        </div>

                        {isAdmin && (
                            <div className="task-form-field">
                                <label>Role:</label>
                                <select value={role} onChange={e => setRole(e.target.value as 'Admin' | 'Manager' | 'User')}>
                                    <option value="Admin">Admin</option>
                                    <option value="Manager">Manager</option>
                                    <option value="User">User</option>
                                </select>
                            </div>
                        )}

                        {(!selectedUser.id || (!isAdmin && selectedUser.id === auth.userId)) && (
                            <>
                                <div className="task-form-field">
                                    <label>Password:</label>
                                    <input type="password" value={password} onChange={e => setPassword(e.target.value)} />
                                    {fieldErrors.password && <div className="error-msg">{fieldErrors.password}</div>}
                                </div>

                                <div className="task-form-field">
                                    <label>Confirm Password:</label>
                                    <input type="password" value={confirmPassword} onChange={e => setConfirmPassword(e.target.value)} />
                                    {fieldErrors.confirmPassword && <div className="error-msg">{fieldErrors.confirmPassword}</div>}
                                </div>
                            </>
                        )}

                        <button className="btn btn-primary" onClick={handleSaveUser}>Save</button>

                        {!isAdmin && selectedUser.id === auth.userId && (
                            <button className="btn btn-danger" onClick={() => handleDeleteUser(auth.userId!)}>Delete Account</button>
                        )}

                        {(isAdmin || selectedUser.id === auth.userId) && selectedUser.id && (
                            <>
                                <h4>Projects</h4>
                                <ul>{projects.map(p => <li key={p.id}>{p.name}</li>)}</ul>

                                <h4>Created Tasks</h4>
                                <ul>{createdTasks.map(t => <li key={t.id}>{t.title}</li>)}</ul>

                                <h4>Assigned Tasks</h4>
                                <ul>{assignedTasks.map(t => <li key={t.id}>{t.title}</li>)}</ul>
                            </>
                        )}
                    </div>
                </div>
            )}
        </div>
    )
}
