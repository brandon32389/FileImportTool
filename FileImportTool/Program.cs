using System;
using System.Collections.Generic;
using System.IO;


namespace FileImportTool
{
    class Program
    {
        static int Main(string[] args)
        {
            // Code to return to the SQL job agent. 0 = success, negative = failure. We can customize this if we think it out.
            int returnCode = 0;

            // bools to help determine which actions to take. Popul
            bool tableExists = false;
            bool columnsMatch = true; //testing
            bool tableCreated = false;
            bool truncateTable = false;


            // Set params sent in at runtime to be used in the program.
            // Param order: // [0]-FilePath | [1]-TableName | [2] TruncateYesNo | [3]-Delimeter.
            string filePath = args[0];//Environment.GetCommandLineArgs()[0];
            string tableString = args[1];//Environment.GetCommandLineArgs()[1];

            // Set the Truncate table variable to true if the value is yes. Defaulted to no on initialization.
            if (args[2].ToLower() == "yes")
            {
                truncateTable = true;
            }
            
            // Assign the delimeter of the file if CSV.
            string delimeter = args[3];

            // Get the extension of the file to decide how to import the file.
            string fileExtension = Path.GetExtension(filePath);
            
            //Breaks table name into its parts to be used in the connection string.
            List<string> tableComponents = new List<string>();
            foreach (string value in tableString.Split('.'))
            {
                tableComponents.Add(value);// [0]-Server | [1]-Database | [2] Schema | [3]-Table name.
            }
            
            string server = tableComponents[0].ToString();
            string database = tableComponents[1].ToString();
            //Builds the schema.table name structure that SQL expects. 2 = schema, 3 = table name.
            string tableName = $"{tableComponents[2].ToString()}.{tableComponents[3].ToString()}";

            // Set up connection string. [TODO: Could this be generalized further to allow it to work outside of SHU?]
            string connString = $"Data Source={server}.sacredheart.edu;Initial Catalog={database};Integrated Security=SSPI";

            //Create objects needed.
            FilePrep fp = new FilePrep();
            Sql sql = new Sql();

            // Import the file.
            fp.LoadFile(filePath, fileExtension, delimeter);

            // Create Sql connection. Future proofing in case we go multi-server in future release.
            sql.ConnectToSql(connString);

            // See if table exists.
            tableExists = sql.CheckTable(tableName);

            // If table exists, check whether or not to truncate the table.
            if (tableExists && truncateTable)
            {
                sql.TruncateTable(tableName);
            }

            //// If table exists, check whether or not the columns match.
            //if (tableExists)
            //{
            //    columnsMatch = sql.CheckColumns(tableName, fp.Columns);
            //}

            // If table does not exist, create it set tableCreated to true.
            if (!tableExists)
            {
                tableCreated = sql.CreateTable(tableName, fp.Columns);

            }

            // If table exists and the columns match or the table was created then insert the rows.
            if ((tableExists && columnsMatch) || tableCreated)
            {
                sql.InsertRows(tableName, fp.Columns, fp.Values);
            }


            //Test output
            //Console.WriteLine($"truncateTable = {truncateTable}");
            //Console.WriteLine($"tableExists = {tableExists}");
            //Console.WriteLine($"columnsMatch = {columnsMatch}");
            //Console.WriteLine($"tableCreated = {tableCreated}");

            //Console.ReadLine();

            sql.CloseConnection();
            // Return the status code to indicate success or failure.
            return returnCode;
        }
    }
}
