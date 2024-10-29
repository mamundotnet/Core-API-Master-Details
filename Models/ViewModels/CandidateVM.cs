using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Web.Models.ViewModels
{
    public class CandidateVM
    {
        public int CandidateId { get; set; }
        [Required, StringLength(50), Display(Name = "Candidate Name")]
        public string CandidateName { get; set; } = default!;
        [Required, Display(Name = "Date Of Birth"), Column(TypeName = "date"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public string? Image { get; set; }
        [Display(Name ="Image")]
        public IFormFile? ImagePath { get; set; }
        public bool Fresher { get; set; }
        public string SkillsStringify { get; set; } = default!;
        public List<Skill> SkillList { get; set; }
    }
}
