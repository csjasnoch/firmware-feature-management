using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateWithEnhancedProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NpiPrograms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    MajorVersion = table.Column<int>(type: "INTEGER", nullable: false),
                    HardwareGen = table.Column<int>(type: "INTEGER", nullable: false),
                    ProgramStartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    PmoOwner = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NpiPrograms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OperationalFlows",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CollectionId = table.Column<Guid>(type: "TEXT", nullable: false),
                    FeatureId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationalFlows", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FirmwareVersions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    NpiProgramId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Version = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Stage = table.Column<int>(type: "INTEGER", nullable: false),
                    FilePath = table.Column<string>(type: "TEXT", nullable: false),
                    ReleasedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ReleasedBy = table.Column<string>(type: "TEXT", nullable: false),
                    ChangeLog = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FirmwareVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FirmwareVersions_NpiPrograms_NpiProgramId",
                        column: x => x.NpiProgramId,
                        principalTable: "NpiPrograms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FlowOperation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    OperationalFlowId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlowOperation", x => new { x.OperationalFlowId, x.Id });
                    table.ForeignKey(
                        name: "FK_FlowOperation_OperationalFlows_OperationalFlowId",
                        column: x => x.OperationalFlowId,
                        principalTable: "OperationalFlows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommandDefinitions",
                columns: table => new
                {
                    CommandCode = table.Column<byte>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    ExpectedResponseCode = table.Column<byte>(type: "INTEGER", nullable: false),
                    IsDeprecated = table.Column<bool>(type: "INTEGER", nullable: false),
                    DeprecationMessage = table.Column<string>(type: "TEXT", nullable: false),
                    FirmwareVersionId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommandDefinitions", x => x.CommandCode);
                    table.ForeignKey(
                        name: "FK_CommandDefinitions_FirmwareVersions_FirmwareVersionId",
                        column: x => x.FirmwareVersionId,
                        principalTable: "FirmwareVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FeatureCollections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    NpiProgramId = table.Column<Guid>(type: "TEXT", nullable: true),
                    RequiredLpiVersionId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RequiredPiccoloVersionId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Visibility = table.Column<int>(type: "INTEGER", nullable: false),
                    Owner = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    SharedWith = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsLocked = table.Column<bool>(type: "INTEGER", nullable: false),
                    LockedReason = table.Column<string>(type: "TEXT", nullable: false),
                    Tags = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeatureCollections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeatureCollections_FirmwareVersions_RequiredLpiVersionId",
                        column: x => x.RequiredLpiVersionId,
                        principalTable: "FirmwareVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FeatureCollections_FirmwareVersions_RequiredPiccoloVersionId",
                        column: x => x.RequiredPiccoloVersionId,
                        principalTable: "FirmwareVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FeatureCollections_NpiPrograms_NpiProgramId",
                        column: x => x.NpiProgramId,
                        principalTable: "NpiPrograms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FlowStep",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    FlowOperationOperationalFlowId = table.Column<Guid>(type: "TEXT", nullable: false),
                    FlowOperationId = table.Column<Guid>(type: "TEXT", nullable: false),
                    StepNumber = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: false),
                    Attributes = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlowStep", x => new { x.FlowOperationOperationalFlowId, x.FlowOperationId, x.Id });
                    table.ForeignKey(
                        name: "FK_FlowStep_FlowOperation_FlowOperationOperationalFlowId_FlowOperationId",
                        columns: x => new { x.FlowOperationOperationalFlowId, x.FlowOperationId },
                        principalTable: "FlowOperation",
                        principalColumns: new[] { "OperationalFlowId", "Id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ParameterDefinitions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    DataType = table.Column<string>(type: "TEXT", nullable: false),
                    MinValue = table.Column<int>(type: "INTEGER", nullable: false),
                    MaxValue = table.Column<int>(type: "INTEGER", nullable: false),
                    DefaultValue = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    IsDeprecated = table.Column<bool>(type: "INTEGER", nullable: false),
                    DeprecationMessage = table.Column<string>(type: "TEXT", nullable: false),
                    CommandDefinitionCommandCode = table.Column<byte>(type: "INTEGER", nullable: true),
                    FirmwareVersionId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParameterDefinitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParameterDefinitions_CommandDefinitions_CommandDefinitionCommandCode",
                        column: x => x.CommandDefinitionCommandCode,
                        principalTable: "CommandDefinitions",
                        principalColumn: "CommandCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParameterDefinitions_FirmwareVersions_FirmwareVersionId",
                        column: x => x.FirmwareVersionId,
                        principalTable: "FirmwareVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Features",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Category = table.Column<string>(type: "TEXT", nullable: false),
                    IsComposite = table.Column<bool>(type: "INTEGER", nullable: false),
                    RequiredLpiParams = table.Column<string>(type: "TEXT", nullable: false),
                    RequiredPiccoloCommands = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Owner = table.Column<string>(type: "TEXT", nullable: false),
                    FeatureCollectionId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Features", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Features_FeatureCollections_FeatureCollectionId",
                        column: x => x.FeatureCollectionId,
                        principalTable: "FeatureCollections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FlowBranch",
                columns: table => new
                {
                    FlowStepFlowOperationOperationalFlowId = table.Column<Guid>(type: "TEXT", nullable: false),
                    FlowStepFlowOperationId = table.Column<Guid>(type: "TEXT", nullable: false),
                    FlowStepId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Label = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Action = table.Column<string>(type: "TEXT", nullable: false),
                    IsErrorPath = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlowBranch", x => new { x.FlowStepFlowOperationOperationalFlowId, x.FlowStepFlowOperationId, x.FlowStepId, x.Id });
                    table.ForeignKey(
                        name: "FK_FlowBranch_FlowStep_FlowStepFlowOperationOperationalFlowId_FlowStepFlowOperationId_FlowStepId",
                        columns: x => new { x.FlowStepFlowOperationOperationalFlowId, x.FlowStepFlowOperationId, x.FlowStepId },
                        principalTable: "FlowStep",
                        principalColumns: new[] { "FlowOperationOperationalFlowId", "FlowOperationId", "Id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ParameterValue",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    FeatureId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ParameterId = table.Column<int>(type: "INTEGER", nullable: false),
                    ParameterName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Value = table.Column<int>(type: "INTEGER", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParameterValue", x => new { x.FeatureId, x.Id });
                    table.ForeignKey(
                        name: "FK_ParameterValue_Features_FeatureId",
                        column: x => x.FeatureId,
                        principalTable: "Features",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PiccoloCommand",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    FeatureId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CommandCode = table.Column<byte>(type: "INTEGER", nullable: false),
                    CommandName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Payload = table.Column<string>(type: "TEXT", nullable: false),
                    ExecutionOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PiccoloCommand", x => new { x.FeatureId, x.Id });
                    table.ForeignKey(
                        name: "FK_PiccoloCommand_Features_FeatureId",
                        column: x => x.FeatureId,
                        principalTable: "Features",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommandDefinitions_FirmwareVersionId",
                table: "CommandDefinitions",
                column: "FirmwareVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_FeatureCollections_NpiProgramId",
                table: "FeatureCollections",
                column: "NpiProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_FeatureCollections_RequiredLpiVersionId",
                table: "FeatureCollections",
                column: "RequiredLpiVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_FeatureCollections_RequiredPiccoloVersionId",
                table: "FeatureCollections",
                column: "RequiredPiccoloVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_Features_FeatureCollectionId",
                table: "Features",
                column: "FeatureCollectionId");

            migrationBuilder.CreateIndex(
                name: "IX_FirmwareVersions_NpiProgramId",
                table: "FirmwareVersions",
                column: "NpiProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_ParameterDefinitions_CommandDefinitionCommandCode",
                table: "ParameterDefinitions",
                column: "CommandDefinitionCommandCode");

            migrationBuilder.CreateIndex(
                name: "IX_ParameterDefinitions_FirmwareVersionId",
                table: "ParameterDefinitions",
                column: "FirmwareVersionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FlowBranch");

            migrationBuilder.DropTable(
                name: "ParameterDefinitions");

            migrationBuilder.DropTable(
                name: "ParameterValue");

            migrationBuilder.DropTable(
                name: "PiccoloCommand");

            migrationBuilder.DropTable(
                name: "FlowStep");

            migrationBuilder.DropTable(
                name: "CommandDefinitions");

            migrationBuilder.DropTable(
                name: "Features");

            migrationBuilder.DropTable(
                name: "FlowOperation");

            migrationBuilder.DropTable(
                name: "FeatureCollections");

            migrationBuilder.DropTable(
                name: "OperationalFlows");

            migrationBuilder.DropTable(
                name: "FirmwareVersions");

            migrationBuilder.DropTable(
                name: "NpiPrograms");
        }
    }
}
