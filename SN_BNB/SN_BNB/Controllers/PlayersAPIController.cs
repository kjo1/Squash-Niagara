using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SN_BNB.Data;
using SN_BNB.Models;

namespace SN_BNB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersAPIController : ControllerBase
    {
        private readonly SNContext _context;

        public PlayersAPIController(SNContext context)
        {
            _context = context;
        }

        // GET: api/PlayersAPI
        [HttpGet("Home/{id}")]
        public IEnumerable<Player> GetHomePlayers([FromRoute] int id)
        {
            IEnumerable<Player> player = null;
            try
            {
                player = _context.Players.Include(p => p.Team).ThenInclude(t => t.HomeFixtures).Where(p => p.Team.HomeFixtures.Any(f => f.ID == id));
            }
            catch(Exception e)
            {
                e.ToString();                
            }
            return player;
        }

        [HttpGet("Away/{id}")]
        public IEnumerable<Player> GetAwayPlayers([FromRoute] int id)
        {
            return _context.Players.Include(p => p.Team).ThenInclude(t => t.AwayFixtures).Where(p => p.Team.AwayFixtures.Any(f => f.ID == id));
        }

        [HttpGet]
        public string Test()
        {
            return _context.Players.First().Email;
        }


    }
}