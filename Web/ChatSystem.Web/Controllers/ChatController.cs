using ChatSystem.Services.Services;
using ChatSystem.Services.Services.Contracts;
using ChatSystem.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatSystem.Web.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly IUserService _userService;
        private readonly IChatService _chatService;

        public ChatController(IUserService userService, IChatService chatService)
        {
            _userService = userService;
            _chatService = chatService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllUsersAsync();

            return View(users);
        }

        public async Task<IActionResult> GetMessages(int userId, int skip, int take)
        {
            var messages = await _chatService.GetChatMessagesByUserIdsAsync(userId, skip, take);

            return Json(messages);
        }
    }
}