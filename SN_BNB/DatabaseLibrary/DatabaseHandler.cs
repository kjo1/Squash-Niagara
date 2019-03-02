using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLibrary
{
    public class DatabaseHandler
    {

        public void Create(string data, string table)
        {
            //controller sends data to this function
            //also sent is the table to be updated
            string values = "";
            switch (table)
            {
                case "Team":
                    values = "(TeamName, Division_idDivision, TeamPoints, TeamCaptain_idPlayer, TeamCreatedOn, TeamWins, TeamLosses)";
                    break;
                case "Division":
                    values = "(DivisionName)";
                    break;
                case "Fixture":
                    values = "(FixtureDataTime, Location_idLocation, HomeScore, AwayScore, idHomeTeam, idAwayTeam, Season_idSeason)";
                    break;
                default:
                    break;
            }

            //a create command is generated
            string command = "INSERT INTO " + table + values + "VALUES " + data + ";";

            //the command is sent to the database
            //a confirmation or error message is returned
        }

        public void Update(string table)
        {
            //controller sends data to this function
            //also sent is the table to be updated
            //an update command is generated
            string command = "UPDATE " + table;

            //the command is sent to the database
            //a confirmation or error message is returned
        }

        public void Delete(string table)
        {
            //controller sends data to this function
            //also sent is the table to be updated
            //a delete command is generated
            string command = "DELETE FROM " + table;

            //the command is sent to the database
            //a confirmation or error message is returned
        }
    }
}
