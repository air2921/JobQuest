using domain.Models;
using domain.Models.Chat;
using Microsoft.EntityFrameworkCore;

namespace datahub.Entity_Framework;

public class AppDbContext : DbContext
{
    public DbSet<UserModel> Users { get; set; }
    public DbSet<CompanyModel> Companies { get; set; }
    public DbSet<ResumeModel> Resumes { get; set; }
    public DbSet<LanguageModel> Languages { get; set; }
    public DbSet<EducationModel> Educations { get; set; }
    public DbSet<ExperienceModel> Experiences { get; set; }
    public DbSet<VacancyModel> Vacancies { get; set; }
    public DbSet<ReviewModel> Reviews { get; set; }
    public DbSet<ResponseModel> Responses { get; set; }
    public DbSet<FavoriteModel> Favorites { get; set; }
    public DbSet<AuthModel> Auths { get; set; }
    public DbSet<RecoveryModel> Recoveries { get; set; }
    public DbSet<ChatModel> Chats { get; set; }
    public DbSet<MessageModel> Messages { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        if (!Database.EnsureCreated())
            Database.Migrate();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LanguageModel>()
            .HasOne(r => r.User)
            .WithMany(r => r.Languages)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CompanyModel>()
            .HasOne(r => r.User)
            .WithOne(r => r.Company)
            .HasForeignKey<CompanyModel>(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ResumeModel>()
            .HasOne(r => r.User)
            .WithMany(r => r.Resumes)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<VacancyModel>()
            .HasOne(v => v.Company)
            .WithMany(v => v.Vacancies)
            .HasForeignKey(v => v.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ReviewModel>()
            .HasOne(r => r.Company)
            .WithMany(r => r.Reviews)
            .HasForeignKey(r => r.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ReviewModel>()
            .HasOne(r => r.User)
            .WithMany(r => r.Reviews)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ResponseModel>()
            .HasOne(r => r.Resume)
            .WithMany(r => r.Responses)
            .HasForeignKey(r => r.ResumeId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ResponseModel>()
            .HasOne(r => r.Vacancy)
            .WithMany(r => r.Responses)
            .HasForeignKey(r => r.VacancyId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<FavoriteModel>()
            .HasOne(f => f.User)
            .WithMany(r => r.Favorites)
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<FavoriteModel>()
            .HasOne(f => f.Vacancy)
            .WithMany(v => v.Favorites)
            .HasForeignKey(f => f.VacancyId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<EducationModel>()
            .HasOne(e => e.Resume)
            .WithMany(e => e.Educations)
            .HasForeignKey(e => e.ResumeId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ExperienceModel>()
            .HasOne(e => e.Resume)
            .WithMany(e => e.Experiences)
            .HasForeignKey(e => e.ResumeId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<AuthModel>()
            .HasOne(a => a.User)
            .WithMany(a => a.Auths)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RecoveryModel>()
            .HasOne(r => r.User)
            .WithMany(r => r.Recoveries)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ChatModel>()
            .HasOne(c => c.CandidateUser)
            .WithMany(u => u.CandidateChats)
            .HasForeignKey(c => c.CandidateId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ChatModel>()
            .HasOne(c => c.EmployerUser)
            .WithMany(u => u.EmployerChats)
            .HasForeignKey(c => c.EmployerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MessageModel>()
            .HasOne(m => m.Chat)
            .WithMany(m => m.Messages)
            .HasForeignKey(m => m.ChatId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MessageModel>()
            .HasOne(m => m.Employer)
            .WithMany(u => u.SentMessagesAsEmployer)
            .HasForeignKey(m => m.EmployerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MessageModel>()
            .HasOne(m => m.Candidate)
            .WithMany(u => u.SentMessagesAsCandidate)
            .HasForeignKey(m => m.CandidateId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserModel>()
            .HasIndex(x => x.Email);

        modelBuilder.Entity<RecoveryModel>()
            .HasIndex(x => x.Value);

        modelBuilder.Entity<AuthModel>()
            .HasIndex(x => x.Value);
    }
}
