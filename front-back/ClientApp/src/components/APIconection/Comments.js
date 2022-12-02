
import { useState } from "react"

import { GetPostById } from "./Post"

const CommentBlueprint = {
    id: 0,
    userName: "",
    comment: "",    
    replies: []
}


//====================================================================
//Post method

const CreateComment = async (PostID, userName, comment, setResultPost ) => {

    const [Comment, setComment] = useState(CommentBlueprint)

    setComment.userName = userName;
    setComment.comment = comment;
    
    const response = await fetch("api/post/" + PostID + "/AddComment", { method: 'POST', headers: { 'Content-Type': 'application/json;charset=utf-8' }, body: JSON.stringify(Comment) });

    const data = await response.json();

    if (response.ok) {        
        setComment.id = data;
        GetPostById(PostID, setResultPost);
    } else {
        console.log("Error: " + data);
    }

}



//===================================================================
//Put methods

const EditComment = async (PostID, CommentID, newComment, setResultPost) => {

    const [Comment, setComment] = useState(CommentBlueprint)

    setComment(newComment);
    setComment.id = CommentID;

    const response = await fetch("api/post/" + PostID + "/EditComment/" + CommentID, { method: 'PUT', headers: { 'Content-Type': 'application/json;charset=utf-8' }, body: JSON.stringify(Comment) });

    if (response.ok) {
        GetPostById(PostID, setResultPost);
    } else {
        const data = await response.json();
        console.log("Error: " + data);
    }

}


//=================================================================
//DELETE

const RemoveComment = async (PostID, CommentID, setResultPost) => {

    const response = await fetch("api/post/" + PostID + "/DeleteComment/" + CommentID, { method: 'DELETE' });

    if (response.ok) {
        GetPostById(PostID, setResultPost);
    } else {
        const data = await response.json();
        console.log("Error: " + data);
    }

}

export { CommentBlueprint, CreateComment, EditComment, RemoveComment };