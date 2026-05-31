using Microsoft.Data.SqlClient;
using StudentTeacherAPI.Models;

namespace StudentTeacherAPI.DAL
{
    public class RecordDAL
    {
        private readonly string _connectionString;

        public RecordDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        // CREATE - Add new record
        public void CreateRecord(Record record)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            string query = "INSERT INTO Records (Title, Description, CreatedBy) VALUES (@Title, @Description, @CreatedBy)";
            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Title", record.Title);
            cmd.Parameters.AddWithValue("@Description", record.Description ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@CreatedBy", record.CreatedBy);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        // READ - Get all records
        public List<Record> GetAllRecords()
        {
            List<Record> records = new List<Record>();
            using SqlConnection conn = new SqlConnection(_connectionString);
            string query = "SELECT * FROM Records";
            using SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                records.Add(new Record
                {
                    RecordId = (int)reader["RecordId"],
                    Title = reader["Title"].ToString()!,
                    Description = reader["Description"].ToString(),
                    CreatedBy = (int)reader["CreatedBy"],
                    CreatedAt = (DateTime)reader["CreatedAt"]
                });
            }
            return records;
        }

        // READ - Get single record by ID
        public Record? GetRecordById(int id)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            string query = "SELECT * FROM Records WHERE RecordId = @RecordId";
            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@RecordId", id);
            conn.Open();
            using SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Record
                {
                    RecordId = (int)reader["RecordId"],
                    Title = reader["Title"].ToString()!,
                    Description = reader["Description"].ToString(),
                    CreatedBy = (int)reader["CreatedBy"],
                    CreatedAt = (DateTime)reader["CreatedAt"]
                };
            }
            return null;
        }

        // UPDATE - Update existing record
        public void UpdateRecord(Record record)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            string query = "UPDATE Records SET Title = @Title, Description = @Description WHERE RecordId = @RecordId";
            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Title", record.Title);
            cmd.Parameters.AddWithValue("@Description", record.Description ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@RecordId", record.RecordId);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        // DELETE - Remove a record
        public void DeleteRecord(int id)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            string query = "DELETE FROM Records WHERE RecordId = @RecordId";
            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@RecordId", id);
            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}