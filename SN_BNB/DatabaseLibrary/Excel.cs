using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using OfficeOpenXml;

namespace DatabaseLibrary
{
    class Excel
    {
        protected struct DataStruct
        {
            string username;
            string password;
            string role;
        }
        public void CreateUsers(string excelDocPath)
        {
            //create a struct to hold username, password, and role
            DataStruct[] dataStructs = new DataStruct[2000];
            //receive excel file
            FileInfo file = new FileInfo(excelDocPath);
            ExcelPackage excelPackage = new ExcelPackage(file);
            //parse the file and update struct
            //update user table
            //send confirmation or error message

        }
    }
}
