using Microsoft.EntityFrameworkCore;

namespace Web.Models
{
    public class CandidateDbContext:DbContext
    {
        public CandidateDbContext(DbContextOptions<CandidateDbContext> options):base(options)
        {
            
        }
        public DbSet<Skill> Skills { get; set; } 
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<CandidateSkill> CandidateSkills { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Skill>().HasData
            (
                new Skill { SkillId=1,SkillName="Singing"},
                new Skill { SkillId=2,SkillName="Dancing"},
                new Skill { SkillId=3,SkillName="Playinig"},
                new Skill { SkillId=4,SkillName="Art"},
                new Skill { SkillId=5,SkillName="Drawing"}
            );
        }
    }
}
