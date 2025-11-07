import React, { useState } from 'react'
import { useSelector } from 'react-redux'
import { RootState } from '../store'
import api from '../api/axios.js'

type FieldErrors = {
    password?: string
    confirmPassword?: string
}

// Конвертация роли в строку
const mapRoleToString = (r: string | null | undefined): 'Admin' | 'Manager' | 'User' => {
    if (r === 'Admin') return 'Admin'
    if (r === 'Manager') return 'Manager'
    return 'User'
}

export default function ProfilePanel() {
    const auth = useSelector((state: RootState) => state.auth)
    const [password, setPassword] = useState('')
    const [confirmPassword, setConfirmPassword] = useState('')
    const [error, setError] = useState<string | null>(null)
    const [success, setSuccess] = useState<string | null>(null)
    const [fieldErrors, setFieldErrors] = useState<FieldErrors>({})

    const handleSavePassword = async () => {
        setFieldErrors({})
        setError(null)
        setSuccess(null)

        if (!auth.userId) {
            setError('User not found')
            return
        }

        if (!password) {
            setFieldErrors({ password: 'Required' })
            return
        }

        if (password !== confirmPassword) {
            setFieldErrors({ confirmPassword: 'Passwords do not match' })
            return
        }

        try {
            await api.put(`/Users/${auth.userId}/password`, { password })
            setSuccess('Password updated successfully')
            setPassword('')
            setConfirmPassword('')
        } catch (err: any) {
            console.error(err)
            setError(err?.message || 'Failed to update password')
        }
    }

    // Роль пользователя в виде строки
    const roleLabel = mapRoleToString(auth.role)

    return (
        <div style={{ maxWidth: 400, margin: '0 auto' }}>
            <h2>Your Profile</h2>
            {error && <div style={{ color: 'red', marginBottom: 12 }}>{error}</div>}
            {success && <div style={{ color: 'green', marginBottom: 12 }}>{success}</div>}

            <div style={{ marginBottom: 8 }}>
                <label>Username:</label>
                <input value={auth.userName ?? ''} disabled style={inputStyle} />
            </div>

            <div style={{ marginBottom: 8 }}>
                <label>Email:</label>
                <input value={auth.email ?? ''} disabled style={inputStyle} />
            </div>

            <div style={{ marginBottom: 12 }}>
                <strong>Role:</strong> {roleLabel}
            </div>

            <h3>Change Password</h3>
            <div style={{ marginBottom: 8 }}>
                <label>New Password:</label>
                <input
                    type="password"
                    value={password}
                    onChange={e => setPassword(e.target.value)}
                    style={inputStyle}
                />
                {fieldErrors.password && <div style={fieldErrorStyle}>{fieldErrors.password}</div>}
            </div>

            <div style={{ marginBottom: 8 }}>
                <label>Confirm Password:</label>
                <input
                    type="password"
                    value={confirmPassword}
                    onChange={e => setConfirmPassword(e.target.value)}
                    style={inputStyle}
                />
                {fieldErrors.confirmPassword && <div style={fieldErrorStyle}>{fieldErrors.confirmPassword}</div>}
            </div>

            <button onClick={handleSavePassword} style={buttonStyle}>Save</button>
        </div>
    )
}

// Стили
const inputStyle: React.CSSProperties = {
    width: '100%',
    padding: 6,
    borderRadius: 4,
    border: '1px solid #ccc',
    fontSize: 14,
    boxSizing: 'border-box'
}

const fieldErrorStyle: React.CSSProperties = {
    color: 'red',
    fontSize: 12,
    marginTop: 2
}

const buttonStyle: React.CSSProperties = {
    marginTop: 12,
    padding: '8px 16px',
    borderRadius: 4,
    border: 'none',
    backgroundColor: '#007bff',
    color: '#fff',
    cursor: 'pointer'
}

