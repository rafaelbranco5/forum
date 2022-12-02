using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace front_back.Database
{
    public class DB_controller
    {
        private static DB_controller safe_instance = null;

       public  static DB_controller instance() { 
            
            if(safe_instance == null)
            {
                safe_instance = new DB_controller();
                safe_instance.DB_connect();
            }

            return safe_instance;
        }

        private DB_controller() { }

        private IMongoCollection<Post> _Post_DB = null;

        public bool DB_connect()
        {
            if(_Post_DB != null) return true;

            BankDatabaseSettings dabaseSettings = new BankDatabaseSettings();

            var mongoClient = new MongoClient(dabaseSettings.ConnectionString);
            if(mongoClient == null)
            {
                return false;
            }

            _Post_DB = mongoClient.GetDatabase(dabaseSettings.DatabaseName).GetCollection<Post>(dabaseSettings.PostCollectionName);
            if (_Post_DB == null)
            {
                return false;
            }

            return true;
        }


        //=====================================
        //Find methods    
        public async Task<List<Post>> FindAllPost()
        {
            List<Post> PostList = await _Post_DB.Find(_ => true).ToListAsync();

            return PostList;
        }



        public async Task<Post> FindById(int id)
        {
            List<Post> TeamsList = await _Post_DB.Find(_ => true).ToListAsync();

            Post post = TeamsList.Find((a) => a.Id.Equals(id));

            return post;
        }

        
        public async Task<Post> FindPostByName(string postName)
        {
            List<Post> PostList = await _Post_DB.Find(_ => true).ToListAsync();

            Post post = PostList.Find((t) => t.PostName == postName);

            return post;
        }
        

        //=================================================
        //Insert methods
        public async Task<int> AddPost(Post newPost)
        {
            List<Post> PostList = await _Post_DB.Find(_ => true).ToListAsync();

            Post checkPostName = PostList.Find((a) => a.PostName.Equals(newPost.PostName));
            if(checkPostName is Post)
            {
                return -1;
            }


            int newPostId = 0;
            if (PostList.Count > 0)
            {
                newPostId = PostList.Last().Id + 1;
            }
            else
            {
                newPostId = 1;
            }



            newPost.Id = newPostId;

            await _Post_DB.InsertOneAsync(newPost);

            return newPostId;
        }


        public async Task<int> AddPostComment(int PostId, MainThread comment)
        {
            Post post = await FindById(PostId);
            if (!(post is Post)) { return 0; }

            int newCommentId;
            if (post.Comments.Count > 0)
            {
                newCommentId = post.Comments.Last().Id + 1;
            }
            else { newCommentId = 1; }

            comment.Id = newCommentId;

            post.Comments.Add(comment);

            await _Post_DB.ReplaceOneAsync(t => t.Id == PostId, post);

            return newCommentId;
        }


        public async Task<int> AddReply(int PostId, int CommentId, SecondThread reply)
        {
            Post post = await FindById(PostId);
            if (!(post is Post)) { return 0; }
            MainThread comment = post.Comments.Find(t => t.Id == CommentId);
            if (!(comment is MainThread)) { return 0; }
                        
            int newReplyId;
            if (comment.Replies.Count > 0)
            {
                newReplyId = comment.Replies.Last().Id + 1;
            }
            else { newReplyId = 1; }

            reply.Id = newReplyId;

            comment.Replies.Add(reply);
            
            await _Post_DB.ReplaceOneAsync(t => t.Id == PostId, post);

            return newReplyId;
        }


        //=============================================
        //Change Methods

        public async Task<bool> ChangePost(int PostId, Post newPost)
        {
            Post post = await FindById(PostId);
            if (!(post is Post)) { return false; }
            newPost.Id = PostId;
            await _Post_DB.ReplaceOneAsync(t => t.Id == PostId, newPost);
            return true;
        }

        public async Task<bool> ChangeComment(int PostId, int CommentId, MainThread newComment)
        {
            Post post = await FindById(PostId);
            if (!(post is Post)) { return false; }

            int commentIndex = post.Comments.FindIndex(t => t.Id == CommentId);
            if(commentIndex == -1) { return false; }

            newComment.Id = CommentId;
            post.Comments[commentIndex] = newComment;

            return await ChangePost(PostId, post);

        }


        public async Task<bool> ChangeReply(int PostId, int CommentId, int ReplyId, SecondThread newReply)
        {
            Post post = await FindById(PostId);
            if (!(post is Post)) { return false; }

            MainThread comment = post.Comments.Find(t => t.Id == CommentId);
            if (!(comment is MainThread)) { return false; }


            int ReplyIndex = comment.Replies.FindIndex(t => t.Id == ReplyId);
            if(ReplyIndex == -1) { return false; }

            newReply.Id = ReplyId;
            comment.Replies[ReplyIndex] = newReply;

            return await ChangePost(PostId, post);
        }


        //==============================================
        //Delete methods
        public async Task<bool> RemovePost(int PostId)
        {
            Post post = await FindById(PostId);
            if (!(post is Post)) { return false; }

            await _Post_DB.DeleteOneAsync(t => t.Id == PostId);
            return true;
        }


        public async Task<bool> RemoveComment(int PostId, int CommentId)
        {
            Post post = await FindById(PostId);
            if (!(post is Post)) { return false; }

            MainThread comment = post.Comments.Find(t => t.Id == CommentId);
            if (!(comment is MainThread)) { return false; }

            post.Comments.Remove(comment);

            await _Post_DB.ReplaceOneAsync(t => t.Id == PostId, post);

            return true;
        }

        public async Task<bool> RemoveReply(int PostId, int CommentId, int ReplyId)
        {
            Post post = await FindById(PostId);
            if (!(post is Post)) { return false; }

            MainThread comment = post.Comments.Find(t => t.Id == CommentId);
            if (!(comment is MainThread)) { return false; }

            SecondThread reply = comment.Replies.Find(t => t.Id == ReplyId);
            if (!(reply is SecondThread)) { return false; }

            comment.Replies.Remove(reply);

            await _Post_DB.ReplaceOneAsync(t => t.Id == PostId, post);

            return true;
        }
    }
}
