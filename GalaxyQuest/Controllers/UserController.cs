using GalaxyQuest.Entities;
using GalaxyQuest.EntitiesDTO;
using GalaxyQuest.Utilities;
using GalaxyQuest.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using GalaxyQuest.RequestBodies;
using GalaxyQuest.Units.PeacefulUnits;
using Microsoft.AspNetCore.Authorization;
using Shard.Shared.Core;

namespace GalaxyQuest.Controllers
{
    [ApiController]
    [Route("Users")]
    public class UserController : Controller
    {
        private readonly UserRepository userRepository;
        private readonly MapRepository mapRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="userRepository">The repository for managing user data.</param>
        /// <param name="mapRepository">The repository for managing map-related data.</param>
        public UserController(UserRepository userRepository, MapRepository mapRepository)
        {
            (this.userRepository, this.mapRepository) = (userRepository, mapRepository);
        }


        /// <summary>
        /// Creates a new user with random units and adds it to the repository.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <param name="user">The request body containing user information.</param>
        /// <returns>An IActionResult representing the result of the user creation.</returns>
        [HttpPut("{id}")]
        public IActionResult CreateUser(string id, [FromBody] PostRequestUserBody user)
        {
            // Generate random units for the user
            var random = new Random(43);
            var randomSystemForUnits = mapRepository.Systems[random.Next(mapRepository.Systems.Count)];
            var randomPlanetForUnits = randomSystemForUnits.Planets[random.Next(randomSystemForUnits.Planets.Count)];

            ScoutUnit scout = new(randomSystemForUnits.Name, randomPlanetForUnits.Name, user.Id);
            BuilderUnit builder = new(randomSystemForUnits.Name, randomPlanetForUnits.Name, user.Id);
            CargoUnit cargo = new(randomSystemForUnits.Name, randomPlanetForUnits.Name, user.Id);

            // Check if the user already exists in the repository
            UserDto userDto = userRepository.Users.FirstOrDefault(x => x.Id == id);

            if (userDto != null)
            {
                // Update user information if the user is authenticated
                if (User.Identity.IsAuthenticated)
                {
                    userDto.Pseudo = user.Pseudo;
                    userDto.DateOfCreation = user.DateOfCreation;
                    userDto.ResourcesQuantity = user.ResourcesQuantity;
                }

                // Validate user ID format and check for ID mismatch
                return userDto.Id != id || !Regex.IsMatch(id, "^[a-zA-Z0-9_-]+$")
                    ? BadRequest("UserController_CreateUser: Invalid user ID format or mismatch when the user exists.")
                    : ResponseHelper.HandleResponse(userDto);
            }

            // Create a new user if it doesn't exist
            var newUserDto = new UserDto(user.Id, user.Pseudo, user.DateOfCreation);

            // Initialize resources for shard users
            if (User.IsInRole("shard"))
            {
                foreach (var resource in newUserDto.ResourcesQuantity)
                {
                    newUserDto.ResourcesQuantity[resource.Key] = 0;
                }
            }
            else
            {
                // Add random units for non-shard users
                newUserDto.Units.Add(scout);
                newUserDto.Units.Add(builder);
                newUserDto.Units.Add(cargo);
            }

            // Add the new user to the repository
            userRepository.Users.Add(newUserDto);

            // Validate user ID format and check for ID mismatch
            return newUserDto.Id != id || !Regex.IsMatch(id, "^[a-zA-Z0-9_-]+$")
                ? BadRequest("UserController_CreateUser: Invalid user ID format or mismatch when the user doesn't exist.")
                : ResponseHelper.HandleResponse(newUserDto);
        }

        /// <summary>
        /// Checks if a user with the specified ID exists.
        /// </summary>
        [HttpGet("{id}")]
        public IActionResult CanFetchCreatedUser(string id)
        {
            var user = userRepository.Users.FirstOrDefault(x => x.Id == id);
            return ResponseHelper.HandleResponse(user, notFoundMessage: "UserController_CanFetchCreatedUser : User not found.");
        }
    }
}
