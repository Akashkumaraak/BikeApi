using BikeApi.Models;

using Microsoft.AspNetCore.Mvc;

namespace BikeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CredentialController : ControllerBase
    {
            private readonly BikeContext _dc;
            public CredentialController(BikeContext dc)
            {
                _dc = dc;
            }
            [HttpPost]
            public async Task<ActionResult<User>> Register(User user)
            {
                _dc.Users.Add(user);
                await _dc.SaveChangesAsync();
                return Ok(user);
            }
            [HttpPost]
            [Route("UserLogin")]
            public async Task<ActionResult<User>> UserLogin(User user)
            {
                try
                {
                    var result = (from i in _dc.Users
                                  where i.EmailId == user.EmailId && i.Password == user.Password
                                  select i).FirstOrDefault();
                    if (result == null)
                    {
                        return BadRequest("Invalid Credentials");
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                      "Wrong Entry");
                }
            }
            [HttpPost]
            [Route("AdminLogin")]
            public async Task<ActionResult<Admin>> AdminLogin(Admin admin)
            {
                try
                {
                    var result = (from i in _dc.Admis
                                  where i.Username == admin.Username && i.Password == admin.Password
                                  select i).FirstOrDefault();
                    if (result == null)
                    {
                        return BadRequest("Invalid Credentials");
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                      "Wrong Entry");
                }
            }

        }
    }


