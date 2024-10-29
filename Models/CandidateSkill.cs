using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Models
{
    public class CandidateSkill
    {
        public int CandidateSkillId { get; set; }
        [ForeignKey("Skill")]
        public int SkillId { get; set; }
        [ForeignKey("Candidate")]
        public int CandidateId { get; set; }
    
        public virtual Skill? Skill { get; set; }
        public virtual Candidate? Candidate { get; set; }
    }
}
