using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Mmcc.Stats.Core.Data.Models;

namespace Mmcc.Stats.Core.Data
{
    public partial class PollerContext : DbContext
    {
        public PollerContext()
        {
        }

        public PollerContext(DbContextOptions<PollerContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ClientToken> ClientTokens { get; set; }
        public virtual DbSet<Ping> Pings { get; set; }
        public virtual DbSet<Server> Servers { get; set; }
        public virtual DbSet<TpsStat> TpsStats { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClientToken>(entity =>
            {
                entity.ToTable("clienttokens");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ClientName)
                    .IsRequired()
                    .HasColumnName("clientname")
                    .HasColumnType("varchar(20)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasColumnName("value")
                    .HasColumnType("varchar(128)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");
            });

            modelBuilder.Entity<Ping>(entity =>
            {
                entity.ToTable("pings");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.HasIndex(e => e.ServerId)
                    .HasName("pings_server_serverId_fk");

                entity.Property(e => e.PingTime)
                    .HasColumnName("pingTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.PlayersMax)
                    .HasColumnName("playersMax")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PlayersOnline)
                    .HasColumnName("playersOnline")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ServerId)
                    .HasColumnName("serverId")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Server)
                    .WithMany()
                    .HasForeignKey(d => d.ServerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("pings_server_serverId_fk");
            });

            modelBuilder.Entity<Server>(entity =>
            {
                entity.ToTable("server");

                entity.Property(e => e.ServerId)
                    .HasColumnName("serverId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Enabled).HasColumnName("enabled");

                entity.Property(e => e.ServerIp)
                    .IsRequired()
                    .HasColumnName("serverIp")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.ServerName)
                    .HasColumnName("serverName")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.ServerPort)
                    .HasColumnName("serverPort")
                    .HasColumnType("int(16)");
            });

            modelBuilder.Entity<TpsStat>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tpsstats");

                entity.HasIndex(e => e.ServerId)
                    .HasName("tpsstats_server_serverId_fk");

                entity.Property(e => e.ServerId)
                    .HasColumnName("serverId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.StatTime)
                    .HasColumnName("statTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.Tps).HasColumnName("tps");

                entity.HasOne(d => d.Server)
                    .WithMany()
                    .HasForeignKey(d => d.ServerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tpsstats_server_serverId_fk");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
