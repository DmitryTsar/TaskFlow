import React, { useEffect, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useSelector } from 'react-redux';
import { RootState } from '../store';
import { tasksService, Task } from '../api/tasksService';
import { projectsService, Project } from '../api/projectsService';

export default function TasksList() {
    const { userId, role } = useSelector((s: RootState) => s.auth);
    const navigate = useNavigate();

    const [tasks, setTasks] = useState<Task[]>([]);
    const [projects, setProjects] = useState<Project[]>([]);
    const [filterProject, setFilterProject] = useState('');
    const [filterStatus, setFilterStatus] = useState('');
    const [filterPriority, setFilterPriority] = useState('');

    const loadTasks = async () => {
        const [tasksData, projectsData] = await Promise.all([
            tasksService.getAll(),
            projectsService.getAll()
        ]);
        setTasks(role === 'Admin' ? tasksData : tasksData.filter(t => t.assignedToId === userId));
        setProjects(projectsData);
    };

    useEffect(() => {
        loadTasks();
    }, [userId, role]);

    const filtered = tasks.filter(t =>
        (!filterProject || t.projectId === filterProject) &&
        (!filterStatus || t.status === Number(filterStatus)) &&
        (!filterPriority || t.priority === Number(filterPriority))
    );

    const statusColors = ['bg-blue-100 text-blue-800', 'bg-yellow-100 text-yellow-800', 'bg-green-100 text-green-800', 'bg-gray-100 text-gray-800', 'bg-purple-100 text-purple-800'];
    const priorityColors = ['bg-green-100 text-green-800', 'bg-yellow-100 text-yellow-800', 'bg-red-100 text-red-800'];

    const addTask = () => {
        if (!filterProject) {
            alert('Please select a project first');
            return;
        }
        navigate(`/projects/${filterProject}/tasks/new`);
    };

    return (
        <div className="tasks-container">
            <div className="tasks-header mb-4">
                <h2>{role === 'Admin' ? 'All Tasks' : 'My Tasks'}</h2>
            </div>

            {/* Filters + Add Task */}
            <div className="filters flex gap-2 mb-4 items-center">
                <select value={filterProject} onChange={e => setFilterProject(e.target.value)}>
                    <option value="">All Projects</option>
                    {projects.map(p => <option key={p.id} value={p.id}>{p.name}</option>)}
                </select>
                <select value={filterStatus} onChange={e => setFilterStatus(e.target.value)}>
                    <option value="">All Statuses</option>
                    <option value="0">New</option>
                    <option value="1">In Progress</option>
                    <option value="2">Completed</option>
                    <option value="3">Blocked</option>
                    <option value="4">Archived</option>
                </select>
                <select value={filterPriority} onChange={e => setFilterPriority(e.target.value)}>
                    <option value="">All Priorities</option>
                    <option value="0">Low</option>
                    <option value="1">Medium</option>
                    <option value="2">High</option>
                </select>

                <button className="btn btn-secondary ml-auto" onClick={addTask}>➕ Add Task</button>
            </div>

            <div className="tasks-grid">
                {filtered.length === 0 && <div className="no-tasks">No tasks found</div>}
                {filtered.map(t => (
                    <div key={t.id} className="task-card">
                        <h3 className="task-title">
                            <Link to={`/projects/${t.projectId}/tasks/${t.id}`}>{t.title}</Link>
                        </h3>
                        <p className="task-project">{projects.find(p => p.id === t.projectId)?.name || '-'}</p>
                        <div className="task-meta flex gap-2">
                            <span className={`task-priority ${priorityColors[t.priority]}`}>
                                {['Low', 'Medium', 'High'][t.priority]}
                            </span>
                            <span className={`task-status ${statusColors[t.status]}`}>
                                {['New', 'In Progress', 'Completed', 'Blocked', 'Archived'][t.status]}
                            </span>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
}
