﻿import { useState } from 'react'
import HomePage from './pages/homepage'
import './App.css';

function App() {
    const [count, setCount] = useState(0)

    return (
        <HomePage />
    )
}

export default App