import React from 'react'
import { Navigate, Outlet } from 'react-router-dom'
import { useSelector } from 'react-redux'
import { RootState } from '../store'

export default function ProtectedRoute() {
    const { token, authInitialized } = useSelector((state: RootState) => state.auth)

    if (!authInitialized) return null // ждем инициализацию

    return token ? <Outlet /> : <Navigate to="/login" replace />
}

