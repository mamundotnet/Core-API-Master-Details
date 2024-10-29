using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class Skill
    {
        public int SkillId { get; set; }
        [Required, StringLength(50), Display(Name = "Skill Name")]
        public string SkillName { get; set; } = default!;

        public virtual ICollection<CandidateSkill> SkillCandidates { get; set; }=new List<CandidateSkill>();
    }
}
