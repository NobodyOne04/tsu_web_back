using Backend6.Application;
using Backend6.Core;
using Backend6.Core.Blog;
using Backend6.Core.Drive;
using Backend6.Core.Forum;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Backend6.Infrastructure;

public class ApplicationUser : IdentityUser;

public static class Roles
{
    public const string Administrators = nameof(Administrators);
}

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<ApplicationUser>(options), IUnitOfWork
{
    public DbSet<PostCategory> PostCategories => Set<PostCategory>();
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<ForumCategory> ForumCategories => Set<ForumCategory>();
    public DbSet<Forum> Forums => Set<Forum>();
    public DbSet<ForumTopic> ForumTopics => Set<ForumTopic>();
    public DbSet<Folder> Folders => Set<Folder>();

    async Task IUnitOfWork.SaveChangesAsync() => await SaveChangesAsync();

    protected override void OnModelCreating(ModelBuilder b)
    {
        base.OnModelCreating(b);  // Identity tables
        b.UsePropertyAccessMode(PropertyAccessMode.Field);
        // the domain creates its own Guid ids, so a new child found through a navigation (a reply, an
        // upload) must be inserted; with database-generated keys EF would assume it exists and update it
        foreach (var type in new[] { typeof(PostCategory), typeof(Post), typeof(PostComment), typeof(PostAttachment),
                     typeof(ForumCategory), typeof(Forum), typeof(ForumTopic), typeof(ForumMessage),
                     typeof(ForumMessageAttachment), typeof(Folder), typeof(DriveFile) })
            b.Entity(type).Property(nameof(Post.Id)).ValueGeneratedNever();

        // users are referenced by id only: the domain doesn't know Identity
        b.Entity<PostCategory>(e => e.Property(x => x.Name).HasMaxLength(Rules.MaxName));
        b.Entity<Post>(e =>
        {
            e.Property(x => x.Title).HasMaxLength(Rules.MaxName);
            e.HasOne(x => x.Category).WithMany().HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.Restrict);
            e.HasMany(x => x.Comments).WithOne(x => x.Post).HasForeignKey(x => x.PostId);
            e.HasMany(x => x.Attachments).WithOne(x => x.Post).HasForeignKey(x => x.PostId);
            e.HasIndex(x => x.Created);
        });
        b.Entity<PostComment>().ToTable("PostComments");
        b.Entity<PostAttachment>(e => { e.ToTable("PostAttachments"); e.Ignore(x => x.IsImage); });

        b.Entity<ForumCategory>(e =>
        {
            e.Property(x => x.Name).HasMaxLength(Rules.MaxName);
            e.HasMany(x => x.Forums).WithOne(x => x.Category).HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.Restrict);
        });
        b.Entity<Forum>(e => e.Property(x => x.Name).HasMaxLength(Rules.MaxName));
        b.Entity<ForumTopic>(e =>
        {
            e.Property(x => x.Name).HasMaxLength(Rules.MaxName);
            e.HasOne(x => x.Forum).WithMany().HasForeignKey(x => x.ForumId).OnDelete(DeleteBehavior.Restrict);
            e.HasMany(x => x.Messages).WithOne(x => x.Topic).HasForeignKey(x => x.TopicId);
            e.Ignore(x => x.StoredFiles);
        });
        b.Entity<ForumMessage>(e =>
        {
            e.ToTable("ForumMessages");
            e.HasMany(x => x.Attachments).WithOne(x => x.Message).HasForeignKey(x => x.MessageId);
        });
        b.Entity<ForumMessageAttachment>(e => { e.ToTable("ForumMessageAttachments"); e.Ignore(x => x.IsImage); });

        b.Entity<Folder>(e =>
        {
            e.Property(x => x.Name).HasMaxLength(Rules.MaxName);
            e.HasOne(x => x.Parent).WithMany(x => x.Folders).HasForeignKey(x => x.ParentId).OnDelete(DeleteBehavior.Restrict);
            e.HasMany(x => x.Files).WithOne(x => x.Folder).HasForeignKey(x => x.FolderId);
            e.HasIndex(x => x.OwnerId);
        });
        b.Entity<DriveFile>(e =>
        {
            e.ToTable("DriveFiles");
            e.Property(x => x.Name).HasMaxLength(Rules.MaxName);
            e.Ignore(x => x.IsImage);
            e.Ignore(x => x.Extension);
        });
    }
}
