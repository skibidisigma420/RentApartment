using RentApartments.Domain.Entities;
using RentApartments.Domain.Enums;
using RentApartments.Domain.ValueObjects;

namespace RentApartments.ConsoleTester
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                TestValueObjects();
                TestLandlord();
                TestApartment();
                TestTenant();
                TestRentRequest();
                TestRentalAgreement();

                Console.WriteLine("\nвсе тесты пройдены успешно.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nошибка при тестировании: {ex}");
            }
        }

        #region Test Data Helpers

        static Title CreateTestTitle() => new Title("КВАРТИРА В МОСКВЕ");
        static Description CreateTestDescription() => new Description("2 квадратных метра В ЦЕНТРЕ МОСКВЫ");
        static Address CreateTestAddress() => new Address("г. Самара, ул. Московская, д. 123");
        static Money CreateTestMoney(decimal value = 30000) => new Money(value);
        static Username CreateTestUsername() => new Username("bot123");
        #endregion

        static void TestValueObjects()
        {
            Console.WriteLine("\nтестируем Value Objects......");

            var title = CreateTestTitle();
            var description = CreateTestDescription();
            var address = CreateTestAddress();
            var money = CreateTestMoney();
            var username = CreateTestUsername();

            Console.WriteLine($" - Title: {title.Value}");
            Console.WriteLine($" - Description: {description.Value}");
            Console.WriteLine($" - Address: {address.Value}");
            Console.WriteLine($" - Money: {money.Value} р.");
            Console.WriteLine($" - Username: {username.Value}");

            // тестирование валидации
            try
            {
                var invalidMoney = new Money(-100);
                throw new Exception("money с отрицательным значением не должен был быть создан");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" - проверка money с отрицательным значением: {ex.Message}");
            }

            Console.WriteLine("Value Objects протестированы успешно");
        }

        static void TestLandlord()
        {
            Console.WriteLine("\nтестируем Landlord......");

            var landlordId = Guid.NewGuid();
            var landlord = new Landlord(landlordId, CreateTestUsername());

            Console.WriteLine($"создан Landlord: ID = {landlord.Id}, Username = {landlord.Username.Value}");

            // тестирование добавления квартиры
            var apartment = new Apartment(
                Guid.NewGuid(),
                CreateTestTitle(),
                CreateTestDescription(),
                CreateTestAddress(),
                CreateTestMoney(),
                landlord
            );

            landlord.AddApartment(apartment);
            Console.WriteLine($"(AddApartment) добавлена квартира: Title = {apartment.Title.Value}, Address = {apartment.Address.Value}");

            if (landlord.ActiveApartments.Count != 1)
                throw new Exception("квартира не была добавлена в ActiveApartments");

            // тестирование обновления описания квартиры
            var newDescription = "теперь в центре москвы но чуть левее";
            landlord.UpdateApartmentDetails(apartment, newDescription);
            Console.WriteLine($"(UpdateApartmentDetails) обновлено описание: {apartment.Description.Value}");

            // тестирование удаления квартиры
            if (!landlord.RemoveApartment(apartment))
                throw new Exception("(RemoveApartment) квартира не была удалена");

            if (landlord.ActiveApartments.Count != 0)
                throw new Exception("квартира не была удалена из ActiveApartments");

            Console.WriteLine("Landlord протестирован успешно");
        }

        static void TestApartment()
        {
            Console.WriteLine("\nтестируем Apartment......");

            var landlord = new Landlord(Guid.NewGuid(), CreateTestUsername());
            var apartment = new Apartment(
                Guid.NewGuid(),
                CreateTestTitle(),
                CreateTestDescription(),
                CreateTestAddress(),
                CreateTestMoney(35000),
                landlord
            );

            landlord.AddApartment(apartment);

            // тестирование изменения статуса
            apartment.SetUnavailable();
            Console.WriteLine($"(SetUnavailable) статус квартиры: {apartment.Status}");

            apartment.ChangeStatus(ApartmentStatus.Available);
            Console.WriteLine($"(ChangeStatus) новый статус: {apartment.Status}");

            // тестирование запросов на аренду
            var tenant = new Tenant(Guid.NewGuid(), CreateTestUsername());
            var request = new RentRequest(apartment, tenant, landlord);
            apartment.AddRentRequest(request);

            Console.WriteLine($"(AddRentRequest) заявка на аренду подана. последняя заявка: {apartment.LastRequest?.Status}");

            // тестирование аренды квартиры
            request.Approve();
            apartment.SetRented(tenant);
            Console.WriteLine($"(SetRented) статус после аренды: {apartment.Status}");

            // тестирование невалидных переходов статуса
            try
            {
                apartment.ChangeStatus(ApartmentStatus.Unavailable);
                throw new Exception("не должен был разрешить переход из Rented в Unavailable");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"(Invalid status change) ожидаемая ошибка: {ex.Message}");
            }

            Console.WriteLine("Apartment протестирован успешно");
        }

        static void TestTenant()
        {
            Console.WriteLine("\nтестируем Tenant...");

            var tenant = new Tenant(Guid.NewGuid(), CreateTestUsername());
            var landlord = new Landlord(Guid.NewGuid(), CreateTestUsername());
            var apartment = new Apartment(
                Guid.NewGuid(),
                CreateTestTitle(),
                CreateTestDescription(),
                CreateTestAddress(),
                CreateTestMoney(40000),
                landlord
            );

            landlord.AddApartment(apartment);

            // тестирование добавления квартиры в наблюдаемые
            tenant.AddObservableApartment(apartment);
            Console.WriteLine($"(AddObservableApartment) квартира добавлена в наблюдаемые: {apartment.Title.Value}");

            if (tenant.ObservableApartments.Count != 1)
                throw new Exception("квартира не была добавлена в ObservableApartments");

            // тестирование создания запроса на аренду
            if (!tenant.TryRentApartment(apartment))
                throw new Exception("(TryRentApartment) не удалось создать заявку на аренду");

            Console.WriteLine($"(TryRentApartment) заявка на аренду создана успешно");

            // тестирование изменения имени пользователя
            var newUsername = new Username("bot2000");
            tenant.ChangeUsername(newUsername);
            Console.WriteLine($"(ChangeUsername) новый username: {tenant.Username.Value}");

            Console.WriteLine("Tenant протестирован успешно");
        }

        static void TestRentRequest()
        {
            Console.WriteLine("\nтестируем RentRequest......");

            var landlord = new Landlord(Guid.NewGuid(), CreateTestUsername());
            var apartment = new Apartment(
                Guid.NewGuid(),
                CreateTestTitle(),
                CreateTestDescription(),
                CreateTestAddress(),
                CreateTestMoney(45000),
                landlord
            );

            landlord.AddApartment(apartment);

            var tenant = new Tenant(Guid.NewGuid(), CreateTestUsername());

            var request = new RentRequest(apartment, tenant, landlord, "привет рим");

            Console.WriteLine($"создан RentRequest:");
            Console.WriteLine($" - Apartment: {apartment.Title.Value}, Address: {apartment.Address.Value}");
            Console.WriteLine($" - Tenant: {tenant.Username.Value}");
            Console.WriteLine($" - Landlord: {landlord.Username.Value}");
            Console.WriteLine($" - Status: {request.Status}");
            Console.WriteLine($" - Message: {request.Message}");

            // тестирование одобрения запроса
            request.Approve();
            Console.WriteLine($"(Approve) статус после одобрения: {request.Status}");

            // тестирование повторного одобрения
            try
            {
                request.Approve();
                throw new Exception("не должен был разрешить повторное одобрение");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"(Double Approve) ожидаемая ошибка: {ex.Message}");
            }

            Console.WriteLine("RentRequest протестирован успешно");
        }

        static void TestRentalAgreement()
        {
            Console.WriteLine("\nтестируем RentalAgreement......");

            var landlord = new Landlord(Guid.NewGuid(), CreateTestUsername());
            var apartment = new Apartment(
                Guid.NewGuid(),
                CreateTestTitle(),
                CreateTestDescription(),
                CreateTestAddress(),
                CreateTestMoney(50000),
                landlord
            );

            landlord.AddApartment(apartment);

            var tenant = new Tenant(Guid.NewGuid(), CreateTestUsername());

            var startDate = new DateTime(2023, 1, 1);

            var endDate = DateTime.Now.AddDays(1);

            var agreement = new RentalAgreement(
                apartment,
                tenant,
                landlord,
                CreateTestMoney(50000),
                startDate,
                DateTime.Now
            );

            Console.WriteLine($"создан RentalAgreement:");
            Console.WriteLine($" - Apartment: {apartment.Title.Value}");
            Console.WriteLine($" - Tenant: {tenant.Username.Value}");
            Console.WriteLine($" - Landlord: {landlord.Username.Value}");
            Console.WriteLine($" - Price: {agreement.MonthlyRent.Value} р.");
            Console.WriteLine($" - Start Date: {agreement.StartDate}");
            Console.WriteLine($" - IsActive: {agreement.IsActive}");

            // тестирование прекращения договора
            agreement.Terminate(endDate);
            Console.WriteLine($"(Terminate) прекращено с валидной датой: {endDate}");
            Console.WriteLine($" - End Date после прекращения: {agreement.EndDate}");
            Console.WriteLine($"доступность после прекращения: {apartment.Status}");

            Console.WriteLine("RentalAgreement протестирован успешно");
        }
    }
}