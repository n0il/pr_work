// DESKTOP-OH4KKOB\SQLEXPRESS
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.SqlClient;
using System.Text;


SqlConnection conn = new SqlConnection(@"Server=DESKTOP-OH4KKOB\SQLEXPRESS; Database=master; Trusted_Connection=True; TrustServerCertificate = True");
void CreateDb()
{
    string newstr = "SELECT database_id FROM sys.databases WHERE Name = 'myDataBase'";  
    SqlCommand cmd1 = new SqlCommand(newstr, conn);
  //  cmd1.ExecuteScalar();





    if (cmd1.ExecuteScalar()!=null)
    {
        string cmd = "CREATE DATABASE myDataBase";
        conn.Open();
        SqlCommand comm = new SqlCommand(cmd, conn);
        comm.ExecuteNonQuery();
        conn.Close();
    }



}

//  CreateDb();



void CreateColumns()
{
    string colcomand1 = "create table Products(\r\npr_Id int PRIMARY KEY IDENTITY (1,1),\r\npr_name varchar(20)\r\n)\r\n\r\n\r\ncreate table Buyers(\r\nb_Id int PRIMARY KEY IDENTITY (1,1),\r\nb_name varchar(20)\r\n)\r\n\r\n\r\ncreate table Sales(\r\ns_id int PRIMARY KEY IDENTITY (1,1),\r\npr_Id int FOREIGN KEY REFERENCES Products(pr_Id),\r\nb_Id int FOREIGN KEY REFERENCES Buyers(b_Id)\r\n)";
    conn = new SqlConnection(@"Server=DESKTOP-OH4KKOB\SQLEXPRESS; Database=myDataBase; Trusted_Connection=True; TrustServerCertificate = True");
    conn.Open();
    SqlCommand comm1 = new SqlCommand(colcomand1, conn);
    comm1.ExecuteNonQuery();
    conn.Close();
}
//CreateColumns();

void AddData()
{
    string command = "INSERT into Products(pr_name)\r\n" +
        "VALUES('Milk'), ('Ball'), ('Winter Tires')\r\n\r\n" +
        "INSERT INTO Buyers(b_name)\r\n" +
        "VALUES ('Martin'), ('Ivailo'), ('Greskomers')\r\n\r\n" +
        "INSERT INTO Sales(pr_Id, b_Id)\r\n" +
        "VALUES(1,2), (2,2), (3, 1), (3,2), (3,3), (1,1)";
    conn = new SqlConnection(@"Server=DESKTOP-OH4KKOB\SQLEXPRESS; Database=myDataBase; Trusted_Connection=True; TrustServerCertificate = True");
    conn.Open();
    SqlCommand c = new SqlCommand(command, conn);
    c.ExecuteNonQuery();
    conn.Close();
}
//AddData();

void QueryMethod()
{
    string query = "SELECT p.pr_name AS [ProductName], COUNT(b_id) AS salesCount\r\n" +
        "from Sales AS s JOIN Products AS p ON s.pr_Id = p.pr_Id\r\nGROUP BY p.pr_name";
    conn = new SqlConnection(@"Server=DESKTOP-OH4KKOB\SQLEXPRESS; Database=myDataBase; Trusted_Connection=True; TrustServerCertificate = True");
    conn.Open();
    SqlCommand m = new SqlCommand(query, conn);
    SqlDataReader r = m.ExecuteReader();
    StringBuilder str = new StringBuilder();
    while (r.Read())
    {
        string  bId = r["ProductName"].ToString();
        int Pid = int.Parse(r["salesCount"].ToString());
        
        str.AppendLine($"{Pid} - {bId}");

    }
    conn.Close();
    foreach (var st in str.ToString())
    {
        Console.WriteLine(st);
    }
}

QueryMethod();