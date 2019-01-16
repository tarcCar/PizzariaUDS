using MySql.Data.MySqlClient;
using System.Data;

namespace PizzariaUDS.Repositories.Interfaces
{
    public interface IRepository
    {
        IDbConnection Database { get; }
    }
}