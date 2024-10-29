using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Models
{
    public class Candidate
    {
        public int CandidateId { get; set; }
        [Required,StringLength(50),Display(Name = "Candidate Name")]
        public string CandidateName { get; set; } = default!;
        [Required, Display(Name = "Date Of Birth"), Column(TypeName ="date"),DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}",ApplyFormatInEditMode =true)]
        public DateTime DateOfBirth { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public string? Image { get; set; } 
        public bool Fresher { get; set; }

        public virtual ICollection<CandidateSkill> CandidateSkills { get; set; } = new List<CandidateSkill>();
    }
}
