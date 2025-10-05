using System.Net;
using AutoMapper;
using DL.GameOfLife.Api.Models;
using DL.GameOfLife.Domain.Entities;
using DL.GameOfLife.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
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
    [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BoardModel newBoard)
    {
        try
        {
            var board = _mapper.Map<Board>(newBoard);
            await _service.NewGame(board);
            return Ok(board);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error trying to create a new game");
        }
        return BadRequest("Error trying to create a new game");

    }

    ///<summary>
    /// Get the current state of a board
    ///<summary>
    ///<remarks>
    /// Retrive the current state of the game based on its id
    ///</remarks>
    ///<param name="boardId">The unique identifier of the board</param>
    [HttpGet("{boardId}")]
    public async Task<IActionResult> Get(string boardId)
    {
        try
        {
            var board = await _service.LoadGame(boardId);
            return Ok(board);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error trying to load the game");
        }
        return BadRequest("Invalid boardId");
    }

    ///<summary>
    /// Move to generation
    ///<summary>
    ///<remarks>
    /// Advance to a future state based on the desired number
    ///</remarks>
    ///<param name="boardId">The unique identifier of the board.</param>
    ///<param name="statesCount">How many states you want to advance based on the current board</param>
    [ProducesResponseType(typeof(BoardModel), (int)HttpStatusCode.OK)]
    [HttpGet("move_to_generation/{boardId}/{statesCount}")]
    public ActionResult<BoardModel> GetNext(string boardId, int statesCount)
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
    public ActionResult<BoardModel> GoToFinal(string boardId)
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
    [HttpDelete("{boardId}")]
    public async Task<IActionResult> Clear(string boardId)
    {
        try
        {
            var totalDeleted = await _service.EndGame(boardId);
            if (totalDeleted > 0)

            {
                return Ok();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error trying to load the game");
        }
        return BadRequest("Invalid boarId");
    }

}
