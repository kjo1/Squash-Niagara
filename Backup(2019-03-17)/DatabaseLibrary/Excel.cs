using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using OfficeOpenXml;
using Microsoft.EntityFrameworkCore;

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
        protected struct FixtureStruct
        {
            string FixtureDateTime;
            string Location;     //need to find the location id for sql statement
            string HomeTeam;     //need to find team id for sql statement
            string AwayTeam;     
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

        public void CreateFixtures(string excelDocPath, DbContext context)
        {
            //create a struct to hold fixture data
            FixtureStruct[] dataStructs = new FixtureStruct[2000];

            //receive excel file
            FileInfo file = new FileInfo(excelDocPath);
            ExcelPackage excelPackage = new ExcelPackage(file);
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[1];

            //parse the file and update struct
            //find location id, hometeam id, and awayteam id
            //make a new season
            //update fixture table
            //send confirmation or error message
        }
    }
}