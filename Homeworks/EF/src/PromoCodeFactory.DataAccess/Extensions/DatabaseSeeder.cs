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
            // Ensure database is created
            await context.Database.EnsureCreatedAsync();
            
            Console.WriteLine("Starting database seeding...");

            // Seed Roles first (no dependencies)
            if (!await context.Roles.AnyAsync())
            {
                await context.Roles.AddRangeAsync(FakeDataFactory.Roles);
                await context.SaveChangesAsync();
                Console.WriteLine($"Seeded {FakeDataFactory.Roles.Count()} roles");
            }

            // Seed Employees (depends on Roles)
            if (!await context.Employees.AnyAsync())
            {
                var roles = await context.Roles.ToListAsync();
                var employees = FakeDataFactory.Employees.ToList();
                
                // Update employee roles with actual role entities from database
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
                Console.WriteLine($"Seeded {employees.Count} employees");
            }

            // Seed Preferences (no dependencies)
            if (!await context.Preferences.AnyAsync())
            {
                await context.Preferences.AddRangeAsync(FakeDataFactory.Preferences);
                await context.SaveChangesAsync();
                Console.WriteLine($"Seeded {FakeDataFactory.Preferences.Count()} preferences");
            }

            // Seed Customers (no dependencies)
            if (!await context.Customers.AnyAsync())
            {
                await context.Customers.AddRangeAsync(FakeDataFactory.Customers);
                await context.SaveChangesAsync();
                Console.WriteLine($"Seeded {FakeDataFactory.Customers.Count()} customers");
            }

            // Seed CustomerPreferences (depends on Customers and Preferences)
            if (!await context.CustomerPreferences.AnyAsync())
            {
                var customers = await context.Customers.ToListAsync();
                var preferences = await context.Preferences.ToListAsync();
                var customerPreferences = FakeDataFactory.CustomerPreferences.ToList();

                // Update foreign key IDs
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
                Console.WriteLine($"Seeded {customerPreferences.Count} customer preferences");
            }

            // Seed PromoCodes (depends on Customers, Preferences, and Employees)
            if (!await context.PromoCodes.AnyAsync())
            {
                var customers = await context.Customers.ToListAsync();
                var preferences = await context.Preferences.ToListAsync();
                var employees = await context.Employees.ToListAsync();
                var promoCodes = FakeDataFactory.Promocodes.ToList();

                // Update foreign key references
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
                Console.WriteLine($"Seeded {promoCodes.Count} promo codes");
            }
            
            Console.WriteLine("Database seeding completed successfully!");
        }
    }
}
