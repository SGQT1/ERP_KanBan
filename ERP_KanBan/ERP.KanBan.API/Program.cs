var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

// 百分比改为纯数字，去掉%符号，字段名保持英文
app.MapGet("/kanban/OrdersStock/OrdersStockStyle", () =>
{
    return Results.Ok(new[]
    {
        new {
            modelName = "MR530V1(D)(PU)-SG",
            orderNum = 15,
            orderRatio = 6.00,
            orderPair = 30186,
            pairRatio = 11.33,
            stockPair = 27174,
            stockRatio = 7.57
        },
        new {
            modelName = "MR530V1(D)(20S2-1)-EMA",
            orderNum = 11,
            orderRatio = 4.00,
            orderPair = 25146,
            pairRatio = 9.44,
            stockPair = 24756,
            stockRatio = 6.89
        },
        new {
            modelName = "GR740V1(M)(25Q1-1)-BM",
            orderNum = 5,
            orderRatio = 2.00,
            orderPair = 0,
            pairRatio = 0.00,
            stockPair = 24558,
            stockRatio = 6.84
        },
        new {
            modelName = "U740V1(D)(C2C專案-41)-96I",
            orderNum = 11,
            orderRatio = 4.00,
            orderPair = 19104,
            pairRatio = 7.17,
            stockPair = 19104,
            stockRatio = 5.32
        },
        new {
            modelName = "MR530V1(D)(23S2-4)-CK",
            orderNum = 4,
            orderRatio = 1.00,
            orderPair = 0,
            pairRatio = 0.00,
            stockPair = 19065,
            stockRatio = 5.31
        }
    });
});

app.Run();