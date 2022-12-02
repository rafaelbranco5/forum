using front_back.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Client;
using MongoDB.Bson.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace test_backend.Database
{
    [TestClass]
    public class DatabaseUnitTest
    {

        [TestMethod]
        public void ValidateSettingsCreation()
        {

            BankDatabaseSettings db_test = new BankDatabaseSettings();

            Assert.IsNotNull(db_test, "Cant create Settings");

            Assert.IsTrue(db_test.ConnectionString.Equals("mongodb+srv://admin:admin@cluster0.chqxutx.mongodb.net/?retryWrites=true&w=majority"), "ConnectionString changed \n expected: mongodb+srv://admin:admin@cluster0.chqxutx.mongodb.net/?retryWrites=true&w=majority \n obtained: " + db_test.ConnectionString);
            Assert.IsTrue(db_test.DatabaseName.Equals("Pair-Forum-DB"), "DatabaseName changed \n expected: Pair-Forum-DB \n obtained: " + db_test.DatabaseName);
            Assert.IsTrue(db_test.PostCollectionName.Equals("Forum"), "BankCollectionName changed \n expected: Forum \n obtained: " + db_test.PostCollectionName);

        }

        [TestMethod]
        public void ValidateConnection()
        {

            DB_controller db_test = DB_controller.instance();
            bool result = db_test.DB_connect();

            Assert.IsTrue(result, "Fail to connect MongoDB");

        }




        [TestMethod]
        public async Task ValidateCreatePostAndRemoveThem()
        {

            DB_controller db_test = DB_controller.instance();
            db_test.DB_connect();

            //Create post
            Post post = new Post();
            post.UserName = "Luciano";
            post.PostName = "Solera proyect";
            post.Category = "question";
            post.Body = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.";
            post.Image = "question mark";
            post.Public = "True";


            //Check post initial state
            Assert.IsTrue(post.UserName.Equals("Luciano"), "Post FirstName expected: Luciano -- obtained: " + post.UserName);
            Assert.IsTrue(post.PostName.Equals("Solera proyect"), "Post PostName expected: Solera proyect -- obtained: " + post.PostName);
            Assert.IsTrue(post.Category.Equals("question"), "Post Category expected: question -- obtained: " + post.Category);
            Assert.IsTrue(post.Body.Equals("Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum."), "Post Body expected: Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum. \n obtained: " + post.Body);
            Assert.IsTrue(post.Image.Equals("question mark"), "Post Image expected: question mark -- obtained: " + post.Image);
            Assert.IsTrue(post.Public.Equals("True"), "Post Public expected: True -- obtained: " + post.Public);


            Assert.IsTrue(post.Comments.Count.Equals(0), "Client Accounts count expected: 0 -- obtained: " + post.Comments.Count.ToString());


            //Add post and recover it
            int PostId = await db_test.AddPost(post);
            Assert.IsTrue((PostId > 0), "Error creating Post");

            Post checkPost = await db_test.FindById(PostId);

            //Check post recover state
            Assert.IsTrue(checkPost.UserName.Equals("Luciano"), "Post FirstName expected: Luciano -- obtained: " + checkPost.UserName);
            Assert.IsTrue(checkPost.PostName.Equals("Solera proyect"), "Post PostName expected: Solera proyect -- obtained: " + checkPost.PostName);
            Assert.IsTrue(checkPost.Category.Equals("question"), "Post Category expected: question -- obtained: " + checkPost.Category);
            Assert.IsTrue(checkPost.Body.Equals("Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum."), "Post Body expected: Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum. \n obtained: " + checkPost.Body);
            Assert.IsTrue(checkPost.Image.Equals("question mark"), "Post Image expected: question mark -- obtained: " + checkPost.Image);
            Assert.IsTrue(checkPost.Public.Equals("True"), "Post Public expected: True -- obtained: " + checkPost.Public);


            //Remove post
            await db_test.RemovePost(PostId);
            //Check if the post is removed
            checkPost = await db_test.FindById(PostId);
            Assert.IsNull(checkPost);


        }


        [TestMethod]
        public async Task ValidateCreateCommentAndRemoveThem()
        {

            DB_controller db_test = DB_controller.instance();
            db_test.DB_connect();

            //Create post
            Post post = new Post();
            post.UserName = "Luciano";
            post.PostName = "Solera proyect";
            post.Category = "question";
            post.Body = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.";
            post.Image = "question mark";
            post.Public = "True";

            //Add post and recover it
            int PostId = await db_test.AddPost(post);

            //Create a comment
            MainThread comment = new MainThread();
            comment.UserName = "Rafael";
            comment.Comment = "Good Post";

            //Put commet
            int commentId = await db_test.AddPostComment(PostId, comment);

            //Recover Post with new comment
            Post checkPost = await db_test.FindById(PostId);

            //Check account state
            Assert.IsTrue(checkPost.Comments.Count.Equals(1), "Post Comments count expected: 1 -- obtained: " + checkPost.Comments.Count.ToString());
            Assert.IsTrue(checkPost.Comments[0].UserName.Equals("Rafael"), "Post Comments UserName expected: Rafael -- obtained: " + checkPost.Comments[0].UserName);
            Assert.IsTrue(checkPost.Comments[0].Comment.Equals("Good Post"), "Post Comments comment expected: Good Post -- obtained: " + checkPost.Comments[0].Comment);
            Assert.IsTrue(checkPost.Comments[0].Replies.Count.Equals(0), "Post Comments reply count expected: 0 -- obtained: " + checkPost.Comments[0].Replies.Count.ToString());

            //Remove comment
            await db_test.RemoveComment(PostId, commentId);
            //Recover post without comment
            checkPost = await db_test.FindById(PostId);
            Assert.IsTrue(checkPost.Comments.Count.Equals(0), "Post Comments count expected: 0 -- obtained: " + checkPost.Comments.Count.ToString());


            //Remove post
            await db_test.RemovePost(PostId);
            //Check the post is removed
            checkPost = await db_test.FindById(PostId);
            Assert.IsNull(checkPost);


        }

        [TestMethod]
        public async Task ValidateCreateReplyAndRemoveThem()
        {

            DB_controller db_test = DB_controller.instance();


            //Create post
            Post post = new Post();
            post.UserName = "Luciano";
            post.PostName = "Solera proyect 2";
            post.Category = "question";
            post.Body = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.";
            post.Image = "question mark";
            post.Public = "True";

            //Add post and recover it
            int PostId = await db_test.AddPost(post);

            //Create a comment
            MainThread comment = new MainThread();
            comment.UserName = "Rafael";
            comment.Comment = "Good Post";

            //Put commet
            int commentId = await db_test.AddPostComment(PostId, comment);

            SecondThread reply = new SecondThread();
            reply.UserName = "Solera";
            reply.comment = "Start working";

            int replyID = await db_test.AddReply(PostId, commentId, reply);

            //Recover Post with new comment and reply
            Post checkPost = await db_test.FindById(PostId);

            Assert.IsTrue(checkPost.Comments.Count.Equals(1), "Post Comments count expected: 1 -- obtained: " + checkPost.Comments.Count.ToString());
            Assert.IsTrue(checkPost.Comments[0].Replies.Count.Equals(1), "Post Comments replies count expected: 1 -- obtained: " + checkPost.Comments[0].Replies.Count.ToString());

            Assert.IsTrue(checkPost.Comments[0].Replies[0].UserName.Equals("Solera"), "Post Comments reply UserName expected: Rafael -- obtained: " + checkPost.Comments[0].Replies[0].UserName);
            Assert.IsTrue(checkPost.Comments[0].Replies[0].comment.Equals("Start working"), "Post Comments reply comment expected: Good Post -- obtained: " + checkPost.Comments[0].Replies[0].comment);

            //Remove reply
            await db_test.RemoveReply(PostId, commentId, replyID);
            //Recover post without reply
            checkPost = await db_test.FindById(PostId);
            Assert.IsTrue(checkPost.Comments[0].Replies.Count.Equals(0), "Post Comments reply count expected: 0 -- obtained: " + checkPost.Comments[0].Replies.Count.ToString());


            //Remove comment
            await db_test.RemoveComment(PostId, commentId);
            //Recover post without comment
            checkPost = await db_test.FindById(PostId);
            Assert.IsTrue(checkPost.Comments.Count.Equals(0), "Post Comments count expected: 0 -- obtained: " + checkPost.Comments.Count.ToString());


            //Remove post
            await db_test.RemovePost(PostId);
            //Check the post is removed
            checkPost = await db_test.FindById(PostId);
            Assert.IsNull(checkPost);


        }


        [TestMethod]
        public async Task ValidateFindPost()
        {

            DB_controller db_test = DB_controller.instance();


            //Create post
            Post post = new Post();
            post.UserName = "Luciano";
            post.PostName = "Solera proyect";
            post.Category = "question";
            post.Body = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.";
            post.Image = "question mark";
            post.Public = "True";

            //Add client and recover it
            int PostId1 = await db_test.AddPost(post);

            post = new Post();
            post.UserName = "Lusvarghi";
            post.PostName = "Solera Bank proyect";
            post.Category = "suggestion";
            post.Body = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.";
            post.Image = "pencil";
            post.Public = "False";

            //Add client and recover it
            int PostId2 = await db_test.AddPost(post);

            //Recover Post with new comment and reply
            Post checkPost = await db_test.FindById(PostId1);

            //Check post recover state
            Assert.IsTrue(checkPost.UserName.Equals("Luciano"), "Post FirstName expected: Luciano -- obtained: " + checkPost.UserName);
            Assert.IsTrue(checkPost.PostName.Equals("Solera proyect"), "Post PostName expected: Solera proyect -- obtained: " + checkPost.PostName);
            Assert.IsTrue(checkPost.Category.Equals("question"), "Post Category expected: question -- obtained: " + checkPost.Category);
            Assert.IsTrue(checkPost.Body.Equals("Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum."), "Post Body expected: Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum. \n obtained: " + checkPost.Body);
            Assert.IsTrue(checkPost.Image.Equals("question mark"), "Post Image expected: question mark -- obtained: " + checkPost.Image);
            Assert.IsTrue(checkPost.Public.Equals("True"), "Post Public expected: True -- obtained: " + checkPost.Public);

            checkPost = await db_test.FindById(PostId2);

            //Check post recover state
            Assert.IsTrue(checkPost.UserName.Equals("Lusvarghi"), "Post FirstName expected: Luciano -- obtained: " + checkPost.UserName);
            Assert.IsTrue(checkPost.PostName.Equals("Solera Bank proyect"), "Post PostName expected: Solera proyect -- obtained: " + checkPost.PostName);
            Assert.IsTrue(checkPost.Category.Equals("suggestion"), "Post Category expected: question -- obtained: " + checkPost.Category);
            Assert.IsTrue(checkPost.Body.Equals("Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum."), "Post Body expected: Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum. \n obtained: " + checkPost.Body);
            Assert.IsTrue(checkPost.Image.Equals("pencil"), "Post Image expected: question mark -- obtained: " + checkPost.Image);
            Assert.IsTrue(checkPost.Public.Equals("False"), "Post Public expected: True -- obtained: " + checkPost.Public);


            //Remove post
            await db_test.RemovePost(PostId1);
            //Check the post is removed
            checkPost = await db_test.FindById(PostId1);
            Assert.IsNull(checkPost);

            //Remove post
            await db_test.RemovePost(PostId2);
            //Check the post is removed
            checkPost = await db_test.FindById(PostId2);
            Assert.IsNull(checkPost);


        }









    }
}
