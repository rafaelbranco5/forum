
using front_back.Database;

using Microsoft.AspNetCore.Mvc;

namespace front_back.Controllers
{

    public class ValidatePost
    {
        public string PostName { get; set; } = string.Empty;        
    }


    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {

        public PostController() { }

        //==========================================================
        //GetMethods

        // Returning a list of Post
        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> All()
        {            
            DB_controller dB_Controller = DB_controller.instance();
            
            List<Post> PostList = await dB_Controller.FindAllPost();

            return StatusCode(StatusCodes.Status200OK, PostList);
        }

        
        // Returning is the PostName exist
        [HttpGet]
        [Route("ValidatePost")]
        public async Task<IActionResult> ValidateClient([FromBody] ValidatePost postName)
        {
            DB_controller dB_Controller = DB_controller.instance();

            Post post = await dB_Controller.FindPostByName(postName.PostName);

            bool result = post is Post;

            return result ? StatusCode(StatusCodes.Status200OK, result) : StatusCode(StatusCodes.Status400BadRequest, result);
        }


        // Returning a Post with ID
        [HttpGet]
        [Route("{ID:int}")]
        public async Task<IActionResult> ClientId(int ID)
        {
            DB_controller dB_Controller = DB_controller.instance();

            Post post = await dB_Controller.FindById(ID);
            bool result = post is Post;

            return result ? StatusCode(StatusCodes.Status200OK, post) : StatusCode(StatusCodes.Status400BadRequest, "Cant find post with ID: " + ID.ToString());
        }

        //=====================================================
        //Post Methods


        // Creating a Post
        [HttpPost]
        [Route("AddPost")]
        public async Task<IActionResult> AddPost([FromBody] Post newPost)
        {
            DB_controller dB_Controller = DB_controller.instance();

            int newPostId = await dB_Controller.AddPost(newPost);

            bool result = newPostId > 0;
            return result ? StatusCode(StatusCodes.Status200OK, newPostId) : StatusCode(StatusCodes.Status400BadRequest, "Cant create Post \n Post Name in use");
        }

        // Creating a Comment
        [HttpPost]
        [Route("{ID:int}/AddComment")]
        public async Task<IActionResult> AddComment(int ID, [FromBody] MainThread newComment)
        {
            DB_controller dB_Controller = DB_controller.instance();

            int CommentID = await dB_Controller.AddPostComment(ID, newComment);
            bool result = CommentID != 0;
            return result? StatusCode(StatusCodes.Status200OK, CommentID) : StatusCode(StatusCodes.Status400BadRequest, "Cant create new Comment for the post with ID: " + ID.ToString());
        }
        
        // Creating a Reply
        [HttpPost]
        [Route("{ID:int}/Comment/{CommentID:int}/AddReply")]
        public async Task<IActionResult> AddReply(int ID, int CommentID, [FromBody] SecondThread newReply)
        {
            DB_controller dB_Controller = DB_controller.instance();

            int TReplyID = await dB_Controller.AddReply(ID, CommentID, newReply);
            bool result = TReplyID != 0;
            return result ? StatusCode(StatusCodes.Status200OK, TReplyID) : StatusCode(StatusCodes.Status400BadRequest, "Cant create Reply for the comment with ID: " + CommentID.ToString() + " int the post with ID: " + ID.ToString());
        }
        

        //=======================================================
        //Put methods

        // Change a Post
        [HttpPut]
        [Route("EditPost/{ID:int}")]
        public async Task<IActionResult> EditPost(int ID, [FromBody] Post newPost)
        {
            DB_controller dB_Controller = DB_controller.instance();

            bool result = await dB_Controller.ChangePost(ID, newPost);

            return result ? StatusCode(StatusCodes.Status200OK, "ok") : StatusCode(StatusCodes.Status400BadRequest, "Invalid Post ID - ID given: " + ID.ToString());

        }

        
        // Change a Comment
        [HttpPut]
        [Route("{ID:int}/EditComment/{CommentID:int}")]
        public async Task<IActionResult> EditComment(int ID, int CommentID, [FromBody] MainThread newComment)
        {
            DB_controller dB_Controller = DB_controller.instance();

            bool result = await dB_Controller.ChangeComment(ID, CommentID, newComment);

            return result ? StatusCode(StatusCodes.Status200OK, "ok") : StatusCode(StatusCodes.Status400BadRequest, "Invalid Post ID or Invalid Comment ID");

        }

        
        // Change a Reply
        [HttpPut]
        [Route("{ID:int}/Comment/{CommentID:int}/EditReply/{ReplyID:int}")]
        public async Task<IActionResult> EditReply(int ID, int CommentID, int ReplyID, [FromBody] SecondThread newReply)
        {
            DB_controller dB_Controller = DB_controller.instance();

            bool result = await dB_Controller.ChangeReply(ID, CommentID, ReplyID, newReply);

            return result ? StatusCode(StatusCodes.Status200OK, "ok") : StatusCode(StatusCodes.Status400BadRequest, "Invalid Post ID or Invalid Comment ID or Invalid Reply ID");

        }
        
        //=========================================================
        //Delete methods


        // Deleting a Post
        [HttpDelete]
        [Route("DeletePost/{ID:int}")]
        public async Task<IActionResult> DeletePost(int ID)
        {
            DB_controller dB_Controller = DB_controller.instance();

            bool result = await dB_Controller.RemovePost(ID);

            return result ? StatusCode(StatusCodes.Status200OK, "ok") : StatusCode(StatusCodes.Status400BadRequest, "Invalid Post ID");
        }

        
        // Deleting a Comment
        [HttpDelete]
        [Route("{ID:int}/DeleteComment/{CommentID:int}")]
        public async Task<IActionResult> DeleteComment(int ID, int CommentID)
        {
            DB_controller dB_Controller = DB_controller.instance();

            bool result = await dB_Controller.RemoveComment(ID, CommentID);

            return result ? StatusCode(StatusCodes.Status200OK, "ok") : StatusCode(StatusCodes.Status400BadRequest, "Invalid Post ID or Invalid Comment ID");
        }

        
        // Deleting a Reply
        [HttpDelete]
        [Route("{ID:int}/Comment/{CommentID:int}/DeleteReply/{ReplyID:int}")]
        public async Task<IActionResult> DeleteReply(int ID, int CommentID, int ReplyID)
        {
            DB_controller dB_Controller = DB_controller.instance();

            bool result = await dB_Controller.RemoveReply(ID, CommentID, ReplyID);

            return result ? StatusCode(StatusCodes.Status200OK, "ok") : StatusCode(StatusCodes.Status400BadRequest, "Invalid Post ID or Invalid Comment ID or Invalid Reply ID");
        }
        
    }
}
