//after running whatever code once, add db file to .../bin/debug/net8.0
//add SQLite via tools - NuGet package
//read data from db, execute SQL command select / insert / delete / select

using System.Data.SQLite;       //appears automatically from SQLiteConnection

readData(CreateConnection());
//insertCustomer(CreateConnection());
//removeCustomer(CreateConnection());
findCustomer(CreateConnection());

static SQLiteConnection CreateConnection()
{
    SQLiteConnection connection = new SQLiteConnection("Data Source=mydb.db; Version = 3; New = True; Compress = True;");
    //values needed for connection saved in SQLiteConnection
    
    try
    {
        connection.Open();
        Console.WriteLine("DB found.");
    }
    catch       //works if try fails
    {
        Console.WriteLine("DB not found.");
    }
    return connection;
}

static void readData(SQLiteConnection myConnection)
{
    Console.Clear();
    SQLiteDataReader reader;        //saves data into itself
    SQLiteCommand command;

    command = myConnection.CreateCommand();
    command.CommandText = "SELECT rowid, * FROM customer";     //SQL command code goes here

    reader = command.ExecuteReader();   //actual read command

    while (reader.Read())       //repeats reads until rows are over
    {
        string readerRowId = reader["rowid"].ToString();      //or use reader.GetString(0)
        string readerStringFirstName = reader.GetString(1);
        string readerStringLastName = reader.GetString(2);
        string readerStringDoB = reader.GetString(3);

        Console.WriteLine($"{readerRowId}. Full name: {readerStringFirstName} {readerStringLastName}; DoB: {readerStringDoB}");
    }    
    myConnection.Close();
}

static void insertCustomer(SQLiteConnection myConnection)
{
    SQLiteCommand command;
    string fName, lName, DoB;

    Console.WriteLine("Enter first name:");
    fName = Console.ReadLine();
    Console.WriteLine("Enter last name:");
    lName = Console.ReadLine();
    Console.WriteLine("Enter date of birth (mm-dd-yyyy):");
    DoB = Console.ReadLine();

    command = myConnection.CreateCommand();
    command.CommandText = $"INSERT INTO customer (firstName,lastName,dateOfBirth) VALUES ('{fName}', '{lName}', '{DoB}')";

    int rowInserted = command.ExecuteNonQuery();      //doesnt read = non query
    Console.WriteLine($"Row inserted: {rowInserted}");

    readData(myConnection);
}

static void removeCustomer(SQLiteConnection myConnection)
{
    SQLiteCommand command;

    string idToDelete;
    Console.WriteLine("Ender customer id to delete:");
    idToDelete = Console.ReadLine();

    command = myConnection.CreateCommand();
    command.CommandText = $"DELETE FROM customer WHERE rowid = {idToDelete}";

    int rowRemoved = command.ExecuteNonQuery();
    Console.WriteLine($"{rowRemoved} was removed from customer table.");

    readData(myConnection);
}

static void findCustomer(SQLiteConnection myConnection)
{
    SQLiteDataReader reader;
    SQLiteCommand command;
    string searchName;
    Console.WriteLine("Enter a first name to display customer data:");

    searchName = Console.ReadLine();
    command = myConnection.CreateCommand();
    command.CommandText = $"SELECT customer.rowid, customer.firstName, customer.lastName, status.statusType FROM customerStatus " +
    $"JOIN customer ON customer.rowid = customerStatus.customerId " +
    $"JOIN status ON status.rowid = customerStatus.statusId " +
    $"WHERE firstname LIKE '{searchName}'";

    reader = command.ExecuteReader();

    while (reader.Read())
    {
        string readerRowid = reader["rowid"].ToString();
        string readerStringName = reader.GetString(1);
        string readerStringLastName = reader.GetString(2);
        string readerStringStatus = reader.GetString(3);

        Console.WriteLine($"Search result: ID: {readerRowid}. {readerStringName} {readerStringLastName}. Status: {readerStringStatus}");
    }
    myConnection.Close();
}