using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PizzaStoreApi.Data;
using PizzaStoreApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<PizzaStoreDb>(options => options.UseInMemoryDatabase("items"));

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Pizza Store API",
        Description = "Making the pizzas you love",
        Version = "v1"
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "PizzaStore API V1");
    });
}

// Get - All pizzas
app.MapGet("/pizza", async (PizzaStoreDb db) => await db.Pizzas.ToListAsync());

// Get - Pizza by id
app.MapGet("/pizza/{id}", async (PizzaStoreDb db, int id) => await db.Pizzas.FindAsync(id)).WithName("GetPizzaById");

// Post - Create pizza
app.MapPost("/pizza", async (
    PizzaStoreDb db,
    Pizza pizza) =>
{
    await db.AddAsync(pizza);
    await db.SaveChangesAsync();
    return Results.Created($"/pizza/{pizza.Id}", pizza);
});

// Put - Update pizza by id
app.MapPut("/pizza/{id}", async (
    PizzaStoreDb db,
    Pizza updatedPizza,
    int id) =>
{
    var pizza = await db.Pizzas.FindAsync(id);
    if (pizza is null) return Results.NotFound();

    pizza.Name = updatedPizza.Name;
    pizza.Description = updatedPizza.Description;
    await db.SaveChangesAsync();

    return Results.NoContent();
});

// Delete - Delete pizza by id
app.MapDelete("/pizza/{id}", async (
    PizzaStoreDb db,
    int id) =>
{
    var pizza = await db.Pizzas.FindAsync(id);
    if (pizza is null) return Results.NotFound();

    db.Pizzas.Remove(pizza);
    await db.SaveChangesAsync();
    return Results.Ok();
});

//app.UseHttpsRedirection();

app.Run();
