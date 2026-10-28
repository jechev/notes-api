using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApi.Data;
using MyApi.Models;

namespace MyApi.Controllers;

[ApiController]
[Route("notes")]
public class NotesController(AppDb db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Note>>> GetAll()
    {
        var notes = await db.Notes.OrderByDescending(n => n.Id).ToListAsync();
        return Ok(notes);
    }

    [HttpPost]
    public async Task<ActionResult<Note>> Create(NoteInput input)
    {
        if (string.IsNullOrWhiteSpace(input.Text))
            return BadRequest(new { error = "Text is required" });

        var note = new Note { Text = input.Text.Trim() };
        db.Notes.Add(note);
        await db.SaveChangesAsync();
        return Created($"/notes/{note.Id}", note);
    }
}