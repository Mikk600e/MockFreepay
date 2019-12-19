using System.Data;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
namespace ProduktMock.Models
{
    public class MockPost
    {

        public int id {get; set;}
        public int CardNumber {get; set;}
        public bool isAccepted {get; set;}
        internal AppDB Db {get; set;}
        public MockPost()
        {
        }
        internal MockPost(AppDB db)
        {
            Db = db;
        }
        public async Task InsertAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `transaktion` (`CardNumber`, `IsAccepted`) VALUES (@CardNumber, @IsAccepted);";
            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
            id = (int) cmd.LastInsertedId;
        }
        public async Task UpdateAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE `transaktion` SET `CardNumber` = @CardNumber, `IsAccepted` = @IsAccepted WHERE `Id` = @id;";
            BindParams(cmd);
            BindID(cmd);
            await cmd.ExecuteNonQueryAsync();
        }
        public async Task DeleteAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `transaktion` WHERE `Id` = @id;";
            BindID(cmd);
            await cmd.ExecuteNonQueryAsync();
        }
        private void BindID(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
        }
        private void BindParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@cardnumber",
                DbType = DbType.Int32,
                Value = CardNumber,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName ="@isaccepted",
                DbType = DbType.Boolean,
                Value = isAccepted,
            });
        }
    }
}