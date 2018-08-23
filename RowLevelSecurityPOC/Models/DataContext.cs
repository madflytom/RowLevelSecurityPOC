using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RowLevelSecurityPOC.Models
{
    public class DataContext : DbContext
    {
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Product> Products { get; set; }

        private SqlConnection connection;

        private Guid tenantId;

        public DataContext(DbContextOptions<DataContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            if (httpContextAccessor.HttpContext != null)
            {
                var tenant = (Tenant)httpContextAccessor.HttpContext.Items["TENANT"];
                tenantId = tenant.TenantId;
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Tenant>().ToTable("Tenant");
            modelBuilder.Entity<Product>().ToTable("Product");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = "Server=DESKTOP-IJL3G1J\\SQL2016EXPRESS;Database=ProductDB;Trusted_Connection=True;";
            connection = new SqlConnection(connectionString);
            connection.StateChange += Connection_StateChange;

            optionsBuilder.UseSqlServer(connection);

            base.OnConfiguring(optionsBuilder);
        }

        private void Connection_StateChange(object sender, System.Data.StateChangeEventArgs e)
        {
            if (e.CurrentState == ConnectionState.Open)
            {
                var cmd = connection.CreateCommand();
                cmd.CommandText = @"exec sp_set_session_context @key=N'TenantId', @value=@TenantId";
                cmd.Parameters.AddWithValue("@TenantId", tenantId);
                cmd.ExecuteNonQuery();
            }
            else if (e.CurrentState == ConnectionState.Closed)
	        {
	            connection.StateChange -= Connection_StateChange;
	        }
        }
    }
}
