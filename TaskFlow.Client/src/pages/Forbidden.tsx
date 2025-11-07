import React from 'react'
import { Link } from 'react-router-dom'

export default function Forbidden() {
    return (
        <div style={{ padding: 40, textAlign: 'center' }}>
            <h1>403 — Доступ запрещён</h1>
            <p>У вас нет прав для просмотра этой страницы.</p>
            <Link to="/">Вернуться на главную</Link>
        </div>
    )
}

