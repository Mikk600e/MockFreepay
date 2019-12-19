using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ProduktMock.Models
{
    public class MockPostQuery
    {
        public AppDB Db {get;}
        public MockPostQuery(AppDB db)
        {
            Db = db;
        }
        public async Task<MockPost> FindOneAsync(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `Id`, `CardNumber`, `IsAccepted` FROM `transaktion` WHERE `Id` = @id";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }
        public async Task<List<MockPost>> LatestPostAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText =  @"SELECT `Id`, `CardNumber`, `IsAccepted` FROM `transaktion` ORDER BY `Id` DESC LIMIT 10;";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }
        public async Task DeleteAllSync()
        {
            using var txn = await Db.Connection.BeginTransactionAsync();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "@DELETE FROM `transaktion`";
            await cmd.ExecuteNonQueryAsync();
            await txn.CommitAsync();
        }

        private async Task<List<MockPost>> ReadAllAsync(DbDataReader reader)
        {
            var posts = new List<MockPost>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new MockPost(Db)
                    {
                        id = reader.GetInt32(0),
                        CardNumber = reader.GetInt32(1),
                        isAccepted = reader.GetBoolean(2),
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }
    }
}