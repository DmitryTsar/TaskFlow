import React, { Suspense, lazy, useEffect } from 'react'
import { Routes, Route, Navigate } from 'react-router-dom'
import { useDispatch } from 'react-redux'
import { AppDispatch } from './store'
import { initializeAuth } from './store/slices/authSlice'
import ProtectedRoute from './components/ProtectedRoute'
import RoleProtectedRoute from './components/RoleProtectedRoute'
import PublicRoute from './components/PublicRoute'
import Layout from './components/Layout'
import './styles.css'

// Pages
const Login = lazy(() => import('./pages/Login'))
const Register = lazy(() => import('./pages/Register'))
const ProjectsList = lazy(() => import('./pages/ProjectsList'))
const ProjectDetails = lazy(() => import('./pages/ProjectDetails'))
const ProjectForm = lazy(() => import('./pages/ProjectForm'))
const TasksList = lazy(() => import('./pages/TasksList'))
const TaskForm = lazy(() => import('./pages/TaskForm'))
const TaskDetails = lazy(() => import('./pages/TaskDetails'))
const UsersAdminPanel = lazy(() => import('./pages/UsersAdminPanel'))
const ProfilePanel = lazy(() => import('./pages/ProfilePanel'))
const Forbidden = lazy(() => import('./pages/Forbidden'))

const NotFound = () => <div className="text-center mt-12">Page not found</div>
const LoadingFallback = () => (
    <div className="flex justify-center items-center h-screen text-lg text-gray-600">
        Loading...
    </div>
)

export default function App() {
    const dispatch = useDispatch<AppDispatch>()

    useEffect(() => {
        dispatch(initializeAuth())
    }, [dispatch])

    return (
        <Suspense fallback={<LoadingFallback />}>
            <Routes>
                {/* Public */}
                <Route element={<PublicRoute />}>
                    <Route path="/login" element={<Login />} />
                    <Route path="/register" element={<Register />} />
                </Route>

                {/* Protected */}
                <Route element={<ProtectedRoute />}>
                    <Route element={<Layout />}>
                        {/* Projects - Admin & User */}
                        <Route element={<RoleProtectedRoute roles={['Admin', 'User']} />}>
                            <Route path="/projects" element={<ProjectsList />} />
                            <Route path="/projects/new" element={<ProjectForm />} />
                            <Route path="/projects/edit/:id" element={<ProjectForm />} />
                            <Route path="/projects/:projectId" element={<ProjectDetails />} />
                            <Route path="/projects/:projectId/tasks/new" element={<TaskForm />} />
                            <Route path="/projects/:projectId/tasks/edit/:taskId" element={<TaskForm />} />
                            <Route path="/projects/:projectId/tasks/:taskId" element={<TaskDetails />} />
                        </Route>

                        {/* My Tasks - Admin & User */}
                        <Route element={<RoleProtectedRoute roles={['Admin', 'User']} />}>
                            <Route path="/tasks" element={<TasksList />} />
                        </Route>

                        {/* Users Admin Panel - Admin only */}
                        <Route element={<RoleProtectedRoute roles={['Admin']} />}>
                            <Route path="/users" element={<UsersAdminPanel />} />
                        </Route>

                        {/* Profile - Admin & User */}
                        <Route element={<RoleProtectedRoute roles={['Admin', 'User']} />}>
                            <Route path="/profile" element={<ProfilePanel />} />
                        </Route>
                    </Route>
                </Route>

                {/* Utility */}
                <Route path="/forbidden" element={<Forbidden />} />
                <Route path="/" element={<Navigate to="/projects" replace />} />
                <Route path="*" element={<NotFound />} />
            </Routes>
        </Suspense>
    )
}
