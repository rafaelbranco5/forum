import React, { useEffect, useState } from "react";
import styled from "styled-components";
import axios from 'axios';
import './StyleSheet.css'



function Homepage() {
    const [posts, setPosts] = useState([]);

    const fetchPosts = async () => {
        const { data } = await axios.get("http://localhost:44460/api/post/All");
        const post = data;
        setPosts(post);
        console.log(post);
    };

    useEffect(() => {
        fetchPosts();
    }, []);


    
    return (
        <ul>
            <li>
                <button >
                    {posts.map((post, index) => ({post}))}
                    
                </button>
            </li>
        </ul>
    )
}

export default Homepage