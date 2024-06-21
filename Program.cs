var builder = WebApplication.CreateBuilder(args);
const string PieApiCorsPolicyName = "PIEAPICORSPolicy";

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: PieApiCorsPolicyName,
    policy =>
    {
        var prodSiteUrl = builder.Configuration.GetValue<string>("ClientSiteUrl");

        if (!string.IsNullOrWhiteSpace(prodSiteUrl))
        {
            policy.WithOrigins(prodSiteUrl).WithMethods("GET", "POST", "OPTIONS").WithHeaders("*");
        }        
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection();
}

app.UseCors(PieApiCorsPolicyName);

app.UseAuthorization();

app.MapControllers();

app.Run();
