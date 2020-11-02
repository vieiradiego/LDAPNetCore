using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.MySqlClient;
using System;
using System.IO;

namespace AgentNetCore.Migrations
{
    public partial class DumpData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string[] scriptsCreate = {
                                      File.ReadAllText(Environment.CurrentDirectory + @"\Data\Dataset\V1_0_1__Insert_Data_in_Credentials.sql"),
                                      File.ReadAllText(Environment.CurrentDirectory + @"\Data\Dataset\V1_0_2__Insert_Data_in_Servers.sql"),
                                      File.ReadAllText(Environment.CurrentDirectory + @"\Data\Dataset\V1_0_3__Insert_Data_in_Configurations.sql"),
                                     };
            foreach (var script in scriptsCreate)
            {
                migrationBuilder.Sql(script, false);
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
