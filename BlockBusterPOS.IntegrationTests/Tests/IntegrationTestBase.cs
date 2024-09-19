using BlockBusterPOS.IntegrationTests.Client;
using BlockBusterPOS.Models;
using FluentAssertions;
using KellermanSoftware.CompareNetObjects;
using System.Collections.ObjectModel;

namespace BlockBusterPOS.IntegrationTests.Tests;

public abstract class IntegrationTestBase
{
    protected static string JsonConfigFile => "IntegrationsSettings.json";

    protected static void AssertTransactionAreEqual(CustomerTransactionModel expectedTransaction, CustomerTransactionDto actualTransaction, decimal expectedRentalPrice)
    {
        var expectedTransactionDto = MapModelToCustomerTransactionDto(expectedTransaction);

        expectedTransactionDto.ShouldCompare(actualTransaction, compareConfig: new ComparisonConfig
        {
            MembersToIgnore = new List<string>() { "TotalRentalPrice" }
        });

        expectedRentalPrice.Should().Be(actualTransaction.TotalRentalPrice);
    }

    protected static void CompareRentals(List<RentalModelDto> expectedRentals, List<RentalModelDto> actualRentals)
    {
        actualRentals.ShouldCompare(
            expectedRentals,
            compareConfig: new ComparisonConfig
            {
                SkipInvalidIndexers = true,
                IgnoreCollectionOrder = true,
                MembersToIgnore = ["RentalDate"]
            });
    }

    protected static CustomerTransactionDto MapModelToCustomerTransactionDto(CustomerTransactionModel model)
    {
        return new CustomerTransactionDto
        {
            Customer = new CustomerModelDto
            {
                Id = model.Customer.Id,
                Name = model.Customer.Name,
                PhoneNumber = model.Customer.PhoneNumber,
                IsClubMember = model.Customer.IsClubMember
            },
            Rentals = model.Rentals.Select(rental => new RentalModelDto
            {
                Type = (RentalType)rental.Type,
                Count = rental.Count
            }).ToList()
        };
    }

    protected static ReadOnlyDictionary<int, CustomerTransactionModel> CreateSampleTransactionData(int cutomerId, bool isClubMember, int dVDCount, int bluRayCount)
    {
        return new Dictionary<int, CustomerTransactionModel>()
        {
            {
                cutomerId,
                new CustomerTransactionModel()
                {
                    Customer = new Models.CustomerModel()
                    {
                        Id = cutomerId,
                        Name = "John",
                        PhoneNumber = "123",
                        IsClubMember = isClubMember
                    },
                    Rentals =
                    [
                         new RentalModel()
                         {
                             Type = Enum.RentalType.DVD,
                             Count = dVDCount
                         },
                         new RentalModel()
                         {
                             Type = Enum.RentalType.BluRay,
                             Count = bluRayCount
                         }
                    ]
                }
            }
        }.AsReadOnly(); ;
    }

    protected static CreateCustomerTransactionDto CreateSampleCustomerTransactionDto(int cutomerId, bool isClubMember, int dVDCount, int bluRayCount)
    {
        return new CreateCustomerTransactionDto
        {
            Customer = new Client.CustomerModel
            {
                Id = cutomerId,
                Name = "John",
                PhoneNumber = "123",
                IsClubMember = isClubMember
            },
            Rentals =
            [
                new RentalModelDtoBasic()
                {
                    Type = RentalType.DVD,
                    Count = dVDCount
                },
                new RentalModelDtoBasic()
                {
                    Type = RentalType.BluRay,
                    Count = bluRayCount
                }
            ]
        };
    }

    protected static List<RentalModelDto> MapRentalsToDto(ICollection<RentalModelDtoBasic> rentalDtos)
    {
        return rentalDtos.Select(dto => new RentalModelDto
        {
            Type = dto.Type,
            Count = dto.Count,
            RentalDate = DateTime.UtcNow 
        }).ToList();
    }

    protected static List<RentalModelDto> MapRentalsToDto(IReadOnlyCollection<RentalModel> rentals)
    {
        return rentals.Select(rental => new RentalModelDto
        {
            Type = (RentalType)rental.Type, 
            Count = rental.Count,
            RentalDate = DateTime.UtcNow 
        }).ToList();
    }
}

