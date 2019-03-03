using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using OfficeOpenXml;
using Microsoft.EntityFrameworkCore;
using SN_BNB.Data;

namespace DatabaseLibrary
{
    class Excel
    {
        /* The excel document for users will have three columns, in this order */
        protected struct UserStruct
        {
            string username;
            string password;
            string role;
        }

        /* The excel document for Fixtures will have these columns, in this order */
        public struct FixtureStruct
        {
            public string FixtureDateTime;
            public string Location;     //need to find the location id for sql statement
            public string HomeTeam;     //need to find team id for sql statement
            public string AwayTeam;     
        }


        public void CreateUsers(string excelDocPath)
        {
            //create a struct to hold username, password, and role
            UserStruct[] dataStructs = new UserStruct[2000];

            //receive excel file
            FileInfo file = new FileInfo(excelDocPath);
            ExcelPackage excelPackage = new ExcelPackage(file);
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[1];

            //parse the file and update struct
            //update user table
            //send confirmation or error message

        }

        public void CreateFixtures(string excelDocPath, SNContext context)
        {
            //create a struct to hold fixture data
            FixtureStruct[] dataStructs = new FixtureStruct[2000];

            //receive excel file
            FileInfo file = new FileInfo(excelDocPath);
            ExcelPackage excelPackage = new ExcelPackage(file);
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[1];

            //parse the file and update struct
            int row = 1;
            while (true)
            {

                if (worksheet.Cells[row, 1].Value.ToString() == "")  break;
                FixtureStruct tempStruct = new FixtureStruct();
            
                tempStruct.FixtureDateTime = worksheet.Cells[row, 1].Value.ToString();
                tempStruct.Location = worksheet.Cells[row, 2].Value.ToString();
                tempStruct.HomeTeam = worksheet.Cells[row, 3].Value.ToString();
                tempStruct.AwayTeam = worksheet.Cells[row, 4].Value.ToString();

                row += 1;
                dataStructs.Append(tempStruct);
            }
            //find location id, hometeam id, and awayteam id
            //make a new season
            //update fixture table
            //send confirmation or error message
        }
    }
}