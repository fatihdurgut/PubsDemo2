using Microsoft.EntityFrameworkCore;
using PubsApp.Core.Entities;

namespace PubsApp.Infrastructure.Data;

/// <summary>
/// Database context for the Pubs database
/// </summary>
public class PubsDbContext : DbContext
{
    public PubsDbContext(DbContextOptions<PubsDbContext> options) : base(options)
    {
    }

    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Title> Titles => Set<Title>();
    public DbSet<Publisher> Publishers => Set<Publisher>();
    public DbSet<TitleAuthor> TitleAuthors => Set<TitleAuthor>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Author entity
        modelBuilder.Entity<Author>(entity =>
        {
            entity.ToTable("authors");
            entity.HasKey(e => e.AuthorId);
            
            entity.Property(e => e.AuthorId)
                .HasColumnName("au_id")
                .HasMaxLength(11)
                .IsRequired();
            
            entity.Property(e => e.LastName)
                .HasColumnName("au_lname")
                .HasMaxLength(40)
                .IsRequired();
            
            entity.Property(e => e.FirstName)
                .HasColumnName("au_fname")
                .HasMaxLength(20)
                .IsRequired();
            
            entity.Property(e => e.Phone)
                .HasColumnName("phone")
                .HasColumnType("char(12)")
                .IsRequired();
            
            entity.Property(e => e.Address)
                .HasColumnName("address")
                .HasMaxLength(40);
            
            entity.Property(e => e.City)
                .HasColumnName("city")
                .HasMaxLength(20);
            
            entity.Property(e => e.State)
                .HasColumnName("state")
                .HasColumnType("char(2)");
            
            entity.Property(e => e.Zip)
                .HasColumnName("zip")
                .HasColumnType("char(5)");
            
            entity.Property(e => e.Contract)
                .HasColumnName("contract")
                .IsRequired();
        });

        // Configure Publisher entity
        modelBuilder.Entity<Publisher>(entity =>
        {
            entity.ToTable("publishers");
            entity.HasKey(e => e.PublisherId);
            
            entity.Property(e => e.PublisherId)
                .HasColumnName("pub_id")
                .HasColumnType("char(4)")
                .IsRequired();
            
            entity.Property(e => e.PublisherName)
                .HasColumnName("pub_name")
                .HasMaxLength(40);
            
            entity.Property(e => e.City)
                .HasColumnName("city")
                .HasMaxLength(20);
            
            entity.Property(e => e.State)
                .HasColumnName("state")
                .HasColumnType("char(2)");
            
            entity.Property(e => e.Country)
                .HasColumnName("country")
                .HasMaxLength(30);
        });

        // Configure Title entity
        modelBuilder.Entity<Title>(entity =>
        {
            entity.ToTable("titles");
            entity.HasKey(e => e.TitleId);
            
            entity.Property(e => e.TitleId)
                .HasColumnName("title_id")
                .HasMaxLength(6)
                .IsRequired();
            
            entity.Property(e => e.TitleName)
                .HasColumnName("title")
                .HasMaxLength(80)
                .IsRequired();
            
            entity.Property(e => e.Type)
                .HasColumnName("type")
                .HasColumnType("char(12)")
                .IsRequired();
            
            entity.Property(e => e.PublisherId)
                .HasColumnName("pub_id")
                .HasColumnType("char(4)");
            
            entity.Property(e => e.Price)
                .HasColumnName("price")
                .HasColumnType("money");
            
            entity.Property(e => e.Advance)
                .HasColumnName("advance")
                .HasColumnType("money");
            
            entity.Property(e => e.Royalty)
                .HasColumnName("royalty");
            
            entity.Property(e => e.YtdSales)
                .HasColumnName("ytd_sales");
            
            entity.Property(e => e.Notes)
                .HasColumnName("notes")
                .HasMaxLength(200);
            
            entity.Property(e => e.PublishedDate)
                .HasColumnName("pubdate")
                .HasColumnType("datetime")
                .IsRequired();

            // Configure relationship with Publisher
            entity.HasOne(t => t.Publisher)
                .WithMany(p => p.Titles)
                .HasForeignKey(t => t.PublisherId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Configure TitleAuthor entity (many-to-many)
        modelBuilder.Entity<TitleAuthor>(entity =>
        {
            entity.ToTable("titleauthor");
            entity.HasKey(e => new { e.AuthorId, e.TitleId });
            
            entity.Property(e => e.AuthorId)
                .HasColumnName("au_id")
                .HasMaxLength(11)
                .IsRequired();
            
            entity.Property(e => e.TitleId)
                .HasColumnName("title_id")
                .HasMaxLength(6)
                .IsRequired();
            
            entity.Property(e => e.AuthorOrder)
                .HasColumnName("au_ord");
            
            entity.Property(e => e.RoyaltyPercentage)
                .HasColumnName("royaltyper");

            // Configure relationships
            entity.HasOne(ta => ta.Author)
                .WithMany(a => a.TitleAuthors)
                .HasForeignKey(ta => ta.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(ta => ta.Title)
                .WithMany(t => t.TitleAuthors)
                .HasForeignKey(ta => ta.TitleId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
