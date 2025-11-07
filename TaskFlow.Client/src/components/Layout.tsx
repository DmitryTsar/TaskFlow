import React from 'react'
import { Link, Outlet, useNavigate } from 'react-router-dom'
import { useDispatch, useSelector } from 'react-redux'
import { logout } from '../store/slices/authSlice'
import { RootState, AppDispatch } from '../store'

export default function Layout() {
    const { token, userName, role } = useSelector((state: RootState) => state.auth)
    const dispatch = useDispatch<AppDispatch>()
    const navigate = useNavigate()

    const doLogout = () => {
        dispatch(logout())
        navigate('/login')
    }

    const isAdmin = role === 'Admin'

    const navLinks = token
        ? [
            { to: '/tasks', label: 'Tasks' },
            { to: '/projects', label: 'Projects' },
            ...(isAdmin
                ? [
                    { to: '/users', label: 'Users' },
                    { to: '/profile', label: 'My Profile' }
                ]
                : [{ to: '/profile', label: 'Profile' }])
        ]
        : []

    return (
        <div className="app-container">
            <div className="topbar">
                <div>
                    {token &&
                        navLinks.map((link, idx) => (
                            <span key={link.to}>
                                <Link to={link.to}>{link.label}</Link>
                                {idx < navLinks.length - 1 && ' '}
                            </span>
                        ))}
                </div>

                <div>
                    {token ? (
                        <>
                            <span style={{ marginRight: 12 }}>
                                {userName} ({role || 'User'})
                            </span>
                            <button onClick={doLogout}>Logout</button>
                        </>
                    ) : (
                        <Link to="/login">Login</Link>
                    )}
                </div>
            </div>

            <div className="content">
                <Outlet />
            </div>
        </div>
    )
}
