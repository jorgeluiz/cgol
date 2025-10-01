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

    public GameOfLifeController(ILogger<GameOfLifeController> logger, IMapper mapper)
    {
        _logger = logger;
        _mapper = mapper;
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
    public ActionResult<string> Create([FromBody] BoardModel newBoard)
    {
        return Ok();
    }

    ///<summary>
    /// Get the current state of a board
    ///<summary>
    ///<remarks>
    /// Retrive the current state of the game based on its id
    ///</remarks>
    ///<param name="boardId">The unique identifier of the board</param>
    [HttpGet("{boardId}")]
    public ActionResult Get([FromQuery] string boardId)
    {
        return Ok();
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
    public ActionResult<BoardModel> GetNext([FromQuery] string boardId, int statesCount)
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
    public ActionResult<BoardModel> GoToFinal([FromQuery] string boardId)
    {
        return Ok();
    }

}
