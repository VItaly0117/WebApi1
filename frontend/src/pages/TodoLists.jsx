import { useEffect, useState } from 'react';
import axios from 'axios';
import { useAuth } from '../contexts/AuthContext';
import TodoList from '../components/TodoList';

export default function TodoLists() {
    const [todoLists, setTodoLists] = useState([]);
    const [title, setTitle] = useState('');
    const { user } = useAuth();

    useEffect(() => {
        const fetchTodoLists = async () => {
            const response = await axios.get('/api/todolist', {
                headers: { Authorization: `Bearer ${user.token}` }
            });
            if (response.data.success) {
                setTodoLists(response.data.data);
            }
        };
        fetchTodoLists();
    }, [user.token]);

    const handleCreate = async (e) => {
        e.preventDefault();
        const response = await axios.post('/api/todolist', { title }, {
            headers: { Authorization: `Bearer ${user.token}` }
        });
        if (response.data.success) {
            setTodoLists([...todoLists, response.data.data]);
            setTitle('');
        }
    };

    return (
        <div>
            <h2>Todo Lists</h2>
            <form onSubmit={handleCreate}>
                <input type="text" value={title} onChange={(e) => setTitle(e.target.value)} placeholder="New Todo List" required />
                <button type="submit">Create</button>
            </form>
            {todoLists.map(list => (
                <TodoList key={list.id} list={list} />
            ))}
        </div>
    );
}
