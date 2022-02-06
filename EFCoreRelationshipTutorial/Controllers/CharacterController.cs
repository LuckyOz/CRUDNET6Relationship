using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreRelationshipTutorial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharacterController : ControllerBase
    {
        private readonly DataContext _context;

        public CharacterController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Character>>> GetCharacter(int userId)
        {
            var character = await _context.Characters.Include(q => q.Weapon).Include(q => q.Skills).Where(q => q.UserId == userId).ToListAsync();

            return character;
        }

        [HttpPost]
        public async Task<ActionResult<List<Character>>> Create(CreateCharacter request)
        {
            var user = await _context.Users.FindAsync(request.UserId);
            if (user == null)
                return NotFound();

            var newCharacter = new Character
            {
                Name = request.Name,
                RpgClass = request.RpgClass,
                User = user,
                UserId = user.Id
            };

            _context.Characters.Add(newCharacter);
            await _context.SaveChangesAsync();

            return await GetCharacter(newCharacter.UserId);
        }

        [HttpPost("weapons")]
        public async Task<ActionResult<Character>> AddWeapon(CreateWeapon request)
        {
            var character = await _context.Characters.FindAsync(request.CharacterId);

            if (character == null)
                return NotFound();

            var newWeapon = new Weapon
            {
                Name = request.Name,
                Demage = request.Demage,
                Character = character,
                CharacterId = character.Id
            };

            _context.Weapons.Add(newWeapon);
            await _context.SaveChangesAsync();

            return character;
        }

        [HttpPost("skill")]
        public async Task<ActionResult<Character>> AddCharacterSkill(AddCharacterSkill request)
        {
            var character = await _context.Characters.Where(q => q.Id == request.CharacterId).Include(q => q.Skills).FirstOrDefaultAsync();
            var skill = await _context.Skills.FindAsync(request.SkillId);

            if (character == null || skill == null)
                return NotFound();

            character.Skills.Add(skill);
            await _context.SaveChangesAsync();

            return character;
        }
    }
}
