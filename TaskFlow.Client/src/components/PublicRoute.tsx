import React from 'react'
import { Navigate, Outlet } from 'react-router-dom'
import { useSelector } from 'react-redux'
import { RootState } from '../store'

export default function PublicRoute() {
    const { token, authInitialized } = useSelector((state: RootState) => state.auth)

    if (!authInitialized) return null // пока инициализация

    return token ? <Navigate to="/tasks" replace /> : <Outlet />
}

