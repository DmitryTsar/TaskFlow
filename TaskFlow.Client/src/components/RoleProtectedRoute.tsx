import React from 'react'
import { Navigate, Outlet } from 'react-router-dom'
import { useSelector } from 'react-redux'
import { RootState } from '../store'

interface Props {
    roles: string[] // С‚РµРїРµСЂСЊ РјР°СЃСЃРёРІ СЃС‚СЂРѕРєРѕРІС‹С… СЂРѕР»РµР№
}

export default function RoleProtectedRoute({ roles }: Props) {
    const { token, role, authInitialized } = useSelector((state: RootState) => state.auth)

    // РїРѕРєР° auth РЅРµ РёРЅРёС†РёР°Р»РёР·РёСЂРѕРІР°РЅ
    if (!authInitialized) return null

    // РµСЃР»Рё РЅРµ Р°РІС‚РѕСЂРёР·РѕРІР°РЅ
    if (!token) return <Navigate to="/login" replace />

    // РµСЃР»Рё СЂРѕР»СЊ РЅРµ СЃРѕРІРїР°РґР°РµС‚ СЃ СЂР°Р·СЂРµС€С‘РЅРЅС‹РјРё
    if (!role || !roles.includes(role)) return <Navigate to="/forbidden" replace />

    // РґРѕСЃС‚СѓРї СЂР°Р·СЂРµС€С‘РЅ
    return <Outlet />
}

