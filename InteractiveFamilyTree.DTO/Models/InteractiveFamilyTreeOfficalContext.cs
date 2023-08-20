using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace InteractiveFamilyTree.DTO.Models;

public interface IInteractiveFamilyTreeOfficalContext
{
    public  DbSet<Career> Careers { get; set; }

    public  DbSet<ChildAndParentsRelationShip> ChildAndParentsRelationShips { get; set; }

    public  DbSet<CoupleRelationship> CoupleRelationships { get; set; }

    public  DbSet<EventParticipant> EventParticipants { get; set; }

    public  DbSet<FamilyEvent> FamilyEvents { get; set; }

    public  DbSet<FamilyMember> FamilyMembers { get; set; }

    public  DbSet<FamilyTree> FamilyTrees { get; set; }

    public  DbSet<Member> Members { get; set; }
}

public partial class InteractiveFamilyTreeOfficalContext : DbContext, IInteractiveFamilyTreeOfficalContext
{
    public InteractiveFamilyTreeOfficalContext()
    {
    }

    public InteractiveFamilyTreeOfficalContext(DbContextOptions<InteractiveFamilyTreeOfficalContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Career> Careers { get; set; }

    public virtual DbSet<ChildAndParentsRelationShip> ChildAndParentsRelationShips { get; set; }

    public virtual DbSet<CoupleRelationship> CoupleRelationships { get; set; }

    public virtual DbSet<EventParticipant> EventParticipants { get; set; }

    public virtual DbSet<FamilyEvent> FamilyEvents { get; set; }

    public virtual DbSet<FamilyMember> FamilyMembers { get; set; }

    public virtual DbSet<FamilyTree> FamilyTrees { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(GetConnectionString());
        }
    }

    protected string GetConnectionString()
    {
        IConfiguration config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", true, true)
        .Build();
        var strConn = config["ConnectionStrings:DefaultConnection"];

        return strConn;
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Career>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Career__3214EC07D9EC15AA");

            entity.ToTable("Career");

            entity.Property(e => e.Detail).HasMaxLength(100);
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.FamilyMemberId).HasColumnName("FamilyMemberID").IsRequired();
            entity.Property(e => e.StartDate).HasColumnType("datetime").IsRequired();
            entity.Property(e => e.Status).HasDefaultValueSql("((1))").IsRequired();

            entity.HasOne(d => d.FamilyMember).WithMany(p => p.Careers)
                .HasForeignKey(d => d.FamilyMemberId)
                .HasConstraintName("FK__Career__FamilyMe__3E52440B");
        });

        modelBuilder.Entity<ChildAndParentsRelationShip>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ChildAnd__3214EC07AD15E14A");

            entity.ToTable("ChildAndParentsRelationShip");

            entity.Property(e => e.ChildId).HasColumnName("ChildID").IsRequired();
            entity.Property(e => e.ParentId).HasColumnName("ParentID").IsRequired();

            entity.HasOne(d => d.Child).WithMany(p => p.ChildAndParentsRelationShipChildren)
                .HasForeignKey(d => d.ChildId)
                .HasConstraintName("FK__ChildAndP__Child__37A5467C");

            entity.HasOne(d => d.Parent).WithMany(p => p.ChildAndParentsRelationShipParents)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK__ChildAndP__Paren__36B12243")
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<CoupleRelationship>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CoupleRe__3214EC076A142B10");

            entity.ToTable("CoupleRelationship");

            entity.Property(e => e.CreateDate).HasColumnType("datetime").IsRequired();
            entity.Property(e => e.HusbandId).HasColumnName("HusbandID").IsRequired();
            entity.Property(e => e.MarrageStatus).HasMaxLength(20).IsRequired();
            entity.Property(e => e.UpdateDate).HasColumnType("datetime").IsRequired();
            entity.Property(e => e.WifeId).HasColumnName("WifeID").IsRequired();

            entity.HasOne(d => d.Husband).WithMany(p => p.CoupleRelationshipHusbands)
                .HasForeignKey(d => d.HusbandId)
                .HasConstraintName("FK__CoupleRel__Husba__3A81B327");

            entity.HasOne(d => d.Wife).WithMany(p => p.CoupleRelationshipWives)
                .HasForeignKey(d => d.WifeId)
                .HasConstraintName("FK__CoupleRel__WifeI__3B75D760")
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<EventParticipant>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EventPar__3214EC07E45F10D0");

            entity.ToTable("EventParticipant");

            entity.Property(e => e.FamilyMemberId).HasColumnName("FamilyMemberID").IsRequired();
            entity.Property(e => e.Status).IsRequired();

            entity.HasOne(d => d.Event).WithMany(p => p.EventParticipants)
                .HasForeignKey(d => d.EventId)
                .HasConstraintName("FK__EventPart__Event__46E78A0C");

            entity.HasOne(d => d.FamilyMember).WithMany(p => p.EventParticipants)
                .HasForeignKey(d => d.FamilyMemberId)
                .HasConstraintName("FK__EventPart__Famil__47DBAE45")
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<FamilyEvent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FamilyEv__3214EC07ECA7CF6C");

            entity.ToTable("FamilyEvent");

            entity.Property(e => e.Date).HasColumnType("datetime").IsRequired();
            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Status).HasDefaultValueSql("((1))").IsRequired();
            entity.Property(e => e.Type).HasDefaultValueSql("((1))").IsRequired();

            entity.HasOne(d => d.Tree).WithMany(p => p.FamilyEvents)
                .HasForeignKey(d => d.TreeId)
                .HasConstraintName("FK__FamilyEve__TreeI__4222D4EF")
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<FamilyMember>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FamilyMe__3214EC27E2E307B1");

            entity.ToTable("FamilyMember");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Birthday).HasColumnType("datetime").IsRequired();
            entity.Property(e => e.CreateDate).HasColumnType("datetime").IsRequired();
            entity.Property(e => e.FullName).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Gender).HasDefaultValueSql("((1))").IsRequired();
            entity.Property(e => e.MemberId).HasColumnName("MemberID");
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .HasDefaultValueSql("('member')");
            entity.Property(e => e.Status).HasDefaultValueSql("((1))").IsRequired();
            entity.Property(e => e.StatusHealth).HasDefaultValueSql("((1))").IsRequired();
            entity.Property(e => e.TreeId).HasColumnName("TreeID").IsRequired();
            entity.Property(e => e.UpdateDate).HasColumnType("datetime").IsRequired();

            entity.HasOne(d => d.Member).WithOne()
                .HasForeignKey<FamilyMember>(d => d.MemberId)
                .IsRequired(false)
                .HasConstraintName("FK__FamilyMem__Membe__2F10007B");

            entity.HasOne(d => d.Tree).WithMany(p => p.FamilyMembers)
                .HasForeignKey(d => d.TreeId)
                .HasConstraintName("FK__FamilyMem__TreeI__2E1BDC42")
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<FamilyTree>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FamilyTr__3214EC07BEE43752");

            entity.Property(e => e.CreateDate).HasColumnType("datetime").IsRequired();
            entity.Property(e => e.FirstName).HasMaxLength(100).IsRequired();
            entity.Property(e => e.ModifyDate).HasColumnType("datetime").IsRequired();
            entity.Property(e => e.Status).HasDefaultValueSql("((1))").IsRequired();
            entity.Property(e => e.ManagerId).HasDefaultValueSql("((0))");
            entity.Property(e => e.TotalGeneration).HasDefaultValueSql("((1))").IsRequired();

            entity.HasOne(d => d.Member).WithOne()
                .HasForeignKey<FamilyTree>(d => d.ManagerId)
                .HasConstraintName("FK__FamilyTre__Membe__2F10007B");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Members__3214EC07CF0AAB4E");

            entity.HasIndex(e => e.Email, "UQ__Members__A9D105348AD26F24").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Birthday).HasColumnType("datetime").IsRequired();
            entity.Property(e => e.Email).HasMaxLength(50).IsRequired();
            entity.Property(e => e.FullName).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Image).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValueSql("('1')");
            entity.Property(e => e.Phone).HasMaxLength(11).IsRequired();
            entity.Property(e => e.Status).HasDefaultValueSql("((1))").IsRequired();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
