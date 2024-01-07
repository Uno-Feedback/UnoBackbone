using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Uno.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Base");

            migrationBuilder.CreateTable(
                name: "User",
                schema: "Base",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SingupDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Base_User", x => x.Id);
                },
                comment: "stores information about registered users in our application");

            migrationBuilder.CreateTable(
                name: "Project",
                schema: "Base",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IP = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    InsertTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Base_Project", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Base_Project_Base_User",
                        column: x => x.UserId,
                        principalSchema: "Base",
                        principalTable: "User",
                        principalColumn: "Id");
                },
                comment: "Project information in project management software");

            migrationBuilder.CreateTable(
                name: "Connector",
                schema: "Base",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Url = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Type = table.Column<byte>(type: "tinyint", nullable: false, comment: "Space-efficient representation for C# enum values")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Base_Connector", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Base_Connector_Base_Project",
                        column: x => x.ProjectId,
                        principalSchema: "Base",
                        principalTable: "Project",
                        principalColumn: "Id");
                },
                comment: "Information on project control platforms for submitting issues");

            migrationBuilder.CreateTable(
                name: "Issue",
                schema: "Base",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(600)", maxLength: 600, nullable: false),
                    Reporter = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Url = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SentDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Base_Issue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Base_Issue_Base_Project",
                        column: x => x.ProjectId,
                        principalSchema: "Base",
                        principalTable: "Project",
                        principalColumn: "Id");
                },
                comment: "User recordings to be sent to the project management platform");

            migrationBuilder.CreateTable(
                name: "ConnectorReportPriorities",
                schema: "Base",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConnectorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Key = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Base_ConnectorReportPriorities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Base_ConnetorReportPriorities_Base_Connector",
                        column: x => x.ConnectorId,
                        principalSchema: "Base",
                        principalTable: "Connector",
                        principalColumn: "Id");
                },
                comment: "Stores connectors report priorities like bug or sub-task");

            migrationBuilder.CreateTable(
                name: "ConnectorReportTypes",
                schema: "Base",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConnectorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Key = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Base_ConnectorReportType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Base_ConnetorReportTypes_Base_Connector",
                        column: x => x.ConnectorId,
                        principalSchema: "Base",
                        principalTable: "Connector",
                        principalColumn: "Id");
                },
                comment: "Stores connectors report types like bug or sub-task");

            migrationBuilder.CreateTable(
                name: "ConnectorInIssue",
                schema: "Base",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConnectorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IssueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConnectorMetaData = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ClientMetaData = table.Column<string>(type: "nvarchar(750)", maxLength: 750, nullable: false),
                    IssueMetaData = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TryCount = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Base_ConnectorInIssue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Base_ConnectorInIssue_Base_Connector",
                        column: x => x.ConnectorId,
                        principalSchema: "Base",
                        principalTable: "Connector",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Base_ConnectorInIssue_Base_Issue",
                        column: x => x.IssueId,
                        principalSchema: "Base",
                        principalTable: "Issue",
                        principalColumn: "Id");
                },
                comment: "Stores many-to-many relationships between isuues and connectors");

            migrationBuilder.CreateTable(
                name: "IssueAttachment",
                schema: "Base",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IssueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: false),
                    Type = table.Column<byte>(type: "tinyint", nullable: false, comment: "Space-efficient representation for C# enum values")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Base_IssueAttachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Base_IssueAttachment_Base_Issue",
                        column: x => x.IssueId,
                        principalSchema: "Base",
                        principalTable: "Issue",
                        principalColumn: "Id");
                },
                comment: "Attachments with each Issue");

            migrationBuilder.CreateIndex(
                name: "IX_Base_Connector(ProjectId)",
                schema: "Base",
                table: "Connector",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Base_ConnectorInIssue(ConnectorId)",
                schema: "Base",
                table: "ConnectorInIssue",
                column: "ConnectorId");

            migrationBuilder.CreateIndex(
                name: "IX_Base_ConnectorInIssue(IssueId)",
                schema: "Base",
                table: "ConnectorInIssue",
                column: "IssueId");

            migrationBuilder.CreateIndex(
                name: "IX_Base_ConnectorReprtPriorities(ConnectorId)",
                schema: "Base",
                table: "ConnectorReportPriorities",
                column: "ConnectorId");

            migrationBuilder.CreateIndex(
                name: "IX_Base_ConnectorReprtTypes(ConnectorId)",
                schema: "Base",
                table: "ConnectorReportTypes",
                column: "ConnectorId");

            migrationBuilder.CreateIndex(
                name: "IX_Base_Issue(ProjectId)",
                schema: "Base",
                table: "Issue",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Base_IssueAttachment(IssueId)",
                schema: "Base",
                table: "IssueAttachment",
                column: "IssueId");

            migrationBuilder.CreateIndex(
                name: "IX_Base_Project(UserId)",
                schema: "Base",
                table: "Project",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConnectorInIssue",
                schema: "Base");

            migrationBuilder.DropTable(
                name: "ConnectorReportPriorities",
                schema: "Base");

            migrationBuilder.DropTable(
                name: "ConnectorReportTypes",
                schema: "Base");

            migrationBuilder.DropTable(
                name: "IssueAttachment",
                schema: "Base");

            migrationBuilder.DropTable(
                name: "Connector",
                schema: "Base");

            migrationBuilder.DropTable(
                name: "Issue",
                schema: "Base");

            migrationBuilder.DropTable(
                name: "Project",
                schema: "Base");

            migrationBuilder.DropTable(
                name: "User",
                schema: "Base");
        }
    }
}
