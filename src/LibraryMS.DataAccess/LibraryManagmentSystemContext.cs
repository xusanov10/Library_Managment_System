using LibraryMS.Core.Entities;
using Libray_Managment_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Library_Managment_System1;

public class LibraryManagmentSystemContext : DbContext
{
    public LibraryManagmentSystemContext(DbContextOptions<LibraryManagmentSystemContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<Bookcopy> Bookcopies { get; set; }

    public virtual DbSet<Borrowrecord> Borrowrecords { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Fine> Fines { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Publisher> Publishers { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<Reservation> Reservations { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Rolepermission> Rolepermissions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Userprofile> Userprofiles { get; set; }

    public virtual DbSet<Userrole> Userroles { get; set; }

    public virtual DbSet<Order> Orders { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresEnum("bookcopystatus", new[] { "Available", "Borrowed", "Reserved", "Lost" })
            .HasPostgresEnum("borrowstatus", new[] { "Borrowed", "Returned", "Overdue" })
            .HasPostgresEnum("reporttype", new[] { "Daily", "Monthly", "Yearly", "Custom" })
            .HasPostgresEnum("reservationstatus", new[] { "Pending", "Approved", "Cancelled" })
            .HasPostgresEnum("gendertype", new[] { "Mail", "Female" });

        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("authors_pkey");

            entity.ToTable("authors");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Birthyear).HasColumnName("birthyear");
            entity.Property(e => e.Country)
                .HasMaxLength(100)
                .HasColumnName("country");
            entity.Property(e => e.Deathyear).HasColumnName("deathyear");
            entity.Property(e => e.Fullname)
                .HasMaxLength(150)
                .HasColumnName("fullname");
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("books_pkey");

            entity.ToTable("books");

            entity.HasIndex(e => e.Isbn, "books_isbn_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Authorid).HasColumnName("authorid");
            entity.Property(e => e.Categoryid).HasColumnName("categoryid");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'")
                .HasColumnName("createdat");
            entity.Property(e => e.Filepath)
                .HasMaxLength(255)
                .HasColumnName("filepath");
            entity.Property(e => e.Isbn)
                .HasMaxLength(50)
                .HasColumnName("isbn");
            entity.Property(e => e.Publishedyear).HasColumnName("publishedyear");
            entity.Property(e => e.Publisherid).HasColumnName("publisherid");
            entity.Property(e => e.Quantity)
                .HasDefaultValue(1)
                .HasColumnName("quantity");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");

            entity.HasOne(d => d.Author).WithMany(p => p.Books)
                .HasForeignKey(d => d.Authorid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("books_authorid_fkey");

            entity.HasOne(d => d.Category).WithMany(p => p.Books)
                .HasForeignKey(d => d.Categoryid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("books_categoryid_fkey");

            entity.HasOne(d => d.Publisher).WithMany(p => p.Books)
                .HasForeignKey(d => d.Publisherid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("books_publisherid_fkey");
        });

        modelBuilder.Entity<Bookcopy>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("bookcopies_pkey");

            entity.ToTable("bookcopies");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Bookid).HasColumnName("bookid");
            entity.Property(e => e.Copynumber).HasColumnName("copynumber");
            entity.Property(e => e.Status).HasColumnType("bookcopystatus");


            entity.HasOne(d => d.Book).WithMany(p => p.Bookcopies)
                .HasForeignKey(d => d.Bookid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("bookcopies_bookid_fkey");
        });

        modelBuilder.Entity<Borrowrecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("borrowrecords_pkey");

            entity.ToTable("borrowrecords");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Bookcopyid).HasColumnName("bookcopyid");
            entity.Property(e => e.Status).HasColumnType("borrowstatus");
            entity.Property(e => e.Borrowdate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'")
                .HasColumnName("borrowdate");
            entity.Property(e => e.Duedate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'")
                .HasColumnName("duedate");
            entity.Property(e => e.Penalty)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("0")
                .HasColumnName("penalty");
            entity.Property(e => e.Returndate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'")
                .HasColumnName("returndate");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.Bookcopy).WithMany(p => p.Borrowrecords)
                .HasForeignKey(d => d.Bookcopyid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("borrowrecords_bookcopyid_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Borrowrecords)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("borrowrecords_userid_fkey");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("categories_pkey");

            entity.ToTable("categories");

            entity.HasIndex(e => e.Name, "categories_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Fine>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("fines_pkey");

            entity.ToTable("fines");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amount)
                .HasPrecision(10, 2)
                .HasColumnName("amount");
            entity.Property(e => e.Borrowrecordid).HasColumnName("borrowrecordid");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'")
                .HasColumnName("createdat");
            entity.Property(e => e.Paid)
                .HasDefaultValue(false)
                .HasColumnName("paid");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.Borrowrecord).WithMany(p => p.Fines)
                .HasForeignKey(d => d.Borrowrecordid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fines_borrowrecordid_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Fines)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fines_userid_fkey");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("notifications_pkey");

            entity.ToTable("notifications");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'")
                .HasColumnName("createdat");
            entity.Property(e => e.Isread)
                .HasDefaultValue(false)
                .HasColumnName("isread");
            entity.Property(e => e.Message)
                .HasMaxLength(500)
                .HasColumnName("message");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("notifications_userid_fkey");
        });

        modelBuilder.Entity<Payment>(entity =>
        {


            entity.HasKey(e => e.Id).HasName("payments_pkey");

            entity.ToTable("payments");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amount)
                .HasPrecision(10, 2)
                .HasColumnName("amount");
            entity.Property(e => e.Fineid).HasColumnName("fineid");
            entity.Property(e => e.Method)
                .HasMaxLength(50)
                .HasColumnName("method");
            entity.Property(e => e.Paymentdate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'")
                .HasColumnName("paymentdate");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.Fine).WithMany(p => p.Payments)
                .HasForeignKey(d => d.Fineid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("payments_fineid_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Payments)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("payments_userid_fkey");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("permissions_pkey");

            entity.ToTable("permissions");

            entity.HasIndex(e => e.Name, "permissions_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Publisher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("publishers_pkey");

            entity.ToTable("publishers");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.Contactinfo)
                .HasMaxLength(100)
                .HasColumnName("contactinfo");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("reports_pkey");

            entity.ToTable("reports");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Reporttype).HasColumnType("reporttype");
            entity.Property(e => e.Filepath)
                .HasMaxLength(255)
                .HasColumnName("filepath");
            entity.Property(e => e.Generatedat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'")
                .HasColumnName("generatedat");
            entity.Property(e => e.Generatedby).HasColumnName("generatedby");
            entity.Property(e => e.Title)
                .HasMaxLength(150)
                .HasColumnName("title");

            entity.HasOne(d => d.GeneratedbyNavigation).WithMany(p => p.Reports)
                .HasForeignKey(d => d.Generatedby)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("reports_generatedby_fkey");
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("reservations_pkey");

            entity.ToTable("reservations");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Bookcopyid).HasColumnName("bookcopyid");
            entity.Property(e => e.Status).HasColumnType("reservationstatus");
            entity.Property(e => e.Reserveddate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'")
                .HasColumnName("reserveddate");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.Bookcopy).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.Bookcopyid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("reservations_bookcopyid_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("reservations_userid_fkey");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("roles_pkey");

            entity.ToTable("roles");

            entity.HasIndex(e => e.Name, "roles_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Rolepermission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("rolepermissions_pkey");

            entity.ToTable("rolepermissions");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Permissionid).HasColumnName("permissionid");
            entity.Property(e => e.Roleid).HasColumnName("roleid");

            entity.HasOne(d => d.Permission).WithMany(p => p.Rolepermissions)
                .HasForeignKey(d => d.Permissionid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("rolepermissions_permissionid_fkey");

            entity.HasOne(d => d.Role).WithMany(p => p.Rolepermissions)
                .HasForeignKey(d => d.Roleid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("rolepermissions_roleid_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'")
                .HasColumnName("createdat");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .HasColumnName("email");
            entity.Property(e => e.Fullname)
                .HasMaxLength(150)
                .HasColumnName("fullname");
            entity.Property(e => e.Passwordhash)
                .HasMaxLength(255)
                .HasColumnName("passwordhash");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");

            modelBuilder.Entity<Userprofile>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("userprofiles_pkey");

                entity.ToTable("userprofiles");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");
                entity.Property(e => e.Address)
                    .HasMaxLength(255)
                    .HasColumnName("address");
                entity.Property(e => e.Birthdate).HasColumnName("birthdate");
                entity.Property(e => e.Gender)
                    .HasMaxLength(10)
                    .HasColumnName("gender");
                entity.Property(e => e.Phonenumber)
                    .HasMaxLength(50)
                    .HasColumnName("phonenumber");

                entity.HasOne(d => d.IdNavigation).WithOne(p => p.Userprofile)
                    .HasForeignKey<Userprofile>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("userprofiles_id_fkey");
                entity.Property(e => e.ProfilePictureUrl)
                    .HasMaxLength(255)
                    .HasColumnName("profile_picture_url");
                entity.Property(e => e.Gender)
                      .HasConversion<string>()
                      .HasMaxLength(10)
                      .IsUnicode(false)
                      .HasColumnName("gender");
            });

            modelBuilder.Entity<Userrole>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("userroles_pkey");

                entity.ToTable("userroles");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Roleid).HasColumnName("roleid");
                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.HasOne(d => d.Role).WithMany(p => p.Userroles)
                    .HasForeignKey(d => d.Roleid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("userroles_roleid_fkey");

                entity.HasOne(d => d.User).WithMany(p => p.Userroles)
                    .HasForeignKey(d => d.Userid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("userroles_userid_fkey");
            });

            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Student", Description = "Default role for students" },
                new Role { Id = 2, Name = "Moderator", Description = "Moderator role with limited management rights" },
                new Role { Id = 3, Name = "Admin", Description = "Administrator role with full permissions" }
            );

            OnModelCreatingPartial(modelBuilder);
        });
    }

    void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {

    }
}
