﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClickerAPI.Models;
using ClickerAPI.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ClickerAPI.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("My Policy")]
    public class UsersController : Controller
    {
        private readonly UserService _userService;
        public UsersController(UserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        public ActionResult<List<User>> Get() =>
            _userService.Get();

        [HttpGet("{id:length(24)}", Name = "GetUser")]
        public ActionResult<User> Get(string id)
        {
            var user = _userService.Get(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPost]
        [EnableCors("My Policy")]
        public ActionResult<User> Create([FromBody] User user)
        {

            if(_userService.GetByUsername(user.Username) == null)
            {
                _userService.Create(user);
            }
            else
            {
                return StatusCode(409, "User exists in database!");
            }
           
            return CreatedAtRoute("GetUser", new { id = user.Id.ToString() }, user);
        }

        public class Login
        {
            public string username { get; set; }
            public string password { get; set; }
        }
        [Route("/api/login")]
        [HttpPost]
        [EnableCors("My Policy")]
        public ActionResult LoginReq([FromBody] Login login)
        {
            Console.WriteLine(login.username + login.password);
            if (_userService.GetByUsername(login.username) == null)
            {
                return StatusCode(404, "User doesn't exists!");
            }
            User user = _userService.GetByUsername(login.username);
            if(user.Password != login.password)
            {
                return StatusCode(401, "Wrong password or username");
            }

            return Ok(new { Message = "Ok", StatusCode = "200" });
        } 

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, User userIn)
        {
            var user = _userService.Get(id);

            if(user == null)
            {
                return NotFound();
            }

            _userService.Update(id, userIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var user = _userService.Get(id);

            if (user == null)
            {
                return NotFound();
            }

            _userService.Remove(user.Id);

            return NoContent();
        }


    }
}
