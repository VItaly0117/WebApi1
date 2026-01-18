import { useEffect, useState } from 'react';
import axios from 'axios';
import { useAuth } from '../contexts/AuthContext';
import TodoItem from './TodoItem';

export default function TodoList({ list }) {
    const [items, setItems] = useState([]);
    const [title, setTitle] = useState('');
    const [search, setSearch] = useState('');
    const [statusFilter, setStatusFilter] = useState('');
    const { user } = useAuth();

    useEffect(() => {
        const fetchItems = async () => {
            const response = await axios.get(`/api/todoitem/list/${list.id}`, {
                headers: { Authorization: `Bearer ${user.token}` }
            });
            if (response.data.success) {
                setItems(response.data.data);
            }
        };
        fetchItems();
    }, [list.id, user.token]);

    const handleCreateItem = async (e) => {
        e.preventDefault();
        const response = await axios.post('/api/todoitem', { title, todoListId: list.id, priority: 0 }, {
            headers: { Authorization: `Bearer ${user.token}` }
        });
        if (response.data.success) {
            setItems([...items, response.data.data]);
            setTitle('');
        }
    };
    
    const handleSearch = async () => {
        const response = await axios.get(`/api/todoitem/search?title=${search}&isCompleted=${statusFilter}`, {
            headers: { Authorization: `Bearer ${user.token}` }
        });
        if (response.data.success) {
            setItems(response.data.data.filter(item => item.todoListId === list.id));
        }
    };

    return (
        <div>
            <h3>{list.title}</h3>
            <form onSubmit={handleCreateItem}>
                <input type="text" value={title} onChange={(e) => setTitle(e.target.value)} placeholder="New Todo Item" required />
                <button type="submit">Add</button>
            </form>
            <div>
                <input type="text" value={search} onChange={(e) => setSearch(e.target.value)} placeholder="Search..." />
                <select value={statusFilter} onChange={(e) => setStatusFilter(e.target.value)}>
                    <option value="">All</option>
                    <option value="true">Completed</option>
                    <option value="false">Incomplete</option>
                </select>
                <button onClick={handleSearch}>Search</button>
            </div>
            {items.map(item => (
                <TodoItem key={item.id} item={item} setItems={setItems} />
            ))}
        </div>
    );
}
