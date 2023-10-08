using GalaxyQuest.Entities;
using GalaxyQuest.EntitiesDTO;
using GalaxyQuest.Utilities;
using GalaxyQuest.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace GalaxyQuest.Controllers
{
    [ApiController]
    [Route("Users")]
    public class UserController : Controller
    {
        private readonly UserRepository _userRepository;
        private readonly MapRepository _mapRepository;

        public UserController(UserRepository userRepository, MapRepository mapRepository) => (_userRepository, _mapRepository) = (userRepository, mapRepository);

        [HttpPut("{id}")]
        public IActionResult CreateUser(string id, [FromBody] User user)
        {
            var newUserDTO = new UserDTO(user.Id, user.Pseudo)
            {
                Units = { new Unit("scout", _mapRepository.GetRandomSystemName()) }
            };
            _userRepository.Users.Add(newUserDTO);
            return newUserDTO.Id != id || !Regex.IsMatch(id, "^[a-zA-Z0-9_-]+$") ? BadRequest() : ResponseHelper.HandleResponse(newUserDTO);
        }

        [HttpGet("{id}")]
        public IActionResult CanFetchCreatedUser(string id)
        {
            var user = _userRepository.Users.FirstOrDefault(x => x.Id == id);
            return ResponseHelper.HandleResponse(user);
        }
    }
}
