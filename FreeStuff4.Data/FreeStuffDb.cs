using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace FreeStuff4.Data
{
    public class FreeStuffDb
    {
        private string _conStr;
        public FreeStuffDb(string conStr)
        {
            _conStr = conStr;
        }
        public List<Post> GetAllPosts()
        {
            using (SqlConnection conn = new SqlConnection(_conStr))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"SELECT * FROM Posts
                                    ORDER BY DateCreated DESC";
                List<Post> result = new List<Post>();
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Post p = new Post();
                    if (String.IsNullOrEmpty(reader["Name"].ToString()))
                    {
                        p.Name = null;
                    }
                    else
                    {
                        p.Name = (string)reader["Name"];
                    }
                    p.Id = (int)reader["Id"];
                    p.DateCreated = (DateTime)reader["DateCreated"];
                    p.PhoneNumber = (string)reader["PhoneNumber"];
                    p.Text = (string)reader["Text"];

                    result.Add(p);
                }
                return result;
            }
        }
        public void AddPost(Post p)
        {
            using (SqlConnection conn = new SqlConnection(_conStr))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"INSERT INTO Posts (DateCreated, Name, PhoneNumber, Text)
                                    VALUES (GETDATE(), @name, @number, @text)
                                    SELECT SCOPE_IDENTITY()";
                object name = p.Name;
                if (name == null)
                {
                    name = DBNull.Value;
                }
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@number", p.PhoneNumber);
                cmd.Parameters.AddWithValue("@text", p.Text);
                conn.Open();
                p.Id = (int)(decimal)cmd.ExecuteScalar();
            }
        }

        public void DeletePost(int id)
        {
            using (SqlConnection conn = new SqlConnection(_conStr))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"DELETE FROM Posts
                                    WHERE Id = @id";
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

    }
}
