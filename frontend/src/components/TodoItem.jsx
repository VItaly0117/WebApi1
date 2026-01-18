import axios from 'axios';
import { useAuth } from '../contexts/AuthContext';

export default function TodoItem({ item, setItems }) {
    const { user } = useAuth();

    const handleToggleComplete = async () => {
        const updatedItem = { ...item, isCompleted: !item.isCompleted };
        
        // Optimistic UI
        setItems(prevItems => prevItems.map(i => i.id === item.id ? updatedItem : i));

        try {
            await axios.put(`/api/todoitem/${item.id}`, updatedItem, {
                headers: { Authorization: `Bearer ${user.token}` }
            });
        } catch (error) {
            // Revert if error
            setItems(prevItems => prevItems.map(i => i.id === item.id ? item : i));
        }
    };

    return (
        <div style={{ textDecoration: item.isCompleted ? 'line-through' : 'none' }}>
            <input type="checkbox" checked={item.isCompleted} onChange={handleToggleComplete} />
            {item.title}
        </div>
    );
}
