using System;
using System.Collections.Generic;
using System.Linq;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace PromoCodeFactory.DataAccess.Data
{
    public static class FakeDataFactory
    {
        public static IEnumerable<Employee> Employees => new List<Employee>()
        {
            new Employee()
            {
                Id = Guid.Parse("451533d5-d8d5-4a11-9c7b-eb9f14e1a32f"),
                Email = "owner@somemail.ru",
                FirstName = "Иван",
                LastName = "Сергеев",
                Role = Roles.FirstOrDefault(x => x.Name == "Admin"),
                AppliedPromocodesCount = 5
            },
            new Employee()
            {
                Id = Guid.Parse("f766e2bf-340a-46ea-bff3-f1700b435895"),
                Email = "andreev@somemail.ru",
                FirstName = "Петр",
                LastName = "Андреев",
                Role = Roles.FirstOrDefault(x => x.Name == "PartnerManager"),
                AppliedPromocodesCount = 10
            },
        };

        public static IEnumerable<Role> Roles => new List<Role>()
        {
            new Role()
            {
                Id = Guid.Parse("53729686-a368-4eeb-8bfa-cc69b6050d02"),
                Name = "Admin",
                Description = "Администратор",
            },
            new Role()
            {
                Id = Guid.Parse("b0ae7aac-5493-45cd-ad16-87426a5e7665"),
                Name = "PartnerManager",
                Description = "Партнерский менеджер"
            }
        };

        public static IEnumerable<Preference> Preferences => new List<Preference>()
        {
            new Preference()
            {
                Id = Guid.Parse("ef7f299f-92d7-459f-896e-078ed53ef99c"),
                Name = "Театр",
            },
            new Preference()
            {
                Id = Guid.Parse("c4bda62e-fc74-4256-a956-4760b3858cbd"),
                Name = "Семья",
            },
            new Preference()
            {
                Id = Guid.Parse("76324c47-68d2-472d-abb8-33cfa8cc0c84"),
                Name = "Дети",
            }
        };

        public static IEnumerable<Customer> Customers
        {
            get
            {
                var customerId = Guid.Parse("a6c8c6b1-4349-45b0-ab31-244740aaf0f0");
                var customers = new List<Customer>()
                {
                    new Customer()
                    {
                        Id = customerId,
                        Email = "ivan_sergeev@mail.ru",
                        FirstName = "Иван",
                        LastName = "Петров"
                    }
                };

                return customers;
            }
        }

        public static IEnumerable<PromoCode> Promocodes
        {
            get
            {
                var promocodes = new List<PromoCode>()
                {
                    new PromoCode()
                    {
                        Id = Guid.Parse("d1f1c8e2-5f4e-4c3b-9a7e-1c2b3d4e5f60"),
                        Code = "PROMO123",
                        BeginDate = DateTime.UtcNow.AddDays(-10),
                        EndDate = DateTime.UtcNow.AddDays(20),
                        PartnerManager = Employees.FirstOrDefault(e => e.Id == Guid.Parse("f766e2bf-340a-46ea-bff3-f1700b435895")),
                        PartnerName = "Партнер А",
                        Preference = Preferences.FirstOrDefault(p => p.Id == Guid.Parse("ef7f299f-92d7-459f-896e-078ed53ef99c")),
                        ServiceInfo = "Скидка 10% на первый заказ",
                        Customer = Customers.FirstOrDefault(c => c.Id == Guid.Parse("a6c8c6b1-4349-45b0-ab31-244740aaf0f0"))
                    }
                }; 
                
                return promocodes;
            }
        }

        public static IEnumerable<CustomerPreference> CustomerPreferences
        {
            get
            {
                var promocodes = new List<CustomerPreference>()
                {
                    new CustomerPreference()
                    {
                        Id = Guid.Parse("b1e2c3d4-e5f6-7a8b-9c0d-e1f2a3b4c5d6"),
                        Preference = Preferences.FirstOrDefault(p => p.Id == Guid.Parse("ef7f299f-92d7-459f-896e-078ed53ef99c")),
                        Customer = Customers.FirstOrDefault(c => c.Id == Guid.Parse("a6c8c6b1-4349-45b0-ab31-244740aaf0f0"))
                    },
                    new CustomerPreference()
                    {
                        Id = Guid.Parse("c2d3e4f5-6a7b-8c9d-0e1f-2a3b4c5d6e7f"),
                        Preference = Preferences.FirstOrDefault(p => p.Id == Guid.Parse("c4bda62e-fc74-4256-a956-4760b3858cbd")),
                        Customer = Customers.FirstOrDefault(c => c.Id == Guid.Parse("a6c8c6b1-4349-45b0-ab31-244740aaf0f0"))
                    }
                };

                return promocodes;
            }
        }
    }
}