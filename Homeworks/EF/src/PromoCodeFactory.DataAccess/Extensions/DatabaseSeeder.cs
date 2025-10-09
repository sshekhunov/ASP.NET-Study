using Microsoft.EntityFrameworkCore;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.DataAccess.Contexts;
using PromoCodeFactory.DataAccess.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PromoCodeFactory.DataAccess.Extensions
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(this PromoDbContext context)
        {
            await context.Database.EnsureCreatedAsync();           

            if (!await context.Roles.AnyAsync())
            {
                await context.Roles.AddRangeAsync(FakeDataFactory.Roles);
                await context.SaveChangesAsync();
            }

            if (!await context.Employees.AnyAsync())
            {
                var roles = await context.Roles.ToListAsync();
                var employees = FakeDataFactory.Employees.ToList();
                
                foreach (var employee in employees)
                {
                    if (employee.Role != null)
                    {
                        var role = roles.FirstOrDefault(r => r.Name == employee.Role.Name);
                        if (role != null)
                        {
                            employee.Role = role;
                        }
                    }
                }

                await context.Employees.AddRangeAsync(employees);
                await context.SaveChangesAsync();
            }

            if (!await context.Preferences.AnyAsync())
            {
                await context.Preferences.AddRangeAsync(FakeDataFactory.Preferences);
                await context.SaveChangesAsync();
            }

            if (!await context.Customers.AnyAsync())
            {
                await context.Customers.AddRangeAsync(FakeDataFactory.Customers);
                await context.SaveChangesAsync();
            }

            if (!await context.CustomerPreferences.AnyAsync())
            {
                var customers = await context.Customers.ToListAsync();
                var preferences = await context.Preferences.ToListAsync();
                var customerPreferences = FakeDataFactory.CustomerPreferences.ToList();

                foreach (var cp in customerPreferences)
                {
                    if (cp.Customer != null)
                    {
                        var customer = customers.FirstOrDefault(c => c.Id == cp.Customer.Id);
                        if (customer != null)
                        {
                            cp.CustomerId = customer.Id;
                            cp.Customer = customer;
                        }
                    }

                    if (cp.Preference != null)
                    {
                        var preference = preferences.FirstOrDefault(p => p.Id == cp.Preference.Id);
                        if (preference != null)
                        {
                            cp.PreferenceId = preference.Id;
                            cp.Preference = preference;
                        }
                    }
                }

                await context.CustomerPreferences.AddRangeAsync(customerPreferences);
                await context.SaveChangesAsync();
            }

            if (!await context.PromoCodes.AnyAsync())
            {
                var customers = await context.Customers.ToListAsync();
                var preferences = await context.Preferences.ToListAsync();
                var employees = await context.Employees.ToListAsync();
                var promoCodes = FakeDataFactory.Promocodes.ToList();

                foreach (var promoCode in promoCodes)
                {
                    if (promoCode.Customer != null)
                    {
                        var customer = customers.FirstOrDefault(c => c.Id == promoCode.Customer.Id);
                        if (customer != null)
                        {
                            promoCode.Customer = customer;
                        }
                    }

                    if (promoCode.Preference != null)
                    {
                        var preference = preferences.FirstOrDefault(p => p.Id == promoCode.Preference.Id);
                        if (preference != null)
                        {
                            promoCode.Preference = preference;
                        }
                    }

                    if (promoCode.PartnerManager != null)
                    {
                        var employee = employees.FirstOrDefault(e => e.Id == promoCode.PartnerManager.Id);
                        if (employee != null)
                        {
                            promoCode.PartnerManager = employee;
                        }
                    }
                }

                await context.PromoCodes.AddRangeAsync(promoCodes);
                await context.SaveChangesAsync();
            }
        }
    }
}
