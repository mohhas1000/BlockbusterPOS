using BlockBusterPOS.Configuration;
using BlockBusterPOS.IntegrationTests.Client;
using BlockBusterPOS.IntegrationTests.Client.Exception;
using BlockBusterPOS.IntegrationTests.Utilities;
using BlockBusterPOS.Models;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace BlockBusterPOS.IntegrationTests.Tests;

[ExcludeFromCodeCoverage]
[TestClass]
public class CustomerTransactionsControllerTests : IntegrationTestBase
{
    private const string _transactionCreatedSuccessMessage = "Transaction created successfully.";
    private const string _expectedErrorMessageForNoMoviesRented = "At least one movie must be rented.";
    private const int NonCustomerId = 1;
    private const int FirstCustomerId = 11110;
    private const int SecondCustomerId = 21110;
    private const bool IsClubMember = true;
    private const int DVDCount = 3;
    private const int BluRayCount = 3;
    private const int NoMoviesRented = 0;
    private const decimal ExpectedRentalPrice = 166.3m;
    private const IReadOnlyDictionary<int, CustomerTransactionModel>? EmptyList = null;

    private readonly IReadOnlyDictionary<int, CustomerTransactionModel> expectedTransactionData = null!;
    private readonly IOptions<CustomerTransactionOptions> _customerTransactionOptionsFaker = 
        A.Fake<IOptions<CustomerTransactionOptions>>(x => x.Strict(StrictFakeOptions.AllowObjectMethods));

    private CustomerTransactionsClient _client = null!;
    private TestWebApplicationBuilder<Program> _builder = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _builder = CreateTestApplicationBuilder();
    }

    [TestMethod]
    [DynamicData(nameof(RentalPriceTestCases), DynamicDataSourceType.Method)]
    public async Task ListCustomerTransaction_WhenDataExists_ReturnsTransactionsWithExpectedRentalPriceAsync(bool isClubMember, int DVDCount, int bluRayCount, decimal expectedRentalPrice)
    {
        IReadOnlyDictionary<int, CustomerTransactionModel> expectedTransactionData = CreateSampleTransactionData(FirstCustomerId, isClubMember, DVDCount, bluRayCount);

        var customerTransactionOptions = new CustomerTransactionOptions
        {
            TestData = expectedTransactionData,
        };

        A.CallTo(() => _customerTransactionOptionsFaker.Value).Returns(customerTransactionOptions);
        
        _builder.WithTestServices(serviceCollection => serviceCollection.AddSingleton(_customerTransactionOptionsFaker));

        _client = new(_builder.Build());

        ICollection<CustomerTransactionDto> result = await _client.ListCustomerTransactionAsync().ConfigureAwait(false);

        Assert.IsNotNull(result);

        AssertTransactionAreEqual(expectedTransactionData.First().Value, result.First(), expectedRentalPrice);
    }

    [TestMethod]
    public async Task ListCustomerTransaction_WhenDataNotExists_ReturnsNoCustomerTransactionsAsync()
    {
        var customerTransactionOptions = new CustomerTransactionOptions
        {
            TestData = EmptyList,
        };

        A.CallTo(() => _customerTransactionOptionsFaker.Value).Returns(customerTransactionOptions);

        _builder.WithTestServices(serviceCollection => serviceCollection.AddSingleton(_customerTransactionOptionsFaker));

        _client = new(_builder.Build());

        ICollection<CustomerTransactionDto> result = await _client.ListCustomerTransactionAsync().ConfigureAwait(false);

        Assert.IsTrue(result.Count == 0);
    }

    [TestMethod]
    public async Task GetCustomerTransaction_WhenValidId_ReturnsExpectedTransactionAsync()
    {
        IReadOnlyDictionary<int, CustomerTransactionModel> expectedTransactionData = CreateSampleTransactionData(FirstCustomerId, IsClubMember, DVDCount, BluRayCount);

        var customerTransactionOptions = new CustomerTransactionOptions
        {
            TestData = expectedTransactionData,
        };

        A.CallTo(() => _customerTransactionOptionsFaker.Value).Returns(customerTransactionOptions);

        _builder.WithTestServices(serviceCollection => serviceCollection.AddSingleton(_customerTransactionOptionsFaker));

        _client = new(_builder.Build());

        CustomerTransactionDto result = await _client.GetCustomerTransactionAsync(FirstCustomerId).ConfigureAwait(false);

        Assert.IsNotNull(result, "The result should not be null.");

        AssertTransactionAreEqual(expectedTransactionData.First().Value, result, ExpectedRentalPrice);
    }

    [TestMethod]
    public async Task GetCustomerTransaction_WhenIdNotAssociated_ReturnsNotFoundAsync()
    {
        IReadOnlyDictionary<int, CustomerTransactionModel> expectedTransactionData = CreateSampleTransactionData(FirstCustomerId, IsClubMember, DVDCount, BluRayCount);

        var customerTransactionOptions = new CustomerTransactionOptions
        {
            TestData = expectedTransactionData,
        };

        A.CallTo(() => _customerTransactionOptionsFaker.Value).Returns(customerTransactionOptions);

        _builder.WithTestServices(serviceCollection => serviceCollection.AddSingleton(_customerTransactionOptionsFaker));

        _client = new(_builder.Build());

        var response = await _client.GetCustomerTransactionAsync(NonCustomerId).ConfigureAwait(false);

        Assert.IsNull(response, "Response should be null if the customer is not found.");
    }


    [TestMethod]
    public async Task CreateCustomerTransaction_WhenNoMovieRented_ClientThrowsExceptionAsync()
    {
        CreateCustomerTransactionDto input = CreateSampleCustomerTransactionDto(SecondCustomerId, IsClubMember, NoMoviesRented, NoMoviesRented);

        var customerTransactionOptions = new CustomerTransactionOptions
        {
            TestData = EmptyList,
        };

        A.CallTo(() => _customerTransactionOptionsFaker.Value).Returns(customerTransactionOptions);

        _builder.WithTestServices(serviceCollection => serviceCollection.AddSingleton(_customerTransactionOptionsFaker));

        _client = new(_builder.Build());

        var exception = await Assert.ThrowsExceptionAsync<HttpResponseException<string>>(() => _client.CreateCustomerTransactionAsync(input));

        Assert.AreEqual((int)HttpStatusCode.BadRequest, exception.StatusCode);
        Assert.AreEqual(_expectedErrorMessageForNoMoviesRented, exception.ExceptionResult);
    }

    [TestMethod]
    public async Task CreateCustomerTransaction_WhenValidInput_TransactionCreatedAsync()
    {
        CreateCustomerTransactionDto input = CreateSampleCustomerTransactionDto(SecondCustomerId, IsClubMember, DVDCount, BluRayCount);

        CustomerTransactionOptions customerTransactionOptions = new CustomerTransactionOptions
        {
            TestData = EmptyList,
        };

        A.CallTo(() => _customerTransactionOptionsFaker.Value).Returns(customerTransactionOptions);

        _builder.WithTestServices(serviceCollection => serviceCollection.AddSingleton(_customerTransactionOptionsFaker));

        _client = new(_builder.Build());

        Client.CreationResponse response = await _client.CreateCustomerTransactionAsync(input).ConfigureAwait(false);

        _transactionCreatedSuccessMessage.Should().Be(response.Message);
        SecondCustomerId.Should().Be(response.CustomerId);
    }

    [TestMethod]
    public async Task CreateCustomerTransaction_WhenValidInputAndCustomerExist_UpdatesRentalsAsync()
    {
        IReadOnlyDictionary<int, CustomerTransactionModel> expectedTransactionData = CreateSampleTransactionData(FirstCustomerId, IsClubMember, DVDCount, BluRayCount);
        CreateCustomerTransactionDto input = CreateSampleCustomerTransactionDto(FirstCustomerId, IsClubMember, DVDCount, BluRayCount);

        var customerTransactionOptions = new CustomerTransactionOptions
        {
            TestData = expectedTransactionData,
        };

        A.CallTo(() => _customerTransactionOptionsFaker.Value).Returns(customerTransactionOptions);

        _builder.WithTestServices(serviceCollection => serviceCollection.AddSingleton(_customerTransactionOptionsFaker));

        _client = new(_builder.Build());

        List<RentalModelDto> initialRentals = MapRentalsToDto(expectedTransactionData.First().Value.Rentals);

        Client.CreationResponse response = await _client.CreateCustomerTransactionAsync(input).ConfigureAwait(false);

        _transactionCreatedSuccessMessage.Should().Be(response.Message);
        FirstCustomerId.Should().Be(response.CustomerId);

        var updatedTransaction = await _client.GetCustomerTransactionAsync(FirstCustomerId).ConfigureAwait(false);

        var expectedRentals = initialRentals.Concat(MapRentalsToDto(input.Rentals)).ToList();
        var actualRentals = updatedTransaction.Rentals.ToList();

        CompareRentals(expectedRentals, actualRentals);
    }

    private static IEnumerable<object[]> RentalPriceTestCases()
    {
        // club member status, number of rented DVDs, number of rented Blurays, expected rental price. 

        yield return new object[] { true, 2, 2, 100m };       // Member, 2 DVD, 2 Blu-ray, expected rental price: 100
        yield return new object[] { false, 1, 1, 68m };       // Non-member, 1 DVD, 1 Blu-Ray, expected rental price 68
        yield return new object[] { true, 5, 6, 325m };       // Member, 5 DVD, 6 Blu-Ray, expected rental price: 225
        yield return new object[] { false, 2, 3, 175m };      // Non-member, 2 DVD, 3 Blu-Ray, expected rental price 175
        yield return new object[] { true, 2, 3, 133.15m };    // Member, 2 DVD, 3 Blu-Ray, expected rental price: 133.15       
        yield return new object[] { false, 0, 5, 195m };      // Non-member, 0 DVD, 5 Blu-Ray, expected rental price: 194
        yield return new object[] { true, 0, 5, 133.15m };    // Member, 0 DVD, 5 Blu-Ray, expected rental price: 133.15
        yield return new object[] { false, 4, 0, 116m };      // Non-member, 4 DVD, 0 Blu-Ray, expected rental price 116
        yield return new object[] { true, 0, 4, 100m };       // Member, 0 DVD, 4 Blu-Ray, expected rental price: 100
        yield return new object[] { true, 4, 0, 100m };       // Member, 4 DVD, 0 Blu-Ray, expected rental price: 100
        yield return new object[] { false, 3, 3, 204m };      // Non-member, 3 DVD and 3 Blu-Ray, expected rental price: 204
        yield return new object[] { true, 3, 3, 166.30m };    // Member, 3 DVD and 3 Blu-Ray, expected rental price: 166.30    
    }

    private static TestWebApplicationBuilder<Program> CreateTestApplicationBuilder()
    {
        return new TestWebApplicationBuilder<Program>(JsonConfigFile)
            .WithPreserveExecutionContext(true);
    }
}