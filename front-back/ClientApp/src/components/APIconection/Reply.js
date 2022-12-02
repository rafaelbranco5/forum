
import { useState } from "react"

import { GetPostById } from "./Post"

const ReplyBlueprint = {
    id: 0,
    userName: "",
    comment: ""
}


//====================================================================
//Post method

const CreateReply = async (PostID, CommentID, userName, comment, setResultPost) => {

    const [reply, setReply] = useState(ReplyBlueprint)

    setReply.userName = userName;
    setReply.comment = comment;
    

    const response = await fetch("api/post/" + PostID + "/Comment/" + CommentID + "/AddReply", { method: 'POST', headers: { 'Content-Type': 'application/json;charset=utf-8' }, body: JSON.stringify(reply) });

    const data = await response.json();

    if (response.ok) {
        setReply.id = data;
        GetPostById(PostID, setResultPost);
    } else {
        console.log("Error: " + data);
    }

}



//===================================================================
//Put methods

const EditReply = async (PostID, CommentID, ReplyID, newReply, setResultPost) => {

    const [reply, setReply] = useState(ReplyBlueprint)

    setReply(newReply);
    setReply.id = ReplyID;

    const response = await fetch("api/post/" + PostID + "/Comment/" + CommentID + "/EditReply/" + ReplyID, { method: 'PUT', headers: { 'Content-Type': 'application/json;charset=utf-8' }, body: JSON.stringify(reply) });

    if (response.ok) {
        GetPostById(PostID, setResultPost);
    } else {
        const data = await response.json();
        console.log("Error: " + data);
    }

}


//=================================================================
//DELETE

const RemoveReply = async (PostID, CommentID, ReplyID, setResultPost) => {

    const response = await fetch("api/post/" + PostID + "/Comment/" + CommentID + "/DeleteReply/" + ReplyID, { method: 'DELETE' });

    if (response.ok) {
        GetPostById(PostID, setResultPost);
    } else {
        const data = await response.json();
        console.log("Error: " + data);
    }

}


export { ReplyBlueprint, CreateReply, EditReply, RemoveReply };