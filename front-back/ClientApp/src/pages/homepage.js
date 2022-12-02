import React, { useEffect, useState } from "react";
import styled from "styled-components";
import axios from 'axios';
import './StyleSheet.css'

import { GetAllPost } from "../components/APIconection/Post";

function Homepage() {
    const [posts, setPosts] = useState([]);

    const fetchPosts = async () => {

        GetAllPost(setPosts);

        console.log(posts);
    };

    useEffect(() => {
        fetchPosts();
    }, []);


    
    return (
        <ul>
            <li>
                <button >
                    {posts.map((post, index) => (

                        <text>
                            {post.postName}
                        </text>
                    ))}
                    
                </button>
            </li>
        </ul>
    )
}

export default Homepage