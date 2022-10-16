using System;
using System.Data.SQLite;

public class sqlite
{
    private static string dbCommand = "Data Source=DemoDB.db;Version=3;New=False;Compress=True;";
    private static SQLiteConnection dbConnection = new SQLiteConnection(dbCommand);
    private static SQLiteCommand Command = new SQLiteCommand("", dbConnection);

    public void StartSQL()
    {
        Console.WriteLine("Starting SQL");
        //dbConnection.Open();
        //Console.WriteLine("SQL Started");
        openConnection();
        
        using (var transaction = connection.BeginTransaction())
                {
                    var insertCmd = connection.CreateCommand();

                    insertCmd.CommandText = "INSERT data INTO Person VALUES('LAGUNITAS')";
                    insertCmd.ExecuteNonQuery();

                    insertCmd.CommandText = "INSERT data INTO Person VALUES('JAI ALAI')";
                    insertCmd.ExecuteNonQuery();

                    insertCmd.CommandText = "INSERT data INTO Person VALUES('RANGER')";
                    insertCmd.ExecuteNonQuery();

                    transaction.Commit();
                }
      
       var selectCmd = dbConnection.CreateCommand();

                selectCmd.CommandText = "SELECT data FROM Person";

                using (var reader = selectCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var message = reader.GetString(0);
                        Console.WriteLine(message);
                    }
                } 
        closeConnection();
    }

    private void openConnection()
    {
        Console.WriteLine("Opening Connection");
        // TODO
        if (dbConnection.State == System.Data.ConnectionState.Closed)
        {
            dbConnection.Open();
            Console.WriteLine("Connection opened to:" + dbConnection.State.ToString());
        }
    }

    private void closeConnection()
    {
        // TODO
        if (dbConnection.State == System.Data.ConnectionState.Open)
        {
            dbConnection.Close();
            Console.WriteLine("Connection closed to:" + dbConnection.State.ToString());
        }
    }



}
public class Program
{
   // Uncomment the following line to resolve.
 static void Main() 
   {
    var instance = new sqlite();
    instance.StartSQL();
   }
}
