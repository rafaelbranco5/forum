
import { useState } from "react"

const PostBlueprint = {
    id: 0,    
    userName: "",
    postName: "",
    category: "",
    body: "",
    image: "",
    public: "",
    comments: []
}

const ValidatepostBluePrint = {
    postName: ""   
}


//=====================================================================
//Get Methods
const GetAllPost = async (setPosts) => {

    const response = await fetch("api/post/All", { method: 'GET', headers: { 'Content-Type': 'application/json;charset=utf-8' } });

    if (response.ok) {
        const data = await response.json();
        console.log(response);
        console.log(data);
        

        setPosts(data);
    } else {
        console.log("Error: Cant connect with the API");
    }

}


const GetPostById = async (PostID, setPost) => {

    const response = await fetch("api/post/" + PostID);

    if (response.ok) {
        const data = await response.json();
        setPost(data);
    } else {
        console.log("Error: Cant find the given Post");
    }

}


const ValidatePost = async (postName, SetBoolResult) => {

    const [post, setPost] = useState(ValidatepostBluePrint)

    setPost.postName = postName;
    

    const response = await fetch("api/post/ValidatePost", { method: 'GET', headers: { 'Content-Type': 'application/json;charset=utf-8' }, body: JSON.stringify(post) });

    const data = await response.json();
    SetBoolResult(data);   

}


//====================================================================
//Post method


const CreatePost = async (userName, postName, category, body, image, publicState, setResultPost) => {

    const [newPost, setNewPost] = useState(PostBlueprint)

    setNewPost.userName = userName;
    setNewPost.postName = postName;
    setNewPost.category = category;
    setNewPost.body = body;
    setNewPost.image = image;
    setNewPost.public = publicState;


    const response = await fetch("api/post/AddPost", { method: 'POST', headers: { 'Content-Type': 'application/json;charset=utf-8' }, body: JSON.stringify(newPost) });

    if (response.ok) {
        const data = await response.json();
        setNewPost.id = data;
        setResultPost(newPost);
    } else {
        console.log("Error: Cant add a Post");
    }

}

//===================================================================
//Put methods

const EditPost = async (PostID, NewPost, setResultPost ) => {

    const [newPost, setNewPost] = useState(PostBlueprint)

    setNewPost(NewPost);
    setNewPost.id = PostID;

    const response = await fetch("api/post/EditPost/" + PostID, { method: 'PUT', headers: { 'Content-Type': 'application/json;charset=utf-8' }, body: JSON.stringify(newPost) });

    if (response.ok) {
        setResultPost(newPost);
    } else {
        const data = await response.json();
        console.log("Error: " + data);
    }

}

//=================================================================
//DELETE

const RemovePost = async (PostID ) => {

    const response = await fetch("api/post/DeletePost/" + PostID, { method: 'DELETE' });

    if (response.ok) {
       
    } else {
        const data = await response.json();
        console.log("Error: " + data);
    }

}

export { GetAllPost, GetPostById, ValidatePost, CreatePost, EditPost, RemovePost, PostBlueprint, ValidatepostBluePrint };