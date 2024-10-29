using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Web.Models.ViewModels;
using Web.Models;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidatesController : ControllerBase
    {
        private readonly CandidateDbContext _context;
        private readonly IWebHostEnvironment _env;
        public CandidatesController(CandidateDbContext _context, IWebHostEnvironment _env)
        {
            this._context = _context;
            this._env = _env;
        }

        [HttpGet]
        [Route("GetSkills")]
        public async Task<ActionResult<IEnumerable<Skill>>> GetSkills()
        {
            return await _context.Skills.ToListAsync();
        }


        [HttpGet]
        [Route("GetCandidates")]
        public async Task<ActionResult<IEnumerable<Candidate>>> GetCandidates()
        {
            return await _context.Candidates.ToListAsync();
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CandidateVM>>> GetCandidateSkills()
        {
            List<CandidateVM> candidateSkills = new List<CandidateVM>();
            var allCandidates = _context.Candidates.ToList();
            foreach (var candidate in allCandidates)
            {
                var skillList = _context.CandidateSkills.Where(x => x.CandidateId == candidate.CandidateId).Select(x => new Skill
                {
                    SkillId = x.SkillId,
                    SkillName = x.Skill.SkillName
                }).ToList();

                candidateSkills.Add(new CandidateVM
                {
                    CandidateId = candidate.CandidateId,
                    CandidateName = candidate.CandidateName,
                    DateOfBirth = candidate.DateOfBirth,
                    Phone = candidate.Phone,
                    Fresher = candidate.Fresher,
                    Image = candidate.Image,
                    SkillList = skillList.ToList()

                });
            }
            return candidateSkills;
        }

        [HttpPost]
        public async Task<ActionResult<CandidateSkill>> PostCandidateSkills([FromForm] CandidateVM vm)
        {
            var skillItems = JsonConvert.DeserializeObject<Skill[]>(vm.SkillsStringify);

            Candidate candidate = new Candidate
            {
                CandidateName = vm.CandidateName,
                DateOfBirth = vm.DateOfBirth,
                Phone = vm.Phone,
                Fresher = vm.Fresher
            };
           
            if (vm.ImagePath != null)
            {
                var webroot = _env.WebRootPath;
                var fileName = DateTime.Now.Ticks.ToString() + Path.GetExtension(vm.ImagePath.FileName);
                var filePath = Path.Combine(webroot, "Images", fileName);

                FileStream fileStream = new FileStream(filePath, FileMode.Create);
                await vm.ImagePath.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
                fileStream.Close();
                candidate.Image = fileName;
            }
           

            foreach (var item in skillItems)
            {
                var candidateSkill = new CandidateSkill
                {
                    Candidate = candidate,
                    CandidateId = candidate.CandidateId,
                    SkillId = item.SkillId
                };
                _context.Add(candidateSkill);
            }
            await _context.SaveChangesAsync();
            return Ok(candidate);
        }

        [Route("Update")]
        [HttpPut]
        public async Task<ActionResult<CandidateSkill>> UpdateCandidateSkills([FromForm] CandidateVM vm)
        {
            var skillItems = JsonConvert.DeserializeObject<Skill[]>(vm.SkillsStringify);

            Candidate candidate = _context.Candidates.Find(vm.CandidateId);
            candidate.CandidateName = vm.CandidateName;
            candidate.DateOfBirth = vm.DateOfBirth;
            candidate.Phone = vm.Phone;
            candidate.Fresher = vm.Fresher;

            if (vm.ImagePath != null)
            {
                var webroot = _env.WebRootPath;
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(vm.ImagePath.FileName);
                var filePath = Path.Combine(webroot, "Images", fileName);

                FileStream fileStream = new FileStream(filePath, FileMode.Create);
                await vm.ImagePath.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
                fileStream.Close();
                candidate.Image = fileName;
            }

          
            var existingSkills = _context.CandidateSkills.Where(x => x.CandidateId == candidate.CandidateId).ToList();
            foreach (var item in existingSkills)
            {
                _context.CandidateSkills.Remove(item);
            }

           
            foreach (var item in skillItems)
            {
                var candidateSkill = new CandidateSkill
                {
                    CandidateId = candidate.CandidateId,
                    SkillId = item.SkillId

                };
                _context.Add(candidateSkill);
            }

            _context.Entry(candidate).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(candidate);
        }


        [Route("Delete/{id}")]
        [HttpDelete]
        public async Task<ActionResult<CandidateSkill>> DeleteCandidateSkill(int id)
        {
            Candidate candidate = _context.Candidates.Find(id);

            var existingSkills = _context.CandidateSkills.Where(x => x.CandidateId == candidate.CandidateId).ToList();
            foreach (var item in existingSkills)
            {
                _context.CandidateSkills.Remove(item);
            }
            _context.Entry(candidate).State = EntityState.Deleted;

            await _context.SaveChangesAsync();

            return Ok(candidate);
        }
    }
}
