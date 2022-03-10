using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileImportTool
{
    class FilePrep
    {
        //List<string[]>
         
        public List<string> Columns = new List<string>();
        public List<string[]> Values = new List<string[]>();
        /************************
         Created: 07/23/21
         Last Updated: 07/23/21
         Description: takes file passed in at runtime, reads in it contents into an object that has the name/value pairs 
         Params: Object? - Do we need to pass in, or create and return?
                 File - The delimited file to be worked on.
                 Delimeter - The character that denotes the separation of columns. typically comma.
         Returns: SqlConnection object.
        ***********************/
        public void LoadFile(string filePath, string fileExtension, string delimeter) {
            //[TODO: Is there a more efficient way to check the type of file? and for CSV, can we discover the delimeter instead of user input?]
            switch (fileExtension)
            {
                case ".csv":
                    ImportCsv(filePath, delimeter);
                    break;
                case ".xslx":
                    ImportExcel(filePath);
                    break;
            }
        }
        /************************
         Created: 07/23/21
         Last Updated: 07/23/21
         Description: Reads file in and loads data into a list.
         Params: FilePath
                 Delimeter - The character that denotes the separation of columns. typically comma.
         Returns: SqlConnection object.
        ***********************/
        private void ImportExcel(string filePath)
        {
            throw new NotImplementedException();
        }
        /************************
         Created: 07/23/21
         Last Updated: 07/23/21
         Description: takes file passed in at runtime, reads in it contents into a two objects that store 
                      the column names and data records from the file. 
         Params: File - The delimited file to be worked on.
                 Delimeter - The character that denotes the separation of columns. typically comma.
         Returns: N/A
        ***********************/
        private void ImportCsv(string filePath, string delimeter)
        {
            List<string[]> csv = new List<string[]>();
            var lines = File.ReadAllLines(filePath);

            //Put the column names into columns list.
            foreach (string value in lines[0].Split(delimeter.ToCharArray()))
            {
                Columns.Add(value);
            }

            //Insert records into the Values list.
            for(int i = 1; i < lines.Count(); i++)
            {
                Values.Add(lines[i].Split(delimeter.ToCharArray()));
            }


        }    
    }
}

    

