// DESKTOP-OH4KKOB\SQLEXPRESS
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.SqlClient;
using System.Text;
DbInitializer initializer = new DbInitializer();

/*initializer.AddData("Milk", "Anatoli", 3);
initializer.AddData("Tesla", "Daniel Petkov", 2);
initializer.AddData("Winter Tyres", "Simeon", 3);*/
initializer.QueryMethod();



public class DbInitializer
{
    private SqlConnection conn;
    public void CreateIfNotExistsDb()
    {
        conn=new SqlConnection(@"Server=DESKTOP-OH4KKOB\SQLEXPRESS; Database={0}; Trusted_Connection=True; TrustServerCertificate = True");
        conn.Open();
        string newstr = "SELECT database_id FROM sys.databases WHERE Name = 'myDataBase'";
        SqlCommand cmd1 = new SqlCommand(newstr, conn);

        if (cmd1.ExecuteScalar() != null)
        {
            string cmd = "CREATE DATABASE myDataBase";
            SqlCommand comm = new SqlCommand(cmd, conn);
            comm.ExecuteNonQuery();
        }
        conn.Close();
    }
    public void CreateColumns()
    {
        string colcomand1 = @"
            create table Products( +
            pr_Id int PRIMARY KEY IDENTITY (1,1),
            pr_name varchar(20) 
            )
            
            create table Buyers( +
            b_Id int PRIMARY KEY IDENTITY (1,1),
            b_name varchar(20)
            
            create table Sales(
            s_id int PRIMARY KEY IDENTITY (1,1),
            pr_Id int FOREIGN KEY REFERENCES Products(pr_Id),
            b_Id int FOREIGN KEY REFERENCES Buyers(b_Id)
            )";
        conn = new SqlConnection(@"Server=DESKTOP-OH4KKOB\SQLEXPRESS; Database=myDataBase; Trusted_Connection=True; TrustServerCertificate = True");
        conn.Open();
        SqlCommand comm1 = new SqlCommand(colcomand1, conn);
        comm1.ExecuteNonQuery();
        conn.Close();
    }


    public  void AddData(string productName, string buyerName, int fk)
    {
        conn = new SqlConnection(@"Server=DESKTOP-OH4KKOB\SQLEXPRESS; Database=myDataBase; Trusted_Connection=True; TrustServerCertificate = True");
        conn.Open();
        using (SqlCommand cmd = new SqlCommand(@"INSERT INTO Products(pr_name) VALUES(@productName)", conn))
        {
            cmd.Parameters.Add("@productName", SqlDbType.VarChar).Value = productName;
            cmd.ExecuteNonQuery();
        }
        using (SqlCommand cmd1 = new SqlCommand(@"INSERT INTO Buyers(b_name) VALUES (@buyerName)", conn))
        {
            cmd1.Parameters.Add("@buyerName", SqlDbType.VarChar).Value = buyerName;
            cmd1.ExecuteNonQuery();
        }
        using (SqlCommand cmd2 = new SqlCommand(@"INSERT INTO Sales(pr_Id, b_Id) VALUES(@pr_Id, @b_Id)", conn))
        {
            cmd2.Parameters.Add("@pr_Id", SqlDbType.Int).Value = fk;
            cmd2.Parameters.Add("@b_Id", SqlDbType.Int).Value = fk;
            cmd2.ExecuteNonQuery();
        }
        conn.Close();
        // Possible updated version of this code is with the use of the AddWithValue method instead of just Add method.
    }


    public void QueryMethod()
    {
        string query = @"SELECT p.pr_name AS [ProductName], COUNT(b_id) AS salesCount 
                from Sales AS s 
                JOIN Products AS p ON s.pr_Id = p.pr_Id
                GROUP BY p.pr_name";
        conn = new SqlConnection(@"Server=DESKTOP-OH4KKOB\SQLEXPRESS; Database=myDataBase; Trusted_Connection=True; TrustServerCertificate = True");
        conn.Open();
        SqlCommand m = new SqlCommand(query, conn);
        SqlDataReader r = m.ExecuteReader();
        StringBuilder str = new StringBuilder();
        while (r.Read())
        {
            string bId = r["ProductName"].ToString();
            int Pid = int.Parse(r["salesCount"].ToString());

            str.AppendLine($"{Pid} - {bId}");

        }
        conn.Close();
        foreach (var st in str.ToString())
        {
            Console.WriteLine(st);
        }
    }
}
