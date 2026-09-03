using FormBuilder.Application.DTOs;
using FormBuilder.Application.Exceptions;
using FormBuilder.Application.Interfaces;
using FormBuilder.Application.Services;
using FormBuilder.Domain.Entities;
using FormBuilder.Domain.Enums;
using Moq;
using Xunit;

namespace FormBuilder.UnitTests;

public class FormTemplateServiceTests
{
    private readonly Mock<IFormTemplateRepository> _repositoryMock = new();
    private readonly FormTemplateService _sut;

    public FormTemplateServiceTests()
    {
        _sut = new FormTemplateService(_repositoryMock.Object);
    }

    private static CreateFormTemplateRequest ValidRequest() => new()
    {
        Name = "Vacation Request",
        CreatedBy = "hr-admin",
        Fields = new List<CreateFormFieldRequest>
        {
            new() { Label = "Employee name", Type = FieldType.Text, IsRequired = true },
            new() { Label = "Start date", Type = FieldType.Date, IsRequired = true }
        },
        ApprovalSteps = new List<CreateApprovalStepRequest>
        {
            new() { StepName = "Manager approval", ApproverIdentity = "manager@company.com", ActionType = ApprovalActionType.ApproveOrReject }
        }
    };

    [Fact]
    public async Task CreateAsync_WithValidRequest_PersistsFieldsAndStepsInOrder()
    {
        // Arrange
        FormTemplate? captured = null;
        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<FormTemplate>(), It.IsAny<CancellationToken>()))
            .Callback<FormTemplate, CancellationToken>((template, _) => captured = template)
            .ReturnsAsync((FormTemplate template, CancellationToken _) => template);

        // Act
        var result = await _sut.CreateAsync(ValidRequest());

        // Assert
        Assert.NotNull(captured);
        Assert.Equal("Vacation Request", captured!.Name);
        Assert.Equal(2, captured.Fields.Count);
        Assert.Single(captured.ApprovalSteps);
        Assert.Equal(1, captured.ApprovalSteps.Single().StepOrder);

        Assert.Equal("Vacation Request", result.Name);
        Assert.Equal(2, result.Fields.Count);
        Assert.Single(result.ApprovalSteps);
    }

    [Fact]
    public async Task CreateAsync_WithNoFields_ThrowsValidationException()
    {
        var request = ValidRequest();
        request.Fields.Clear();

        await Assert.ThrowsAsync<ValidationException>(() => _sut.CreateAsync(request));
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<FormTemplate>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WithNoApprovalSteps_ThrowsValidationException()
    {
        var request = ValidRequest();
        request.ApprovalSteps.Clear();

        await Assert.ThrowsAsync<ValidationException>(() => _sut.CreateAsync(request));
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<FormTemplate>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task GetByIdAsync_WhenTemplateDoesNotExist_ReturnsNull()
    {
        _repositoryMock
            .Setup(r => r.GetByIdAsync(42, It.IsAny<CancellationToken>()))
            .ReturnsAsync((FormTemplate?)null);

        var result = await _sut.GetByIdAsync(42);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsSummariesOrderedByMostRecentFirst()
    {
        var older = new FormTemplate("Sick Leave", "hr-admin");
        older.AddField("Days", FieldType.Number, true);
        older.AddApprovalStep("Manager approval", "manager@company.com", ApprovalActionType.Approve);

        await Task.Delay(1); // ensure a distinct, later CreatedAt for the second template

        var newer = new FormTemplate("Vacation Request", "hr-admin");
        newer.AddField("Start date", FieldType.Date, true);
        newer.AddApprovalStep("Manager approval", "manager@company.com", ApprovalActionType.ApproveOrReject);

        _repositoryMock
            .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<FormTemplate> { older, newer });

        var result = await _sut.GetAllAsync();

        Assert.Equal(2, result.Count);
        Assert.Equal("Vacation Request", result[0].Name);
        Assert.Equal("Sick Leave", result[1].Name);
    }
}
