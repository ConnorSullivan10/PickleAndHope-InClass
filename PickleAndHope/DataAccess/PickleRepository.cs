using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using PickleAndHope.Models;

namespace PickleAndHope.DataAccess
{
    public class PickleRepository
    {
        static List<Pickle> _pickles = new List<Pickle> { 
            new Pickle 
            { 
                Type = "Bread and Butter", 
                NumberInStock = 5,
                Id = 1
            }
        };

        const string ConnectionString = "Server=localhost; Database = PickleAndHope; Trusted_Connection = True;";

        public Pickle Add(Pickle pickle)
        {
            //pickle.Id = _pickles.Max(x => x.Id) + 1;
            //_pickles.Add(pickle);

            var sql = @"insert into Pickle(NumberInStock,Price,Size, Type)
                        output inserted.*
                        values(@NumberInStock, @Price, @Size, @Type)";

            using (var db = new SqlConnection(ConnectionString))
            {
                var result = db.QueryFirstOrDefault<Pickle>(sql, pickle);
                return result;

                //db.Open();

                //var cmd = db.CreateCommand();
                //cmd.CommandText = sql;

                //cmd.Parameters.AddWithValue("NumberInStock", pickle.NumberInStock);
                //cmd.Parameters.AddWithValue("Price", pickle.Price);
                //cmd.Parameters.AddWithValue("Size", pickle.Size);
                //cmd.Parameters.AddWithValue("Type", pickle.Type);

                //var reader = cmd.ExecuteReader();

                //if (reader.Read())
                //{
                //    var newPickle = MapReaderToPickle(reader);
                //    return newPickle;
                //}
                //return null;
            }

        }

        public void Remove(string type)
        {
            throw new NotImplementedException();
        }

        public Pickle Update(Pickle pickle)
        {
            //var pickleToUpdate = GetByType(pickle.Type);
            //pickleToUpdate.NumberInStock += pickle.NumberInStock;

            //return pickleToUpdate;

            var sql = @"update Pickle
                        set NumberInStock = NumberInStock + @NewStock
                        output inserted.*
                        where Id = @Id";

            using (var db = new SqlConnection(ConnectionString))
            {
                var parameters = new
                {
                    NewStock = pickle.NumberInStock,
                    Id = pickle.Id
                };

                return db.QueryFirstOrDefault<Pickle>(sql, parameters);

                //connection.Open();

                //var cmd = connection.CreateCommand();
                //cmd.CommandText = sql;

                //cmd.Parameters.AddWithValue("NewStock", pickle.NumberInStock);
                //cmd.Parameters.AddWithValue("Id", pickle.Id);

                //var reader = cmd.ExecuteReader();
                //if (reader.Read())
                //{
                //    var updatedPickle = MapReaderToPickle(reader);

                //    return updatedPickle;
                //}
                //return null;
            }


        }

        public Pickle GetByType (string typeOfPickle)
        {
            var query = @"select *
                                from Pickle
                                where Type = '@Type'";

            //Sql Connection
            using (var db = new SqlConnection(ConnectionString))
            {
                var parameters = new { Type = typeOfPickle };

                var pickle = db.QueryFirstOrDefault<Pickle>(query, parameters);

                return pickle;
            //        connection.Open();


            //        //Sql Command
            //        var cmd = connection.CreateCommand();
            //        cmd.CommandText = query;
            //        cmd.Parameters.AddWithValue("Type", typeOfPickle);
                    
            //        //execute the command
            //        var reader = cmd.ExecuteReader();

            //        if (reader.Read())
            //        {
            //        var pickle = MapReaderToPickle(reader);

            //            return pickle;
            //        }
            //        return null;
                }
            }

        public IEnumerable<Pickle> GetAll()
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                return db.Query<Pickle>("select * from pickle");
            }

            ////Sql Connection
            //var connection = new SqlConnection(ConnectionString);
            //connection.Open();

            ////Sql Command
            //var cmd = connection.CreateCommand();
            //cmd.CommandText = "select * from pickle";

            ////sql data reader
            //var reader = cmd.ExecuteReader();

            //var pickles = new List<Pickle>();

            ////Map results to c# things
            //while (reader.Read())
            //{
            //    var id = (int)reader["Id"];
            //    var type = (string)reader["Type"];
            //    var pickle = MapReaderToPickle(reader);

            //    pickles.Add(pickle);
            //}

            //connection.Close();

            //return pickles;

        }

        public Pickle GetById(int id)
        {
            //return _pickles.FirstOrDefault(pickle => pickle.Id == id);

            var query = @"select *
                          from Pickle
                          where id = @id";

            using (var db = new SqlConnection(ConnectionString))
            {
                // below is anonymous type for single use in this function
                var parameters = new { Id = id };

                var pickle = db.QueryFirstOrDefault<Pickle>(query, parameters);
                return pickle;

                //connection.Open();

                //var cmd = connection.CreateCommand();
                //var query = @"
                //            select *
                //            from Pickle
                //            where id = @id";

                //cmd.CommandText = query;
                //cmd.Parameters.AddWithValue("id", id);

                //var reader = cmd.ExecuteReader();

                //if (reader.Read())
                //{
                //    return MapReaderToPickle(reader);
                //}
                //return null;
            }
        }

        Pickle MapReaderToPickle(SqlDataReader reader)
        {
            var pickle = new Pickle
            {
                Id = (int)reader["Id"],
                Type = (string)reader["Type"],
                Price = (decimal)reader["Price"],
                NumberInStock = (int)reader["NumberInStock"],
                Size = (string)reader["Size"]
            };

            return pickle;
        }
    }
}
