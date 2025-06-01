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

                Console.WriteLine("\nпопыт прошел успешно.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nошибка тестирования: {ex}");
            }
        }

        #region Test Data Helpers

        static Title CreateTestTitle() => new Title("квартира 2 квадратных метра в центре москвы");
        static Description CreateTestDescription() => new Description("из развлечений есть микроволновка");
        static Address CreateTestAddress() => new Address("самара, ул. Московская, д. 1");
        static Money CreateTestMoney(decimal value = 1000) => new Money(value);
        static Username CreateTestUsername() => new Username("bot123");
        #endregion

        static void TestValueObjects()
        {
            Console.WriteLine("\nтест value objects.......");

            var title = CreateTestTitle();
            var description = CreateTestDescription();
            var address = CreateTestAddress();
            var money = CreateTestMoney();
            var username = CreateTestUsername();

            Console.WriteLine($" - title: {title.Value}");
            Console.WriteLine($" - description: {description.Value}");
            Console.WriteLine($" - address: {address.Value}");
            Console.WriteLine($" - money: {money.Value} р.");
            Console.WriteLine($" - username: {username.Value}");

            Console.WriteLine("обекты созданы");
        }


        static void TestLandlord()
        {
            Console.WriteLine("\nтестим landlord.......");

            var landlordId = Guid.NewGuid();
            var landlord = new Landlord(landlordId, CreateTestUsername());

            Console.WriteLine($"созданный landlord: ID = {landlord.Id}, username = {landlord.Username.Value}");

            var apartment = new Apartment(
                Guid.NewGuid(),
                CreateTestTitle(),
                CreateTestDescription(),
                CreateTestAddress(),
                CreateTestMoney(),
                landlord
            );

            landlord.AddApartment(apartment);
            Console.WriteLine($"(addApartment) добавленная квартира: title = {apartment.Title.Value}, address = {apartment.Address.Value}");

            if (landlord.ActiveApartments.Count != 1)
                throw new Exception("квартира не добавлена");

            if (!landlord.RemoveApartment(apartment))
                throw new Exception("(removeApartment) объявление не снято");

            Console.WriteLine("landlord протестирован");
        }


        static void TestApartment()
        {
            Console.WriteLine("\nтестируем apartment.......");

            var landlord = new Landlord(Guid.NewGuid(), CreateTestUsername());
            var apartment = new Apartment(
                Guid.NewGuid(),
                CreateTestTitle(),
                CreateTestDescription(),
                CreateTestAddress(),
                CreateTestMoney(1500),
                landlord
            );

            landlord.AddApartment(apartment);

            apartment.SetUnavailable();
            Console.WriteLine($"(setUnavailable) статус квартиры: {apartment.Status}");

            apartment.ChangeStatus(ApartmentStatus.Available);
            Console.WriteLine($"(changeStatus) новый статус свободно: {apartment.Status}");

            var tenant = new Tenant(Guid.NewGuid(), CreateTestUsername());
            var request = new RentRequest(apartment, tenant, landlord);
            apartment.AddRentRequest(request);

            Console.WriteLine($"заявка на аренду подана. последняя заявка: {apartment.LastRequest.Status}");

            Console.WriteLine("тестировние apartment завершено");
        }


        static void TestTenant()
        {
            Console.WriteLine("\nтестируем tenant.......");

            var tenant = new Tenant(Guid.NewGuid(), CreateTestUsername());
            var landlord = new Landlord(Guid.NewGuid(), CreateTestUsername());
            var apartment = new Apartment(
                Guid.NewGuid(),
                CreateTestTitle(),
                CreateTestDescription(),
                CreateTestAddress(),
                CreateTestMoney(),
                landlord
            );

            landlord.AddApartment(apartment);

            tenant.AddObservableApartment(apartment);
            Console.WriteLine($"просматриваемая квартира: {apartment.Title.Value}");

            if (!tenant.TryRentApartment(apartment))
                throw new Exception("не удалось создать заявку на аренду");

            Console.WriteLine("тестирование tenant завершено");
        }


        static void TestRentRequest()
        {
            Console.WriteLine("\nтестируем rentRequest.......");

            var landlord = new Landlord(Guid.NewGuid(), CreateTestUsername());
            var apartment = new Apartment(
                Guid.NewGuid(),
                CreateTestTitle(),
                CreateTestDescription(),
                CreateTestAddress(),
                CreateTestMoney(1200),
                landlord
            );

            landlord.AddApartment(apartment);

            var tenant = new Tenant(Guid.NewGuid(), CreateTestUsername());

            var request = new RentRequest(apartment, tenant, landlord);

            Console.WriteLine($"заявка подана:");
            Console.WriteLine($" - apartment: {apartment.Title.Value}, address: {apartment.Address.Value}");
            Console.WriteLine($" - tenant: {tenant.Username.Value}");
            Console.WriteLine($" - landlord: {landlord.Username.Value}");
            Console.WriteLine($" - status: {request.Status}");

            request.Approve();

            Console.WriteLine($"(approve()) статус после одобрения: {request.Status}");

            Console.WriteLine("тестирование rentRequest завершено");
        }


        static void TestRentalAgreement()
        {
            Console.WriteLine("\nтестируем rentalAgreement.......");

            var landlord = new Landlord(Guid.NewGuid(), CreateTestUsername());
            var apartment = new Apartment(
                Guid.NewGuid(),
                CreateTestTitle(),
                CreateTestDescription(),
                CreateTestAddress(),
                CreateTestMoney(1500),
                landlord
            );

            landlord.AddApartment(apartment);

            var tenant = new Tenant(Guid.NewGuid(), CreateTestUsername());

            var startDate = DateTime.Now.AddDays(1);
            var endDate = startDate.AddMonths(1);

            var agreement = new RentalAgreement(
                apartment,
                tenant,
                landlord,
                CreateTestMoney(1500),
                startDate,
                DateTime.Now 
            );

            Console.WriteLine($"договор аренды создан:");
            Console.WriteLine($" - apartment: {apartment.Title.Value}");
            Console.WriteLine($" - tenant: {tenant.Username.Value}");
            Console.WriteLine($" - landlord: {landlord.Username.Value}");
            Console.WriteLine($" - price: ${agreement.MonthlyRent.Value}");
            Console.WriteLine($" - start date: {agreement.StartDate}");
            Console.WriteLine($" - end date: {agreement.EndDate}");
            Console.WriteLine($" - isActive: {agreement.IsActive}");

            agreement.Terminate(endDate);
            Console.WriteLine($"(terminate()) прекращено с валидной датой: {endDate}");
            Console.WriteLine($"вызов isActive после прекращения: {agreement.IsActive}");

            var pastDate = DateTime.Now.AddDays(-1);

            try
            {
                agreement.Terminate(pastDate);
                Console.WriteLine($"(terminate()) прекращено с невалидной датой: {pastDate}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ошибка при попытке terminate() с невалидной датой: {ex.Message}");
            }

            Console.WriteLine($"вызов isActive после попытки невалидного прекращения: {agreement.IsActive}");

            Console.WriteLine("тестирование rentalAgreement завершено");
        }

    }
}