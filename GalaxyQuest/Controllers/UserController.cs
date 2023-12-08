using GalaxyQuest.Entities;
using GalaxyQuest.EntitiesDTO;
using GalaxyQuest.Utilities;
using GalaxyQuest.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using GalaxyQuest.RequestBodies;
using GalaxyQuest.Units;
using Microsoft.AspNetCore.Authorization;

namespace GalaxyQuest.Controllers
{
    [ApiController]
    [Route("Users")]
    public class UserController : Controller
    {
        private readonly UserRepository _userRepository;
        private readonly MapRepository _mapRepository;

        public UserController(UserRepository userRepository, MapRepository mapRepository)
        {
            (_userRepository, _mapRepository) = (userRepository, mapRepository);
        }

        /// <summary>
        /// Creates a new user with random units and adds it to the repository.
        /// </summary>
        [HttpPut("{id}")]
        public IActionResult CreateUser(string id, [FromBody] PostRequestUserBody user)
        {
            
            // Generate random units for the user
            var random = new Random(43);
            var randomSystemForUnits = _mapRepository.Systems[random.Next(_mapRepository.Systems.Count)];
            var randomPlanetForUnits =
                randomSystemForUnits.Planets[random.Next(randomSystemForUnits.Planets.Count)];

            Unit scout = new("scout", randomSystemForUnits.Name, randomPlanetForUnits.Name, user.Id);
            Unit builder = new("builder", randomSystemForUnits.Name, randomPlanetForUnits.Name, user.Id);

            UserDTO userDto = _userRepository.Users.FirstOrDefault(x => x.Id == id);
            
            if (userDto != null)
            {
                if (User.Identity.IsAuthenticated)
                {
                    userDto.Pseudo = user.Pseudo;
                    userDto.DateOfCreation = user.DateOfCreation;
                    userDto.ResourcesQuantity = user.ResourcesQuantity;
                }
                
                return userDto.Id != id || !Regex.IsMatch(id, "^[a-zA-Z0-9_-]+$")
                    ? BadRequest("Invalid user ID format or mismatch.")
                    : ResponseHelper.HandleResponse(userDto);
            }

            // Create and add a new user DTO with generated units
            var userDTO = new UserDTO(user.Id, user.Pseudo)
            {
                Units = { scout, builder }
            };
            
            userDTO.AddUnit(scout);
            userDTO.AddUnit(builder);
            _userRepository.Users.Add(userDTO);

            return userDTO.Id != id || !Regex.IsMatch(id, "^[a-zA-Z0-9_-]+$")
                ? BadRequest("Invalid user ID format or mismatch.")
                : ResponseHelper.HandleResponse(userDTO);
        }

        /// <summary>
        /// Checks if a user with the specified ID exists.
        /// </summary>
        [HttpGet("{id}")]
        public IActionResult CanFetchCreatedUser(string id)
        {
            var user = _userRepository.Users.FirstOrDefault(x => x.Id == id);
            return ResponseHelper.HandleResponse(user, notFoundMessage: "User not found.");
        }
    }
}
