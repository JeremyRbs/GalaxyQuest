using GalaxyQuest.Entities;
using GalaxyQuest.EntitiesDTO;
using GalaxyQuest.Utilities;
using GalaxyQuest.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using GalaxyQuest.Units;

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
        public IActionResult CreateUser(string id, [FromBody] User user)
        {
            // Generate random units for the user
            var random = new Random(43);
            var randomSystemForUnits = _mapRepository.Systems[random.Next(_mapRepository.Systems.Count)];
            var randomPlanetForUnits = randomSystemForUnits.Planets[random.Next(randomSystemForUnits.Planets.Count)];

            ScoutUnit scout = new(randomSystemForUnits.Name, randomPlanetForUnits.Name, user.Id);
            BuilderUnit builder = new(randomSystemForUnits.Name, randomPlanetForUnits.Name, user.Id);

            // Create and add a new user DTO with generated units
            var newUserDTO = new UserDTO(user.Id, user.Pseudo)
            {
                Units = { scout, builder }
            };
            newUserDTO.AddUnit(scout);
            newUserDTO.AddUnit(builder);
            _userRepository.Users.Add(newUserDTO);

            return newUserDTO.Id != id || !Regex.IsMatch(id, "^[a-zA-Z0-9_-]+$")
                ? BadRequest("Invalid user ID format or mismatch.")
                : ResponseHelper.HandleResponse(newUserDTO);
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
