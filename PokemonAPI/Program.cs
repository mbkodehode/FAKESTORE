//to get data from body,form 
using Microsoft.AspNetCore.Mvc;
//to get data from external API 
//fake person 
// using System.Net.Http;



var builder = WebApplication.CreateBuilder(args);

//add services 
//add cors 
builder.Services.AddCors(options =>
{
    options.AddPolicy("allowAll", policy =>
    {
        policy.AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyMethod();
    });
});
//add Http client 
builder.Services.AddHttpClient();

var app = builder.Build();
//apply services like cors and httpclient
app.UseCors("allowAll");
//we use static pages like html,css,js
app.UseStaticFiles();

//later we will change it to static files 
//this will be our home page
//we need to write a homepage 

//app.MapGet("/", () => "Hello World!");

//redirect to index.html  
app.MapGet("/", content =>
{
    content.Response.Redirect("/index.html");
    return Task.CompletedTask;

});

//company page get 
app.MapGet("/pokemon", content =>
{
    content.Response.Redirect("/table.html");
    return Task.CompletedTask;
});




//we create httpClientFactory instance 
// from body and single postman parameter 12,1 etc 
app.MapPost("/staff", async (IHttpClientFactory httpClientFactory, [FromBody] int personNumber) =>

{
    try
    {
        var client = httpClientFactory.CreateClient();
        //since we use async we use now await 

        object endpoint = 1;
        var response = await client.GetStringAsync($"https://pokeapi.co/api/v2/{endpoint}/");
        // one way to return json 
        return Results.Content(response, "application/json");
    }
    catch (Exception exp)
    {
        return Results.BadRequest(new { error = exp.Message });
    }

});

// we can also use body as json 
// from body and single postman parameter 12,1 etc 
app.MapPost("/staffjson", async (IHttpClientFactory httpClientFactory, [FromBody] PersonRequest personRequest) =>

{
    try
    {
        var client = httpClientFactory.CreateClient();
        //since we use async we use now await 

        var response = await client.GetStringAsync($"https://randomuser.me/api/?results={personRequest.PersonNumber}");
        // one way to return json 
        return Results.Content(response, "application/json");
    }
    catch (Exception exp)
    {
        return Results.BadRequest(new { error = exp.Message });
    }

});



app.Run();


public class PersonRequest
{
    public int PersonNumber { get; set; }
}
