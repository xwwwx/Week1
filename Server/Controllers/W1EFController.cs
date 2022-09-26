using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using W1.Server.Models;

namespace W1.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class W1NameEFController : ControllerBase
{
    private readonly ILogger<W1NameEFController> logger;
    private readonly W1DbContext dbContext;

    public W1NameEFController(ILogger<W1NameEFController> logger, W1DbContext dbContext)
    {
        this.logger = logger;
        this.dbContext = dbContext;
    }

    [HttpGet]
    public Task<List<Shared.W1Name>> ReadAll()
    {
        return dbContext.Names.Select(n => new Shared.W1Name() { Id = n.Id, Name = n.Name}).ToListAsync();
    }

    [HttpGet("{id:int}")]
    public Task<Shared.W1Name?> ReadById([FromRoute] int id)
    {
        return dbContext.Names.Select(n => new Shared.W1Name() { Id = n.Id, Name = n.Name }).FirstOrDefaultAsync(n => n.Id == id);
    }

    [HttpPost]
    public async Task<bool> Create([FromBody] W1.Shared.W1Name name)
    {
        try
        {
            await dbContext.Names.AddAsync(new W1Name()
            {
                Name = name.Name,
                CreateDateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            });

            await dbContext.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return false;
        }
    }

    [HttpPut]
    public async Task<bool> Update([FromBody] W1.Shared.W1Name name)
    {
        try
        {
            var oriName = await dbContext.Names.SingleAsync(n => n.Id == name.Id);

            oriName.Name = name.Name;
            oriName.UpdateTime = DateTime.Now;

            await dbContext.SaveChangesAsync();

            return true;
        }
        catch(Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return false;
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<bool> Delete([FromRoute] int id)
    {
        try
        {
            var delName = await dbContext.Names.SingleAsync(n => n.Id == id);

            dbContext.Names.Remove(delName);

            await dbContext.SaveChangesAsync();

            return true;
        }
        catch(Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return false;
        }
    }
}
