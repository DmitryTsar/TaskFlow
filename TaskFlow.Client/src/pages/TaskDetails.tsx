import React, { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { useSelector } from 'react-redux';
import { RootState } from '../store';
import { tasksService, Task } from '../api/tasksService';
import TaskComments from './TaskComments';
import TaskAttachments from './TaskAttachments';
import TaskHistory from './TaskHistory';

export default function TaskDetails() {
    const { projectId, taskId } = useParams<{ projectId: string; taskId: string }>();
    const navigate = useNavigate();
    const { userId, role } = useSelector((s: RootState) => s.auth);

    const [task, setTask] = useState<Task | null>(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        if (!taskId) return;
        setLoading(true);
        tasksService.getById(taskId)
            .then(setTask)
            .finally(() => setLoading(false));
    }, [taskId]);

    const deleteTask = async () => {
        if (!window.confirm('Are you sure you want to delete this task?')) return;
        if (!taskId) return;
        await tasksService.remove(taskId);
        navigate('/tasks'); // После удаления на TasksList
    };

    const editTask = () => {
        if (!taskId || !projectId) return;
        navigate(`/projects/${projectId}/tasks/edit/${taskId}`);
    };

    const statusColors = ['bg-blue-100 text-blue-800', 'bg-yellow-100 text-yellow-800', 'bg-green-100 text-green-800', 'bg-gray-100 text-gray-800', 'bg-purple-100 text-purple-800'];
    const priorityColors = ['bg-green-100 text-green-800', 'bg-yellow-100 text-yellow-800', 'bg-red-100 text-red-800'];

    if (loading) return <div className="no-tasks">Loading...</div>;
    if (!task) return <div className="no-tasks">Task not found</div>;

    const canEdit = role === 'Admin' || task.assignedToId === userId;

    return (
        <div style={{ maxWidth: '700px', margin: '30px auto', padding: '32px', display: 'flex', flexDirection: 'column', gap: '20px' }}>
            {/* Детали задачи */}
            <div className="card">
                <div className="flex justify-between items-center mb-4">
                    <h2>{task.title}</h2>
                    <div className="flex gap-2">
                        {canEdit && (
                            <>
                                <button className="btn btn-primary" onClick={editTask}>Edit</button>
                                <button className="btn btn-danger" onClick={deleteTask}>Delete</button>
                            </>
                        )}
                    </div>
                </div>

                <div className="task-meta mb-4">
                    <span>Project: <b>{task.projectName || 'N/A'}</b></span>
                    <span> | Assigned to: <b>{task.assignedToName || 'N/A'}</b></span>
                </div>

                <div>
                    <p>{task.description || 'No description provided.'}</p>
                    <div className="flex gap-4 mt-2">
                        <span className={`task-status ${statusColors[task.status]}`}>
                            Status: {['New', 'In Progress', 'Completed', 'Blocked', 'Archived'][task.status]}
                        </span>
                        <span className={`task-priority ${priorityColors[task.priority]}`}>
                            Priority: {['Low', 'Medium', 'High'][task.priority]}
                        </span>
                    </div>
                </div>
            </div>

            {/* Комментарии */}
            <TaskComments taskId={task.id} taskTitle={task.title} />

            {/* Вложения */}
            <TaskAttachments taskId={task.id} taskTitle={task.title} />

            {/* История изменений */}
            <TaskHistory taskId={task.id} />
        </div>
    );
}
