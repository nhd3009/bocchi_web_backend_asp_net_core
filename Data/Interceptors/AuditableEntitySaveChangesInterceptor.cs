using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using bocchiwebbackend.Models.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace bocchiwebbackend.Data.Interceptors
{
    public class AuditableEntitySaveChangesInterceptor : SaveChangesInterceptor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuditableEntitySaveChangesInterceptor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            UpdateEntities(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            UpdateEntities(eventData.Context);
            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void UpdateEntities(DbContext context)
        {
            if (context == null) return;

            var username = GetCurrentUsername();
            var now = DateTime.UtcNow;

            foreach (var entry in context.ChangeTracker.Entries<AuditableEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = now;
                    entry.Entity.CreatedBy = username;
                    entry.Entity.UpdatedAt = now;
                    entry.Entity.UpdatedBy = username;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = now;
                    entry.Entity.UpdatedBy = username;
                }
            }
        }

        private string GetCurrentUsername()
        {
            try
            {
                var user = _httpContextAccessor.HttpContext?.User;
                if (user?.Identity?.IsAuthenticated == true)
                {
                    return user.FindFirst(ClaimTypes.Name)?.Value ?? "SYSTEM";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return "SYSTEM";
        }
    }
}