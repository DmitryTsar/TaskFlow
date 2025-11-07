import React, { useState } from 'react';
import { useDispatch } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { authService } from '../api/authService.js';
import { setCredentials } from '../store/slices/authSlice.js';
import type { AppDispatch } from '../store/index.js';
import type { AuthResponse } from '../api/authService.js';
import axios from 'axios';
import { parseJwt } from '../store/slices/authSlice';

export default function Login() {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState<string | null>(null);
    const [loading, setLoading] = useState(false);

    const dispatch = useDispatch<AppDispatch>();
    const navigate = useNavigate();

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setError(null);
        setLoading(true);

        try {
            const res: AuthResponse = await authService.login({ email, password });
            // --- ПРОВЕРКА РОЛИ ---
            console.log('JWT token:', res.token);
            const payload = parseJwt(res.token);

            console.log('Parsed JWT payload:', payload); 
            dispatch(setCredentials({ token: res.token }));
            setEmail('');
            setPassword('');
            navigate('/tasks');
        } catch (err: unknown) {
            if (axios.isAxiosError(err)) {
                setError(err.response?.data?.message ?? err.message ?? 'Login failed');
            } else if (err instanceof Error) {
                setError(err.message);
            } else {
                setError('Login failed');
            }
        } finally {
            setLoading(false);
        }
    };

    return (
        <div style={containerStyle}>
            <form onSubmit={handleSubmit} style={formStyle}>
                <h2 style={{ textAlign: 'center', marginBottom: 16 }}>Login</h2>

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

                {error && <div style={errorStyle}>{error}</div>}

                <button type="submit" style={buttonStyle} disabled={loading}>
                    {loading ? 'Logging in...' : 'Login'}
                </button>
                <div style={{ textAlign: 'center', marginTop: 12 }}>
                    <span>Don't have an account? </span>
                    <a href="/register" style={{ color: '#007bff', textDecoration: 'none' }}>
                        Register here
                    </a>
                </div>
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
    backgroundColor: '#007bff',
    color: '#fff',
    fontWeight: 'bold',
    cursor: 'pointer',
};

const errorStyle: React.CSSProperties = {
    color: 'red',
    marginBottom: 12,
    textAlign: 'center',
};

