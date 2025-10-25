using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using FluentValidation;
using ELKOOD.ToDo.Core.Entities;
using ELKOOD.ToDo.Application.Interfaces;
using ELKOOD.ToDo.Application.DTOs.ToDo;

namespace ELKOOD.ToDo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ToDoController : ControllerBase
    {
        private readonly IToDoService _toDoService;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateToDoItemDto> _createValidator;
        private readonly IValidator<UpdateToDoItemDto> _updateValidator;

        public ToDoController(
            IToDoService toDoService, 
            IMapper mapper,
            IValidator<CreateToDoItemDto> createValidator,
            IValidator<UpdateToDoItemDto> updateValidator)
        {
            _toDoService = toDoService;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _toDoService.GetAllToDoItemsAsync();
            var result = _mapper.Map<IEnumerable<ToDoItemDto>>(items);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _toDoService.GetToDoItemByIdAsync(id);
            if (item == null)
                return NotFound();

            var result = _mapper.Map<ToDoItemDto>(item);
            return Ok(result);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(int userId)
        {
            var items = await _toDoService.GetToDoItemsByUserAsync(userId);
            var result = _mapper.Map<IEnumerable<ToDoItemDto>>(items);
            return Ok(result);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetFiltered(
            [FromQuery] int? userId,
            [FromQuery] string? category,
            [FromQuery] string? priority,
            [FromQuery] bool? isCompleted)
        {
            var items = await _toDoService.GetFilteredToDoItemsAsync(userId, category, priority, isCompleted);
            var result = _mapper.Map<IEnumerable<ToDoItemDto>>(items);
            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return BadRequest("Search term is required");

            var items = await _toDoService.SearchToDoItemsAsync(term);
            var result = _mapper.Map<IEnumerable<ToDoItemDto>>(items);
            return Ok(result);
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var pagedResult = await _toDoService.GetPagedToDoItemsAsync(pageNumber, pageSize);
            var result = new
            {
                Items = _mapper.Map<IEnumerable<ToDoItemDto>>(pagedResult.Items),
                pagedResult.TotalCount,
                pagedResult.PageNumber,
                pagedResult.PageSize,
                pagedResult.TotalPages,
                pagedResult.HasPrevious,
                pagedResult.HasNext
            };

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateToDoItemDto createDto)
        {
            var validationResult = await _createValidator.ValidateAsync(createDto);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var toDoItem = _mapper.Map<ToDoItem>(createDto);
            var createdItem = await _toDoService.CreateToDoItemAsync(toDoItem);
            
            var result = _mapper.Map<ToDoItemDto>(createdItem);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateToDoItemDto updateDto)
        {
            var validationResult = await _updateValidator.ValidateAsync(updateDto);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var existingItem = await _toDoService.GetToDoItemByIdAsync(id);
            if (existingItem == null)
                return NotFound();

            _mapper.Map(updateDto, existingItem);
            await _toDoService.UpdateToDoItemAsync(existingItem);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _toDoService.DeleteToDoItemAsync(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPatch("{id}/complete")]
        public async Task<IActionResult> MarkAsCompleted(int id)
        {
            try
            {
                await _toDoService.MarkAsCompletedAsync(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}