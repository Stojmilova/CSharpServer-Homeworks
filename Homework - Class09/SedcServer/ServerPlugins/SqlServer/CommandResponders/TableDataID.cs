using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerInterfaces;

namespace ServerPlugins.SqlServer.CommandResponders
{
    public class TableDataID : ICommandResponder
    {
        public string ConnectionString { get; private set; }
        public string TableName { get; private set; }
        public int ID { get; private set; }

        public TableDataID(string connectionString, string tableName, int Id)
        {
            ConnectionString = connectionString;
            TableName = tableName;
            ID = Id;
        }
        public async Task<Response> GetResponse()
        {
            using (var cnn = new SqlConnection(ConnectionString))
            {
                cnn.Open();
                using (var command = new SqlCommand($@"SELECT * FROM {TableName} WHERE ID = {ID}", cnn))
                {
                    using (var dr = await command.ExecuteReaderAsync())
                    {
                        var schema = dr.GetColumnSchema();
                        var columnNames = schema.Select(cs => cs.ColumnName);
                        object[] values = new object[columnNames.Count()];

                        var results = new List<Dictionary<string, string>>();

                        while (dr.Read())
                        {
                            var columnCount = dr.GetValues(values);
                            var objectDictionary = new Dictionary<string, string>();
                            foreach (var column in schema)
                            {

                                objectDictionary.Add(column.ColumnName, dr.GetValue(column.ColumnOrdinal.Value).ToString());
                            }
                            results.Add(objectDictionary);
                        }

                        var body = SqlHelpers.GenerateJsonData(results);

                        return new Response
                        {
                            ContentType = ContentTypes.JsonApplication,
                            ResponseCode = ResponseCode.Ok,
                            Type = ResponseType.Text,
                            Body = body
                        };
                    }
                }
            }

        }
    }
}
