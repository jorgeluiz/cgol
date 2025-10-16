using DL.GameOfLife.Models;
using FluentAssertions;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace DL.GameOfLife.Api.Tests;

/// <summary>
/// Contains integration tests for the GameOfLifeController.
/// These tests cover the entire application stack, from the HTTP request
/// down to the in-memory database, ensuring all components work together correctly.
/// </summary>
public class GameOfLifeControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public GameOfLifeControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    /// <summary>
    /// Tests the complete lifecycle of a game:
    /// 1. POST: Create a new game board.
    /// 2. GET: Retrieve the created game board.
    /// 3. DELETE: Remove the game board.
    /// 4. GET (Verify): Confirm the board is no longer accessible.
    /// </summary>
    [Fact(DisplayName = @"Given a valid board request, 
        when Create, Get, and Delete endpoints are called sequentially, 
        then it should successfully manage the board's lifecycle")]
    public async Task Create_Get_AndDelete_Should_Follow_Complete_Lifecycle()
    {
        // ARRANGE: Define the initial state of the board (a simple "blinker" pattern)
        var newBoardRequest = new BoardModelRequest
        {
            Cells = new List<BoardCellModel>
            {
                new() { RowNumber = 1, ColumnNumber = 1, IsAlive = false },
                new() { RowNumber = 1, ColumnNumber = 2, IsAlive = false },
                new() { RowNumber = 1, ColumnNumber = 3, IsAlive = false },
                new() { RowNumber = 2, ColumnNumber = 1, IsAlive = true },
                new() { RowNumber = 2, ColumnNumber = 2, IsAlive = true },
                new() { RowNumber = 2, ColumnNumber = 3, IsAlive = true },
                new() { RowNumber = 3, ColumnNumber = 1, IsAlive = false },
                new() { RowNumber = 3, ColumnNumber = 2, IsAlive = false },
                new() { RowNumber = 3, ColumnNumber = 3, IsAlive = false },
            }
        };

        // ACT 1: Create a new game by sending a POST request
        var createResponse = await _client.PostAsJsonAsync("/GameOfLife", newBoardRequest);

        // ASSERT 1: Verify the creation was successful
        createResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var createdBoard = await createResponse.Content.ReadFromJsonAsync<BoardModelResponse>();

        createdBoard.Should().NotBeNull();
        createdBoard.Id.Should().NotBeNullOrEmpty();
        createdBoard.Cells.Should().HaveCount(newBoardRequest.Cells.Count);

        var boardId = createdBoard.Id;

        // ACT 2: Retrieve the newly created game using a GET request
        var getResponse = await _client.GetAsync($"/GameOfLife/{boardId}");

        // ASSERT 2: Verify the retrieval was successful and data is correct
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var retrievedBoard = await getResponse.Content.ReadFromJsonAsync<BoardModelResponse>();

        retrievedBoard.Should().NotBeNull();
        retrievedBoard.Id.Should().Be(boardId);
        retrievedBoard.Cells.Should().HaveCount(newBoardRequest.Cells.Count);

        // ACT 3: Delete the game using a DELETE request
        var deleteResponse = await _client.DeleteAsync($"/GameOfLife/{boardId}");

        // ASSERT 3: Verify the deletion was successful
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var deleteCount = await deleteResponse.Content.ReadFromJsonAsync<long>();
        deleteCount.Should().Be(1);

        // ACT 4: Attempt to retrieve the deleted game again
        var getAfterDeleteResponse = await _client.GetAsync($"/GameOfLife/{boardId}");

        // ASSERT 4: Verify that the game is not found (returns BadRequest as per controller logic)
        getAfterDeleteResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    /// <summary>
    /// Tests the core game logic by advancing the board to the next state.
    /// </summary>
    [Fact(DisplayName = @"Given an existing board with a known pattern,
    when the NextState endpoint is called,
    then it should return the correct next generation of the board")]
    public async Task NextState_Should_ReturnCorrectNextGeneration()
    {
        // ARRANGE: Create a complete 5x5 board with the centered "blinker" pattern.
        // The pattern (vertical) is: (1, 2) V, (2, 2) V, (3, 2) V.
        var newBoardRequest = new BoardModelRequest
        {
            Cells = new List<BoardCellModel>
        {
            // Row 0 (All Dead)
            new() { RowNumber = 0, ColumnNumber = 0, IsAlive = false },
            new() { RowNumber = 0, ColumnNumber = 1, IsAlive = false },
            new() { RowNumber = 0, ColumnNumber = 2, IsAlive = false },
            new() { RowNumber = 0, ColumnNumber = 3, IsAlive = false },
            new() { RowNumber = 0, ColumnNumber = 4, IsAlive = false },
            
            // Row 1
            new() { RowNumber = 1, ColumnNumber = 0, IsAlive = false },
            new() { RowNumber = 1, ColumnNumber = 1, IsAlive = false },
            new() { RowNumber = 1, ColumnNumber = 2, IsAlive = true },  // Alive (V)
            new() { RowNumber = 1, ColumnNumber = 3, IsAlive = false },
            new() { RowNumber = 1, ColumnNumber = 4, IsAlive = false },

            // Row 2 (Center)
            new() { RowNumber = 2, ColumnNumber = 0, IsAlive = false },
            new() { RowNumber = 2, ColumnNumber = 1, IsAlive = false },
            new() { RowNumber = 2, ColumnNumber = 2, IsAlive = true },  // Alive (V)
            new() { RowNumber = 2, ColumnNumber = 3, IsAlive = false },
            new() { RowNumber = 2, ColumnNumber = 4, IsAlive = false },

            // Row 3
            new() { RowNumber = 3, ColumnNumber = 0, IsAlive = false },
            new() { RowNumber = 3, ColumnNumber = 1, IsAlive = false },
            new() { RowNumber = 3, ColumnNumber = 2, IsAlive = true },  // Alive (V)
            new() { RowNumber = 3, ColumnNumber = 3, IsAlive = false },
            new() { RowNumber = 3, ColumnNumber = 4, IsAlive = false },

            // Row 4 (All Dead)
            new() { RowNumber = 4, ColumnNumber = 0, IsAlive = false },
            new() { RowNumber = 4, ColumnNumber = 1, IsAlive = false },
            new() { RowNumber = 4, ColumnNumber = 2, IsAlive = false },
            new() { RowNumber = 4, ColumnNumber = 3, IsAlive = false },
            new() { RowNumber = 4, ColumnNumber = 4, IsAlive = false },
        }
        };

        var createResponse = await _client.PostAsJsonAsync("/GameOfLife", newBoardRequest);
        var createdBoard = await createResponse.Content.ReadFromJsonAsync<BoardModelResponse>();
        var boardId = createdBoard.Id;

        // ACT: Request the next state of the game
        var nextStateResponse = await _client.GetAsync($"/GameOfLife/next_state/{boardId}");

        // ASSERT: Check if the next state is correct (blinker rotated 90 degrees)
        nextStateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var nextStateBoard = await nextStateResponse.Content.ReadFromJsonAsync<BoardModelResponse>();

        // Expected Horizontal Pattern: (2, 1) V, (2, 2) V, (2, 3) V
        nextStateBoard.Should().NotBeNull();
        nextStateBoard.ParentId.Should().Be(boardId);

        // 1. Verify the 3 new ALIVE cells are correct:
        nextStateBoard.Cells.Should().Contain(c => c.RowNumber == 2 && c.ColumnNumber == 1 && c.IsAlive, "Cell (2, 1) must have been born.");
        nextStateBoard.Cells.Should().Contain(c => c.RowNumber == 2 && c.ColumnNumber == 2 && c.IsAlive, "The center cell (2, 2) must have survived.");
        nextStateBoard.Cells.Should().Contain(c => c.RowNumber == 2 && c.ColumnNumber == 3 && c.IsAlive, "Cell (2, 3) must have been born.");

        // 2. Verify the 2 cells that were ALIVE in the input and must DIE are now dead:
        nextStateBoard.Cells.Should().Contain(c => c.RowNumber == 1 && c.ColumnNumber == 2 && !c.IsAlive, "Cell (1, 2) must have died due to underpopulation.");
        nextStateBoard.Cells.Should().Contain(c => c.RowNumber == 3 && c.ColumnNumber == 2 && !c.IsAlive, "Cell (3, 2) must have died due to underpopulation.");
    }
}
