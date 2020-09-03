using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tomagotchi.Models;

namespace Tomagotchi.Controllers
{
    // All of these routes will be at the base URL:     /api/Pets
    // That is what "api/[controller]" means below. It uses the name of the controller
    // in this case PetsController to determine the URL

    [ApiController]
    public class PetsController : ControllerBase
    {
        // This is the variable you use to have access to your database
        private readonly DatabaseContext _context;

        // Constructor that recives a reference to your database context
        // and stores it in _context for you to use in your API methods
        public PetsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Pets
        //
        // Returns a list of all your Pets
        //
        [HttpGet]
        public ActionResult<IEnumerable<Pet>> GetAll()
        {

            return Ok(_context.Pets);
        }

        // GET: api/Pets/5
        //
        // Fetches and returns a specific pet by finding it by id. The id is specified in the
        // URL. In the sample URL above it is the `5`.  The "{id}" in the [HttpGet("{id}")] is what tells dotnet
        // to grab the id from the URL. It is then made available to us as the `id` argument to the method.
        //
        [HttpGet("{id}")]
        public ActionResult<Pet> GetByID(int id)
        {
            var pet = _context.Pets.FirstOrDefault(pet => pet.Id == id);
            if (pet == null)
            {
                return NotFound();
            }
            return Ok(pet);
        }

        // PUT: api/Pets/5
        //
        // Update an individual pet with the requested id. The id is specified in the URL
        // In the sample URL above it is the `5`. The "{id} in the [HttpPut("{id}")] is what tells dotnet
        // to grab the id from the URL. It is then made available to us as the `id` argument to the method.
        //
        // In addition the `body` of the request is parsed and then made available to us as a Pet
        // variable named pet. The controller matches the keys of the JSON object the client
        // supplies to the names of the attributes of our Pet POCO class. This represents the
        // new values for the record.
        //

        // POST: api/Pets
        //
        // Creates a new pet in the database.
        //
        // The `body` of the request is parsed and then made available to us as a Pet
        // variable named pet. The controller matches the keys of the JSON object the client
        // supplies to the names of the attributes of our Pet POCO class. This represents the
        // new values for the record.
        //
        [HttpPost]
        public ActionResult<Pet> Create(Pet petToCreate)
        {
            petToCreate.Birthday = DateTime.Now;
            petToCreate.HungerLevel = 0;
            petToCreate.HappinessLevel = 0;
            Console.WriteLine($"Name: {petToCreate.Name}");
            _context.Pets.Add(petToCreate);
            _context.SaveChanges();
            return CreatedAtAction(null, null, petToCreate);
        }
        [HttpPut("{id}")]
        public ActionResult<Pet> AddHappinessAndHungerLevel(int id, Pet add)
        {
            var foundPet = _context.Pets.FirstOrDefault(pet => pet.Id == id);
            if (foundPet == null)
            {
                return NotFound();
            }
            foundPet.HungerLevel = add.HungerLevel + 3;
            foundPet.HappinessLevel = add.HappinessLevel + 5;

            _context.Entry(foundPet).State = EntityState.Modified;
            _context.SaveChanges();
            return Ok(foundPet);



        }

        [HttpPut("{id}")]
        public ActionResult<Pet> SubtractHappinessAndHunger(int id, Pet subtract)
        {
            var foundPet = _context.Pets.FirstOrDefault(pet => pet.Id == id);
            if (foundPet == null)
            {
                return NotFound();
            }
            foundPet.HungerLevel = subtract.HungerLevel - 5;
            foundPet.HappinessLevel = subtract.HappinessLevel - 3;

            return Ok(foundPet);
        }
        public ActionResult<Pet> SubtractHappiness(int id, Pet subtractHappiness)
        {
            var foundPet = _context.Pets.FirstOrDefault(pet => pet.Id == id);
            if (foundPet == null)
            {
                return NotFound();
            }
            foundPet.HungerLevel = subtractHappiness.HungerLevel - 5;
            foundPet.HappinessLevel = subtractHappiness.HappinessLevel - 3;

            return Ok(foundPet);
        }

        // DELETE: api/Pets/5
        //
        // Deletes an individual pet with the requested id. The id is specified in the URL
        // In the sample URL above it is the `5`. The "{id} in the [HttpDelete("{id}")] is what tells dotnet
        // to grab the id from the URL. It is then made available to us as the `id` argument to the method.
        //
        [HttpDelete("{id}")]

        public ActionResult<Pet> Delete(int id)
        {
            var foundGame = _context.Pets.FirstOrDefault(pet => pet.Id == id);
            if (foundGame == null)
            {
                return NotFound();
            }
            _context.Pets.Remove(foundGame);
            _context.SaveChanges();
            return Ok(foundGame);
        }
    }
}
