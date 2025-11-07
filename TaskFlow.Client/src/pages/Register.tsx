import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { authService } from '../api/authService.js';
import axios from 'axios';

export default function Register() {
    const [userName, setUserName] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [confirmPassword, setConfirmPassword] = useState('');
    const [error, setError] = useState<string | null>(null);
    const [success, setSuccess] = useState<string | null>(null);
    const [loading, setLoading] = useState(false);

    const navigate = useNavigate();

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setError(null);
        setSuccess(null);

        if (password !== confirmPassword) {
            setError('Passwords do not match');
            return;
        }

        setLoading(true);
        try {
            await authService.register({ userName, email, password });
            setSuccess('Registration successful! Redirecting to login...');
            setUserName('');
            setEmail('');
            setPassword('');
            setConfirmPassword('');
            setTimeout(() => navigate('/login'), 1500);
        } catch (err: unknown) {
            if (axios.isAxiosError(err)) {
                setError(err.response?.data?.message ?? err.message ?? 'Registration failed');
            } else if (err instanceof Error) {
                setError(err.message);
            } else {
                setError('Registration failed');
            }
        } finally {
            setLoading(false);
        }
    };

    return (
        <div style={containerStyle}>
            <form onSubmit={handleSubmit} style={formStyle}>
                <h2 style={{ textAlign: 'center', marginBottom: 16 }}>Register</h2>

                <div style={fieldStyle}>
                    <label>Username</label>
                    <input
                        type="text"
                        value={userName}
                        onChange={e => setUserName(e.target.value)}
                        required
                        style={inputStyle}
                        placeholder="Enter your username"
                        disabled={loading}
                    />
                </div>

                <div style={fieldStyle}>
                    <label>Email</label>
                    <input
                        type="email"
                        value={email}
                        onChange={e => setEmail(e.target.value)}
                        required
                        style={inputStyle}
                        placeholder="Enter your email"
                        disabled={loading}
                    />
                </div>

                <div style={fieldStyle}>
                    <label>Password</label>
                    <input
                        type="password"
                        value={password}
                        onChange={e => setPassword(e.target.value)}
                        required
                        style={inputStyle}
                        placeholder="Enter your password"
                        disabled={loading}
                    />
                </div>

                <div style={fieldStyle}>
                    <label>Confirm Password</label>
                    <input
                        type="password"
                        value={confirmPassword}
                        onChange={e => setConfirmPassword(e.target.value)}
                        required
                        style={inputStyle}
                        placeholder="Confirm your password"
                        disabled={loading}
                    />
                </div>

                {error && <div style={errorStyle}>{error}</div>}
                {success && <div style={successStyle}>{success}</div>}

                <button type="submit" style={buttonStyle} disabled={loading}>
                    {loading ? 'Registering...' : 'Register'}
                </button>
            </form>
        </div>
    );
}

// Стили
const containerStyle: React.CSSProperties = {
    display: 'flex',
    justifyContent: 'center',
    alignItems: 'center',
    height: '80vh',
};

const formStyle: React.CSSProperties = {
    width: 360,
    padding: 24,
    border: '1px solid #ccc',
    borderRadius: 8,
    boxShadow: '0 4px 12px rgba(0,0,0,0.1)',
    backgroundColor: '#fff',
};

const fieldStyle: React.CSSProperties = {
    display: 'flex',
    flexDirection: 'column',
    marginBottom: 16,
};

const inputStyle: React.CSSProperties = {
    padding: 8,
    borderRadius: 4,
    border: '1px solid #ccc',
    fontSize: 14,
};

const buttonStyle: React.CSSProperties = {
    width: '100%',
    padding: 10,
    borderRadius: 4,
    border: 'none',
    backgroundColor: '#28a745',
    color: '#fff',
    fontWeight: 'bold',
    cursor: 'pointer',
};

const errorStyle: React.CSSProperties = {
    color: 'red',
    marginBottom: 12,
    textAlign: 'center',
};

const successStyle: React.CSSProperties = {
    color: 'green',
    marginBottom: 12,
    textAlign: 'center',
};
 
