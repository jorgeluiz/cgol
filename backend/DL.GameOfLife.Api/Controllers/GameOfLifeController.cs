using System.Net;
using AutoMapper;
using DL.GameOfLife.Models;
using DL.GameOfLife.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using DL.GameOfLife.Api.ErrorHandling;
namespace DL.GameOfLife.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class GameOfLifeController : ControllerBase
{

    private readonly ILogger<GameOfLifeController> _logger;
    private readonly IMapper _mapper;
    private readonly IGameOfLifeService _service;

    public GameOfLifeController(ILogger<GameOfLifeController> logger, IMapper mapper, IGameOfLifeService service)
    {
        _logger = logger;
        _mapper = mapper;
        _service = service;
    }

    ///<summary>
    /// Create a board
    ///<summary>
    ///<remarks>
    /// Create the first state of the game and returns the id of the board
    ///</remarks>
    ///<param name="newBoard">A object that represents the first state of the game</param>
    /// <response> code="200">The game was successfully created</response>
    /// <response> code="400">The game that was send to be created is invalid</response>
    /// <response> code="500">Internal error</response>
    [HttpPost]
    [ProducesResponseType(typeof(BoardModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] BoardModel newBoard)
    {
        var result = await _service.NewGame(newBoard);

        if (result.IsSuccess)
        {
            return Ok(result.Model);
        }

        return BadRequest(result.ErrorResponse<BoardModel>());

    }

    ///<summary>
    /// Get the current state of a board
    ///<summary>
    ///<remarks>
    /// Retrive the current state of the game based on its id
    ///</remarks>
    ///<param name="boardId">The unique identifier of the board</param>
    /// <response> code="200">The game that was stored in the server</response>
    /// <response> code="400">The game that was send to be loaded is invalid</response>
    /// <response> code="500">Internal error</response>
    [HttpGet("{boardId}")]
    [ProducesResponseType(typeof(BoardModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get(string boardId)
    {
        var result = await _service.LoadGame(boardId);

        if (result.IsSuccess)
        {
            return Ok(result.Model);
        }

        return BadRequest(result.ErrorResponse<BoardModel>());
    }

    ///<summary>
    /// Move to generation
    ///<summary>
    ///<remarks>
    /// Advance to a future state based on the desired number
    ///</remarks>
    ///<param name="boardId">The unique identifier of the board.</param>
    ///<param name="statesCount">How many states you want to advance based on the current board</param>
    [HttpGet("move_to_generation/{boardId}/{statesCount}")]
    [ProducesResponseType(typeof(BoardModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public ActionResult<IActionResult> GetNext(string boardId, int statesCount)
    {
        return Ok();
    }

    ///<summary>
    /// Final move
    ///<summary>
    ///<remarks>
    /// Advance to the last state based on the game
    ///</remarks>
    ///<param name="boardId">The unique identifier of the board.</param>
    [HttpGet("final/{boardId}")]
    [ProducesResponseType(typeof(BoardModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public ActionResult<IActionResult> GoToFinal(string boardId)
    {
        return Ok();
    }

    ///<summary>
    /// End a game
    ///<summary>
    ///<remarks>
    /// Delete all data related to a game, including the board and its cells
    ///</remarks>
    ///<param name="boardId">The unique identifier of the board</param>
    /// <response> code="200">The total amount of games that were ended in this request</response>
    /// <response> code="400">The game that was send to be ended is invalid</response>
    /// <response> code="500">Internal error</response>
    [HttpDelete("{boardId}")]
    [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Clear(string boardId)
    {
        var result = await _service.EndGame(boardId);

        if (result.IsSuccess)
        {
            return Ok(result.Model);
        }

        return BadRequest(result.ErrorResponse<long>());
    }

}
