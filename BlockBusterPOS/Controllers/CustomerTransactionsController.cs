using AutoMapper;
using BlockBusterPOS.Dto;
using BlockBusterPOS.Interfaces.Services;
using BlockBusterPOS.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace BlockBusterPOS.Controllers;

[ApiController]
[Produces("application/json")]
[Route("customerTransactions")]
public class CustomerTransactionsController : ControllerBase
{
    private readonly ITransactionService _transactionService;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateCustomerTransactionDto> _validator;

    public CustomerTransactionsController(ITransactionService service, IMapper mapper, IValidator<CreateCustomerTransactionDto> validator)
    {
        _transactionService = service;
        _mapper = mapper;
        _validator = validator;
    }

    [HttpGet("")]
    [OpenApiOperation(
        "ListCustomerTransaction",
        "List all customer transactions",
        "Gets a list of all customer transactions. If no transactions are found, an empty list will be returned with status code 200.")]
    [Produces(typeof(IReadOnlyCollection<CustomerTransactionDto>))]
    public ActionResult<IReadOnlyCollection<CustomerTransactionDto>> ListCustomerTransactions()
    {
        var transactions = _transactionService.ListCustomerTransactions();
        var transactionDtos = _mapper.Map<IReadOnlyCollection<CustomerTransactionDto>>(transactions);

        return Ok(transactionDtos);
    }

    [HttpGet("{customerId}")]
    [OpenApiOperation(
        "GetCustomerTransaction",
        "Get a customer transaction",
        "Gets the details of a customer transaction based on the customer ID. ")]
    [Produces(typeof(CustomerTransactionDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<CustomerTransactionDto> GetCustomerTransaction(int customerId)
    {
        CustomerTransactionModel? transaction = _transactionService.TryGetCustomerTransaction(customerId);

        if (transaction == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<CustomerTransactionDto>(transaction));
    }

    [HttpPost()]
    [OpenApiOperation(
        "CreateCustomerTransaction",
        "Create a new customer transaction",
        "Creates a new customer transaction. The request body must include the customer name, address, phone number, club membership status (true/false) and transaction items (DVDs and Blu-Rays)." +
        " If the custom ID already exists, it will update the customer's transactions.")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreationResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public IActionResult CreateCustomerTransaction([FromBody] CreateCustomerTransactionDto input)
    {
        if (!input.Rentals.Any(rental => rental.Count > 0))
        {
            return BadRequest("At least one movie must be rented.");
        }

        var validationResult = _validator.Validate(input); // Validate the input model using the provided validator.

        if (!validationResult.IsValid)
        {
            var errorMessages = validationResult.Errors;

            return BadRequest(errorMessages.Select(error => error.ErrorMessage));
        }

        CustomerTransactionModel transaction = _mapper.Map<CreateCustomerTransactionDto, CustomerTransactionModel>(input);

        _transactionService.CreateCustomerTransaction(transaction);

        var response = new CreationResponse
        {
            CustomerId = transaction.Customer.Id,
            Message = "Transaction created successfully."
        };

        return CreatedAtAction(nameof(GetCustomerTransaction), new { customerId = input.Customer.Id }, response);
    }
}
