using FormBuilder.Application.DTOs;
using FormBuilder.Application.Exceptions;
using FormBuilder.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FormBuilder.Api.Controllers;

/// <summary>
/// Organizational form templates: creating a form (with its dynamic fields and its
/// dynamic approval route) and reading it back.
/// </summary>
[ApiController]
[Route("api/form-templates")]
public class FormTemplatesController : ControllerBase
{
    private readonly IFormTemplateService _formTemplateService;

    public FormTemplatesController(IFormTemplateService formTemplateService)
    {
        _formTemplateService = formTemplateService;
    }

    /// <summary>Saves a new form template in its entirety: envelope, fields and approval route.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(FormTemplateDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<FormTemplateDto>> Create(
        [FromBody] CreateFormTemplateRequest request,
        CancellationToken cancellationToken)
    {
        var created = await _formTemplateService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Returns a lightweight list of all existing form templates.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<FormTemplateSummaryDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<FormTemplateSummaryDto>>> GetAll(CancellationToken cancellationToken)
    {
        var templates = await _formTemplateService.GetAllAsync(cancellationToken);
        return Ok(templates);
    }

    /// <summary>Returns one form template, including its fields and approval route.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(FormTemplateDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FormTemplateDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var template = await _formTemplateService.GetByIdAsync(id, cancellationToken)
            ?? throw NotFoundException.ForEntity("FormTemplate", id);

        return Ok(template);
    }
}
