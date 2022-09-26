using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dapper;

namespace W1.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class W1NameDapperController : ControllerBase
    {
        private readonly W1DapperContext dapperContext;
        private readonly ILogger<W1NameDapperController> logger;

        public W1NameDapperController(ILogger<W1NameDapperController> logger, W1DapperContext dapperContext)
        {
            this.logger = logger;
            this.dapperContext = dapperContext;
        }

        [HttpGet]
        public async Task<List<Shared.W1Name>> ReadAll()
        {
            var sql = @"SELECT [Id]
                          ,[Name]
                          ,[CreateDateTime]
                          ,[UpdateTime]
                      FROM [W1].[dbo].[W1_Name]";

            using var conn = dapperContext.CreateConnection();
            var names = (await conn.QueryAsync<Shared.W1Name>(sql)).ToList();
            return names ?? new List<Shared.W1Name>();
        }

        [HttpGet("{id:int}")]
        public async Task<Shared.W1Name?> ReadById([FromRoute] int id)
        {
            var sql = @"SELECT [Id]
                          ,[Name]
                          ,[CreateDateTime]
                          ,[UpdateTime]
                      FROM [W1].[dbo].[W1_Name]
                      WHERE [Id] = @id";

            using var conn = dapperContext.CreateConnection();
            var name = await conn.QueryAsync<Shared.W1Name>(sql, new { id });
            return name.SingleOrDefault();
        }

        [HttpPost]
        public async Task<bool> Create([FromBody] W1.Shared.W1Name name)
        {
            try
            {
                var sql = @"INSERT INTO [dbo].[W1_Name]
                                       ([Name]
                                       ,[CreateDateTime]
                                       ,[UpdateTime])
                                 VALUES
                                       (@Name
                                       ,@CreateDateTime
                                       ,@UpdateTime)
                            ";

                var param = new
                {
                    name.Name,
                    CreateDateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                };

                using var conn = dapperContext.CreateConnection();
                await conn.ExecuteAsync(sql, param);

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
                var sql = @"UPDATE [dbo].[W1_Name]
                               SET [Name] = @Name
                                  ,[UpdateTime] = @UpdateTime
                             WHERE [Id] = @Id
                            ";

                var param = new
                {
                    name.Id,
                    name.Name,
                    UpdateTime = DateTime.Now
                };

                using var conn = dapperContext.CreateConnection();
                await conn.ExecuteAsync(sql, param);

                return true;
            }
            catch (Exception ex)
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
                var sql = @"DELETE FROM [dbo].[W1_Name]
                              WHERE [Id] = @Id";

                var param = new
                {
                    Id = id
                };

                using var conn = dapperContext.CreateConnection();
                await conn.ExecuteAsync(sql, param);
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return false;
            }
        }
    }
}
