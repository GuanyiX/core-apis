using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using WebApplication1.Data;

namespace WebApplication1.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/users", async (AppDbContext db, User user) =>
        {
            db.Users.Add(user);
            await db.SaveChangesAsync();
            return Results.Created($"/users/{user.Id}", user);
        }).WithName("AddUser").WithOpenApi();

        app.MapPut("/users/{id}", async (AppDbContext db, int id, User updatedUser) =>
        {
            var user = await db.Users.FindAsync(id);
            if (user == null) return Results.NotFound();
            user.Username = updatedUser.Username;
            user.Email = updatedUser.Email;
            user.Password = updatedUser.Password;
            await db.SaveChangesAsync();
            return Results.Ok(user);
        }).WithName("UpdateUser").WithOpenApi();

        app.MapDelete("/users/{id}", async (AppDbContext db, int id) =>
        {
            var user = await db.Users.FindAsync(id);
            if (user == null) return Results.NotFound();
            db.Users.Remove(user);
            await db.SaveChangesAsync();
            return Results.NoContent();
        }).WithName("DeleteUser").WithOpenApi();

        app.MapGet("/users", async (AppDbContext db) =>
        {
            var users = await db.Users.ToListAsync();
            return Results.Ok(users);
        }).WithName("GetUsers").WithOpenApi();
    }
}

public class User
{
    public int Id { get; set; } // Primary key for EF Core
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
