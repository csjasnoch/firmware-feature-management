using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Services;

public partial class FirmwareDataService
{
    private readonly ApplicationDbContext _context;

    public FirmwareDataService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task SeedDataAsync()
    {
        // Only seed if database is empty
        if (await _context.NpiPrograms.AnyAsync())
        {
            return;
        }

        var npiPrograms = InitializeNpiPrograms();
        await _context.NpiPrograms.AddRangeAsync(npiPrograms);
        await _context.SaveChangesAsync();

        // Create shared parameter and command definitions first
        var allParameters = new List<ParameterDefinition>();
        var allCommands = new List<CommandDefinition>();
        
        var lpiVersions = InitializeLpiVersionsWithSharedParameters(npiPrograms, allParameters);
        var piccoloVersions = InitializePiccoloVersionsWithSharedCommands(npiPrograms, allCommands);
        
        // Add unique parameters and commands to context
        await _context.ParameterDefinitions.AddRangeAsync(allParameters);
        await _context.CommandDefinitions.AddRangeAsync(allCommands);
        await _context.SaveChangesAsync();
        
        // Now add firmware versions
        await _context.FirmwareVersions.AddRangeAsync(lpiVersions);
        await _context.FirmwareVersions.AddRangeAsync(piccoloVersions);
        await _context.SaveChangesAsync();

        // Use enhanced features based on actual PICCOLO firmware data
        var features = InitializeFeaturesEnhanced();
        await _context.Features.AddRangeAsync(features);
        await _context.SaveChangesAsync();

        var collections = InitializeCollections(npiPrograms, lpiVersions, piccoloVersions, features);
        await _context.FeatureCollections.AddRangeAsync(collections);
        await _context.SaveChangesAsync();
    }

    private List<NpiProgram> InitializeNpiPrograms()
    {
        return new List<NpiProgram>
        {
            new NpiProgram
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Name = "Edina",
                Description = "Legacy Gen2 platform",
                MajorVersion = 2,
                HardwareGen = HardwareGeneration.Gen2,
                ProgramStartDate = new DateTime(2024, 1, 1),
                Status = ProgramStatus.Deprecated,
                PmoOwner = "Legacy Support Team"
            },
            new NpiProgram
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Name = "Eagan",
                Description = "Current production Gen3 platform with enhanced audio",
                MajorVersion = 3,
                HardwareGen = HardwareGeneration.Gen3,
                ProgramStartDate = new DateTime(2025, 6, 1),
                Status = ProgramStatus.Released,
                PmoOwner = "Jane Smith"
            },
            new NpiProgram
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Name = "Elko",
                Description = "Next generation Gen4 platform (in development)",
                MajorVersion = 4,
                HardwareGen = HardwareGeneration.Gen4,
                ProgramStartDate = new DateTime(2026, 1, 1),
                Status = ProgramStatus.Active,
                PmoOwner = "Bob Johnson"
            },
            new NpiProgram
            {
                Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                Name = "Fridley",
                Description = "Specialized industrial variant (Gen3 hardware)",
                MajorVersion = 3,
                HardwareGen = HardwareGeneration.Gen3,
                ProgramStartDate = new DateTime(2025, 9, 1),
                Status = ProgramStatus.Active,
                PmoOwner = "Alice Williams"
            },
            new NpiProgram
            {
                Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                Name = "Chaska",
                Description = "Future planning (Gen4+)",
                MajorVersion = 5,
                HardwareGen = HardwareGeneration.Gen4,
                ProgramStartDate = new DateTime(2026, 6, 1),
                Status = ProgramStatus.Planning,
                PmoOwner = "TBD"
            }
        };
    }

    private List<FirmwareVersion> InitializeLpiVersions(List<NpiProgram> npiPrograms)
    {
        // This method is deprecated - use InitializeLpiVersionsWithSharedParameters instead
        throw new NotImplementedException();
    }

    private List<FirmwareVersion> InitializeLpiVersionsWithSharedParameters(List<NpiProgram> npiPrograms, List<ParameterDefinition> allParameters)
    {
        var edina = npiPrograms.First(p => p.Name == "Edina");
        var eagan = npiPrograms.First(p => p.Name == "Eagan");
        var elko = npiPrograms.First(p => p.Name == "Elko");

        int paramIdCounter = 1000;
        
        var versions = new List<FirmwareVersion>();

        // Edina (Legacy)
        var edina280Params = CreateUniqueParameters(20, ref paramIdCounter);
        allParameters.AddRange(edina280Params);
        versions.Add(new FirmwareVersion
        {
            Id = Guid.NewGuid(),
            NpiProgramId = edina.Id,
            NpiProgram = edina,
            Version = "2.8.0",
            Stage = FirmwareReleaseStage.T3_Released,
            ReleasedAt = new DateTime(2024, 11, 1),
            ReleasedBy = "Legacy Team",
            ChangeLog = new List<string> { "Final Edina release" },
            Parameters = edina280Params
        });

        // Eagan versions
        var eagan310Params = CreateUniqueParameters(24, ref paramIdCounter);
        allParameters.AddRange(eagan310Params);
        versions.Add(new FirmwareVersion
        {
            Id = Guid.NewGuid(),
            NpiProgramId = eagan.Id,
            NpiProgram = eagan,
            Version = "3.1.0",
            Stage = FirmwareReleaseStage.T3_Released,
            ReleasedAt = new DateTime(2025, 12, 1),
            ReleasedBy = "Firmware Team",
            ChangeLog = new List<string> { "Eagan initial release", "24 new parameters" },
            Parameters = eagan310Params
        });

        var eagan320Params = CreateUniqueParameters(26, ref paramIdCounter);
        allParameters.AddRange(eagan320Params);
        versions.Add(new FirmwareVersion
        {
            Id = Guid.NewGuid(),
            NpiProgramId = eagan.Id,
            NpiProgram = eagan,
            Version = "3.2.0",
            Stage = FirmwareReleaseStage.T3_Released,
            ReleasedAt = new DateTime(2026, 1, 15),
            ReleasedBy = "Firmware Team",
            ChangeLog = new List<string> { "Added 2 new directionality parameters" },
            Parameters = eagan320Params
        });

        var eagan321Params = CreateUniqueParameters(26, ref paramIdCounter);
        allParameters.AddRange(eagan321Params);
        versions.Add(new FirmwareVersion
        {
            Id = Guid.NewGuid(),
            NpiProgramId = eagan.Id,
            NpiProgram = eagan,
            Version = "3.2.1",
            Stage = FirmwareReleaseStage.T3_Released,
            ReleasedAt = new DateTime(2026, 1, 28),
            ReleasedBy = "Firmware Team",
            ChangeLog = new List<string> { "Bug fixes only", "Fixed gain calculation" },
            Parameters = eagan321Params
        });

        var eagan330Params = CreateUniqueParameters(28, ref paramIdCounter);
        allParameters.AddRange(eagan330Params);
        versions.Add(new FirmwareVersion
        {
            Id = Guid.NewGuid(),
            NpiProgramId = eagan.Id,
            NpiProgram = eagan,
            Version = "3.3.0",
            Stage = FirmwareReleaseStage.T2_Beta,
            ReleasedAt = new DateTime(2026, 2, 1),
            ReleasedBy = "Firmware Team",
            ChangeLog = new List<string> { "Beta: New noise gate feature" },
            Parameters = eagan330Params
        });

        // Elko (Next Gen)
        var elko400Params = CreateUniqueParameters(32, ref paramIdCounter);
        allParameters.AddRange(elko400Params);
        versions.Add(new FirmwareVersion
        {
            Id = Guid.NewGuid(),
            NpiProgramId = elko.Id,
            NpiProgram = elko,
            Version = "4.0.0",
            Stage = FirmwareReleaseStage.T0_Test,
            ReleasedAt = new DateTime(2026, 2, 2),
            ReleasedBy = "R&D Team",
            ChangeLog = new List<string> { "Experimental: Complete redesign", "IDs may change" },
            Parameters = elko400Params
        });

        return versions;
    }

    private List<FirmwareVersion> InitializePiccoloVersions(List<NpiProgram> npiPrograms)
    {
        // This method is deprecated - use InitializePiccoloVersionsWithSharedCommands instead
        throw new NotImplementedException();
    }

    private List<FirmwareVersion> InitializePiccoloVersionsWithSharedCommands(List<NpiProgram> npiPrograms, List<CommandDefinition> allCommands)
    {
        var edina = npiPrograms.First(p => p.Name == "Edina");
        var eagan = npiPrograms.First(p => p.Name == "Eagan");
        var elko = npiPrograms.First(p => p.Name == "Elko");

        byte cmdCodeCounter = 0x10;
        
        var versions = new List<FirmwareVersion>();

        // Edina (Legacy)
        var edina190Cmds = CreateUniqueCommands(14, ref cmdCodeCounter);
        allCommands.AddRange(edina190Cmds);
        versions.Add(new FirmwareVersion
        {
            Id = Guid.NewGuid(),
            NpiProgramId = edina.Id,
            NpiProgram = edina,
            Version = "1.9.0",
            Stage = FirmwareReleaseStage.T3_Released,
            ReleasedAt = new DateTime(2024, 10, 20),
            ReleasedBy = "Legacy Team",
            ChangeLog = new List<string> { "Final Edina PICCOLO release" },
            Commands = edina190Cmds
        });

        // Eagan versions
        var eagan240Cmds = CreateUniqueCommands(16, ref cmdCodeCounter);
        allCommands.AddRange(eagan240Cmds);
        versions.Add(new FirmwareVersion
        {
            Id = Guid.NewGuid(),
            NpiProgramId = eagan.Id,
            NpiProgram = eagan,
            Version = "2.4.0",
            Stage = FirmwareReleaseStage.T3_Released,
            ReleasedAt = new DateTime(2025, 11, 20),
            ReleasedBy = "Firmware Team",
            ChangeLog = new List<string> { "Eagan initial PICCOLO release" },
            Commands = eagan240Cmds
        });

        var eagan250Cmds = CreateUniqueCommands(18, ref cmdCodeCounter);
        allCommands.AddRange(eagan250Cmds);
        versions.Add(new FirmwareVersion
        {
            Id = Guid.NewGuid(),
            NpiProgramId = eagan.Id,
            NpiProgram = eagan,
            Version = "2.5.0",
            Stage = FirmwareReleaseStage.T3_Released,
            ReleasedAt = new DateTime(2026, 1, 10),
            ReleasedBy = "Firmware Team",
            ChangeLog = new List<string> { "Added advanced audio commands" },
            Commands = eagan250Cmds
        });

        var eagan260Cmds = CreateUniqueCommands(20, ref cmdCodeCounter);
        allCommands.AddRange(eagan260Cmds);
        versions.Add(new FirmwareVersion
        {
            Id = Guid.NewGuid(),
            NpiProgramId = eagan.Id,
            NpiProgram = eagan,
            Version = "2.6.0",
            Stage = FirmwareReleaseStage.T2_Beta,
            ReleasedAt = new DateTime(2026, 2, 1),
            ReleasedBy = "Firmware Team",
            ChangeLog = new List<string> { "Beta: Streaming audio commands" },
            Commands = eagan260Cmds
        });

        // Elko (Next Gen)
        var elko300Cmds = CreateUniqueCommands(24, ref cmdCodeCounter);
        allCommands.AddRange(elko300Cmds);
        versions.Add(new FirmwareVersion
        {
            Id = Guid.NewGuid(),
            NpiProgramId = elko.Id,
            NpiProgram = elko,
            Version = "3.0.0",
            Stage = FirmwareReleaseStage.T0_Test,
            ReleasedAt = new DateTime(2026, 2, 2),
            ReleasedBy = "R&D Team",
            ChangeLog = new List<string> { "Experimental: New command protocol" },
            Commands = elko300Cmds
        });

        return versions;
    }

    private List<ParameterDefinition> CreateUniqueParameters(int count, ref int startId)
    {
        var parameters = new List<ParameterDefinition>();
        var paramNames = new[] { "Gain", "Threshold", "AttackTime", "ReleaseTime", "Directionality", 
                                  "NoiseGate", "Compression", "Frequency", "Bandwidth", "Level" };
        
        for (int i = 0; i < count; i++)
        {
            parameters.Add(new ParameterDefinition
            {
                Id = startId++,
                Name = $"PARAM_{paramNames[i % paramNames.Length]}_{i / paramNames.Length}",
                DataType = "int",
                MinValue = 0,
                MaxValue = 100,
                DefaultValue = 50,
                Description = $"Sample parameter {i}",
                IsDeprecated = false
            });
        }
        
        return parameters;
    }

    private List<CommandDefinition> CreateUniqueCommands(int count, ref byte startCode)
    {
        var commands = new List<CommandDefinition>();
        var cmdNames = new[] { "Initialize", "SetGain", "SetMode", "StartStream", "StopStream", 
                               "GetStatus", "Reset", "Configure" };
        
        for (int i = 0; i < count; i++)
        {
            commands.Add(new CommandDefinition
            {
                CommandCode = startCode++,
                Name = $"CMD_{cmdNames[i % cmdNames.Length]}_{i / cmdNames.Length}",
                Description = $"Sample command {i}",
                ExpectedResponseCode = 0x00,
                IsDeprecated = false,
                Parameters = new List<ParameterDefinition>()
            });
        }
        
        return commands;
    }

    private List<Feature> InitializeFeatures()
    {
        return new List<Feature>
        {
            // Directionality Feature - Complex multi-parameter directional audio processing
            new Feature
            {
                Id = Guid.NewGuid(),
                Name = "Adaptive Directionality",
                Description = "Spatial awareness directionality with 4 memory settings (Omnidirectional, Fixed, Adaptive, Ultra). Controls beam patterns, switching thresholds, and spatial processing for optimal speech intelligibility in noise.",
                Parameters = new List<ParameterValue>
                {
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1004, ParameterName = "DirectionalityMode", Value = 2, Notes = "0=Off, 1=Fixed, 2=Adaptive, 3=Ultra" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1005, ParameterName = "DirectionalityStrength", Value = 75, Notes = "Beam narrowness 0-100" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1006, ParameterName = "SpatialAwarenessLevel", Value = 60, Notes = "Environmental awareness preservation" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1007, ParameterName = "SwitchingThresholdDb", Value = 50, Notes = "SNR threshold for adaptive switching" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1008, ParameterName = "BeamPatternFront", Value = 85, Notes = "Front lobe gain" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1009, ParameterName = "BeamPatternRear", Value = 15, Notes = "Rear attenuation" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1010, ParameterName = "AdaptationSpeed", Value = 40, Notes = "Speed of directional adaptation (0=slow, 100=fast)" }
                },
                CreatedAt = DateTime.Now.AddDays(-10),
                ModifiedAt = DateTime.Now.AddDays(-2),
                Owner = "john.doe@company.com"
            },
            
            // Noise Reduction Feature - Advanced spectral noise suppression
            new Feature
            {
                Id = Guid.NewGuid(),
                Name = "Advanced Noise Reduction",
                Description = "Multi-band spectral noise reduction with speech preservation. Uses 6 channels with independent gain control, modulation detection, and adaptive thresholds for each frequency band.",
                Parameters = new List<ParameterValue>
                {
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1050, ParameterName = "NoiseReductionStrength", Value = 65, Notes = "Overall NR aggressiveness 0-100" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1051, ParameterName = "SpeechPreservation", Value = 80, Notes = "Speech protection level" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1052, ParameterName = "ModulationDetectionThreshold", Value = 45, Notes = "Threshold for speech vs noise classification" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1053, ParameterName = "Channel1Gain", Value = 50, Notes = "250-500 Hz band" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1054, ParameterName = "Channel2Gain", Value = 55, Notes = "500-1k Hz band" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1055, ParameterName = "Channel3Gain", Value = 60, Notes = "1k-2k Hz band" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1056, ParameterName = "Channel4Gain", Value = 65, Notes = "2k-4k Hz band" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1057, ParameterName = "Channel5Gain", Value = 60, Notes = "4k-6k Hz band" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1058, ParameterName = "Channel6Gain", Value = 55, Notes = "6k-8k Hz band" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1059, ParameterName = "AttackTimeMs", Value = 20, Notes = "Attack time in milliseconds" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1060, ParameterName = "ReleaseTimeMs", Value = 200, Notes = "Release time in milliseconds" }
                },
                CreatedAt = DateTime.Now.AddDays(-8),
                ModifiedAt = DateTime.Now.AddDays(-1),
                Owner = "john.doe@company.com"
            },
            
            // DFU Feature - Device Firmware Update workflow with state machine
            new Feature
            {
                Id = Guid.NewGuid(),
                Name = "Device Firmware Update (DFU)",
                Description = "Complete firmware update state machine with failsafe operation, background download capability, and automatic rollback on failure. Workflow: Check Status → Prepare → Download Image Package → Validate Image → Download Script Package → Validate Script → Update Firmware → Acknowledge → Verify or Rollback. State persists across reboots. Supports pause/resume during download.",
                Commands = new List<PiccoloCommand>
                {
                    // Step 1: Check current DFU state
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x06, CommandName = "DfuGetStatus", ExecutionOrder = 1, Payload = new List<byte> { 0x06, 0x0A }, Notes = "Query current DFU agent state (Ready, Downloading, Validated, Update Complete, etc.)" },
                    
                    // Step 2: Reset if needed and prepare for download
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x10, CommandName = "DfuRequest_Reset", ExecutionOrder = 2, Payload = new List<byte> { 0x10, 0x0A, 0x00 }, Notes = "Reset DFU agent to Ready state if currently in error or complete state" },
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x10, CommandName = "DfuRequest_Prepare", ExecutionOrder = 3, Payload = new List<byte> { 0x10, 0x0A, 0x01 }, Notes = "Prepare for download, transition from Ready → Ready To Download" },
                    
                    // Step 3: Download image package in parcels (background operation)
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x0B, CommandName = "DfuWriteParcel", ExecutionOrder = 4, Payload = new List<byte> { 0x0B, 0x0A, 0x00, 0x00, 0x40, 0x00 }, Notes = "Write 64-byte parcels of firmware image (loop until complete)" },
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x06, CommandName = "DfuGetStatus_CheckDownload", ExecutionOrder = 5, Payload = new List<byte> { 0x06, 0x0A }, Notes = "Check progress during download (can pause/resume)" },
                    
                    // Step 4: Validate downloaded image package
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x10, CommandName = "DfuRequest_ValidateImage", ExecutionOrder = 6, Payload = new List<byte> { 0x10, 0x0A, 0x03 }, Notes = "Validate image package CRC/hash, transition Downloading → Image Package Validated" },
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x06, CommandName = "DfuGetStatus_ValidationResult", ExecutionOrder = 7, Payload = new List<byte> { 0x06, 0x0A }, Notes = "Check validation result (success or failure)" },
                    
                    // Step 5: Download script package for parameter preservation
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x0B, CommandName = "DfuWriteParcel_Script", ExecutionOrder = 8, Payload = new List<byte> { 0x0B, 0x0A, 0x01, 0x00, 0x20, 0x00 }, Notes = "Write script package parcels for LPI parameter preservation" },
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x10, CommandName = "DfuRequest_ValidateScript", ExecutionOrder = 9, Payload = new List<byte> { 0x10, 0x0A, 0x05 }, Notes = "Validate script package" },
                    
                    // Step 6: Initiate firmware update (device will reboot)
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x10, CommandName = "DfuRequest_UpdateFirmware", ExecutionOrder = 10, Payload = new List<byte> { 0x10, 0x0A, 0x06 }, Notes = "Initiate update (30-90 sec switchover), device reboots to new firmware" },
                    
                    // Step 7: After reboot, reconnect and check status
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x06, CommandName = "DfuGetStatus_AfterReboot", ExecutionOrder = 11, Payload = new List<byte> { 0x06, 0x0A }, Notes = "Check status after reboot (Reconnecting Target → Update Complete or Update Failed)" },
                    
                    // Step 8: Acknowledge successful update OR initiate rollback
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x10, CommandName = "DfuRequest_AcknowledgeUpdate", ExecutionOrder = 12, Payload = new List<byte> { 0x10, 0x0A, 0x07 }, Notes = "Acknowledge successful update, finalize new firmware" },
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x10, CommandName = "DfuRequest_Rollback", ExecutionOrder = 13, Payload = new List<byte> { 0x10, 0x0A, 0x08 }, Notes = "Rollback to previous firmware if update failed (failsafe)" },
                    
                    // Additional utility commands
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x09, CommandName = "DfuGetPackageInfo", ExecutionOrder = 14, Payload = new List<byte> { 0x09, 0x0A }, Notes = "Get current/downloaded package metadata (version, size, type)" },
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x0E, CommandName = "DfuHashNvmBlock", ExecutionOrder = 15, Payload = new List<byte> { 0x0E, 0x0A, 0x00, 0x10 }, Notes = "Hash verification of downloaded blocks in NVM" }
                },
                Parameters = new List<ParameterValue>
                {
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1100, ParameterName = "DfuMode", Value = 0, Notes = "0=Background DFU (slow, maintains function), 1=Foreground DFU (fast, minimal function)" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1101, ParameterName = "ParcelSizeBytes", Value = 64, Notes = "Parcel size for chunked download (16-256 bytes)" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1102, ParameterName = "ValidationMethod", Value = 1, Notes = "0=None, 1=CRC32, 2=SHA256 hash" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1103, ParameterName = "EnableAutoRollback", Value = 1, Notes = "0=Manual rollback only, 1=Auto rollback on boot failure" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1104, ParameterName = "PreserveSettings", Value = 1, Notes = "0=Reset to defaults, 1=Preserve LPI parameters via script" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1105, ParameterName = "MaxSwitchoverTimeSec", Value = 60, Notes = "Maximum downtime during firmware switchover (30-90 sec)" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1106, ParameterName = "EnablePauseResume", Value = 1, Notes = "0=Disabled, 1=Allow pause/resume on power cycle" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1107, ParameterName = "DeliverySource", Value = 0, Notes = "0=Mobile app, 1=Fitting software, 2=Cloud direct" }
                },
                CreatedAt = DateTime.Now.AddDays(-15),
                ModifiedAt = DateTime.Now.AddDays(-3),
                Owner = "firmware.team@company.com"
            },
            
            // Equalizer Feature
            new Feature
            {
                Id = Guid.NewGuid(),
                Name = "8-Band Graphic Equalizer",
                Description = "Programmable 8-band graphic equalizer with individual gain control per frequency band. Provides precise frequency shaping for audiologist customization.",
                Parameters = new List<ParameterValue>
                {
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1150, ParameterName = "EqBand250HzGain", Value = 50, Notes = "-20dB to +20dB (50=0dB)" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1151, ParameterName = "EqBand500HzGain", Value = 52, Notes = "-20dB to +20dB" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1152, ParameterName = "EqBand1kHzGain", Value = 55, Notes = "-20dB to +20dB" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1153, ParameterName = "EqBand2kHzGain", Value = 60, Notes = "-20dB to +20dB" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1154, ParameterName = "EqBand3kHzGain", Value = 62, Notes = "-20dB to +20dB" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1155, ParameterName = "EqBand4kHzGain", Value = 58, Notes = "-20dB to +20dB" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1156, ParameterName = "EqBand6kHzGain", Value = 53, Notes = "-20dB to +20dB" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1157, ParameterName = "EqBand8kHzGain", Value = 48, Notes = "-20dB to +20dB" }
                },
                CreatedAt = DateTime.Now.AddDays(-20),
                ModifiedAt = DateTime.Now.AddDays(-5),
                Owner = "john.doe@company.com"
            },
            
            // Compression Feature
            new Feature
            {
                Id = Guid.NewGuid(),
                Name = "Slow Compression (WDRC)",
                Description = "Wide Dynamic Range Compression with configurable attack/release times, compression ratios, and knee points. Provides comfortable listening across varying input levels.",
                Parameters = new List<ParameterValue>
                {
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1200, ParameterName = "CompressionRatio", Value = 30, Notes = "1:1 to 10:1 (30 = 3:1)" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1201, ParameterName = "KneePointDb", Value = 55, Notes = "Compression threshold in dB SPL" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1202, ParameterName = "AttackTimeMs", Value = 5, Notes = "Attack time 1-50ms" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1203, ParameterName = "ReleaseTimeMs", Value = 50, Notes = "Release time 10-500ms" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1204, ParameterName = "OutputLimitingDb", Value = 105, Notes = "Maximum output level" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1205, ParameterName = "ExpansionRatio", Value = 15, Notes = "Low-level expansion ratio (15 = 1.5:1)" }
                },
                CreatedAt = DateTime.Now.AddDays(-12),
                ModifiedAt = DateTime.Now.AddDays(-4),
                Owner = "jane.smith@company.com"
            },
            
            // Feedback Cancellation
            new Feature
            {
                Id = Guid.NewGuid(),
                Name = "Adaptive Feedback Cancellation",
                Description = "Real-time acoustic feedback suppression using adaptive filtering. Monitors feedback paths and applies inverse filtering to prevent whistling while preserving gain.",
                Parameters = new List<ParameterValue>
                {
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1250, ParameterName = "FeedbackCancellationEnable", Value = 1, Notes = "0=Disabled, 1=Enabled" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1251, ParameterName = "AdaptationRate", Value = 50, Notes = "Filter adaptation speed 0-100" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1252, ParameterName = "MaxGainReductionDb", Value = 12, Notes = "Maximum gain reduction to prevent feedback" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1253, ParameterName = "DetectionThresholdDb", Value = 3, Notes = "Feedback detection sensitivity" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1254, ParameterName = "FilterLength", Value = 64, Notes = "Adaptive filter taps (32-128)" }
                },
                CreatedAt = DateTime.Now.AddDays(-25),
                ModifiedAt = DateTime.Now.AddDays(-10),
                Owner = "bob.johnson@company.com"
            },
            
            // Streaming Audio
            new Feature
            {
                Id = Guid.NewGuid(),
                Name = "Bluetooth Audio Streaming",
                Description = "Wireless audio streaming from smartphones and devices. Includes audio routing, latency compensation, and volume control for streamed content.",
                Commands = new List<PiccoloCommand>
                {
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x20, CommandName = "StreamStart", ExecutionOrder = 1, Payload = new List<byte> { 0x20, 0x01 } },
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x21, CommandName = "StreamSetVolume", ExecutionOrder = 2, Payload = new List<byte> { 0x21, 0x50 } },
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x22, CommandName = "StreamStop", ExecutionOrder = 3, Payload = new List<byte> { 0x22 } }
                },
                Parameters = new List<ParameterValue>
                {
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1300, ParameterName = "StreamingVolume", Value = 70, Notes = "Streaming audio volume 0-100" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1301, ParameterName = "LatencyCompensationMs", Value = 40, Notes = "Audio/video sync delay" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1302, ParameterName = "MixWithMicrophoneDb", Value = 6, Notes = "Mix ratio: streamed vs mic audio" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1303, ParameterName = "CodecPreference", Value = 1, Notes = "0=SBC, 1=AAC, 2=aptX" }
                },
                CreatedAt = DateTime.Now.AddDays(-18),
                ModifiedAt = DateTime.Now.AddDays(-7),
                Owner = "alice.williams@company.com"
            }
        };
    }

    private List<FeatureCollection> InitializeCollections(
        List<NpiProgram> npiPrograms,
        List<FirmwareVersion> lpiVersions,
        List<FirmwareVersion> piccoloVersions,
        List<Feature> features)
    {
        var eagan = npiPrograms.First(p => p.Name == "Eagan");
        var edina = npiPrograms.First(p => p.Name == "Edina");
        var elko = npiPrograms.First(p => p.Name == "Elko");
        
        var eagan321Lpi = lpiVersions.First(v => v.Version == "3.2.1" && v.NpiProgram?.Name == "Eagan");
        var eagan250Piccolo = piccoloVersions.First(v => v.Version == "2.5.0" && v.NpiProgram?.Name == "Eagan");
        
        var eagan330Lpi = lpiVersions.First(v => v.Version == "3.3.0");
        var eagan260Piccolo = piccoloVersions.First(v => v.Version == "2.6.0");
        
        var edina280Lpi = lpiVersions.First(v => v.Version == "2.8.0");
        var edina190Piccolo = piccoloVersions.First(v => v.Version == "1.9.0");
        
        var elko400Lpi = lpiVersions.First(v => v.Version == "4.0.0");
        var elko300Piccolo = piccoloVersions.First(v => v.Version == "3.0.0");

        return new List<FeatureCollection>
        {
            new FeatureCollection
            {
                Id = Guid.NewGuid(),
                Name = "Eagan Advanced Directionality",
                Description = "Feature set for Eagan program with enhanced directionality",
                NpiProgramId = eagan.Id,
                TargetProgram = eagan,
                RequiredLpiVersionId = eagan321Lpi.Id,
                RequiredLpiVersion = eagan321Lpi,
                RequiredPiccoloVersionId = eagan250Piccolo.Id,
                RequiredPiccoloVersion = eagan250Piccolo,
                Features = features.Take(3).ToList(),
                Visibility = CollectionVisibility.Team,
                Owner = "john.doe@company.com",
                SharedWith = new List<string> { "Firmware Team", "QA Team" },
                CreatedAt = DateTime.Now.AddDays(-5),
                ModifiedAt = DateTime.Now,
                IsLocked = false,
                Tags = new List<string> { "eagan", "directionality", "production" }
            },
            new FeatureCollection
            {
                Id = Guid.NewGuid(),
                Name = "Eagan Beta Features",
                Description = "Testing new features in beta firmware",
                NpiProgramId = eagan.Id,
                TargetProgram = eagan,
                RequiredLpiVersionId = eagan330Lpi.Id,
                RequiredLpiVersion = eagan330Lpi,
                RequiredPiccoloVersionId = eagan260Piccolo.Id,
                RequiredPiccoloVersion = eagan260Piccolo,
                Features = features.Skip(1).Take(2).ToList(),
                Visibility = CollectionVisibility.Private,
                Owner = "john.doe@company.com",
                SharedWith = new List<string>(),
                CreatedAt = DateTime.Now.AddDays(-2),
                ModifiedAt = DateTime.Now.AddDays(-1),
                IsLocked = false,
                Tags = new List<string> { "eagan", "beta", "testing" }
            },
            new FeatureCollection
            {
                Id = Guid.NewGuid(),
                Name = "Edina Legacy Set",
                Description = "Legacy features for Gen2 hardware support",
                NpiProgramId = edina.Id,
                TargetProgram = edina,
                RequiredLpiVersionId = edina280Lpi.Id,
                RequiredLpiVersion = edina280Lpi,
                RequiredPiccoloVersionId = edina190Piccolo.Id,
                RequiredPiccoloVersion = edina190Piccolo,
                Features = features.Take(1).ToList(),
                Visibility = CollectionVisibility.Team,
                Owner = "support@company.com",
                SharedWith = new List<string> { "Support Team" },
                CreatedAt = DateTime.Now.AddDays(-60),
                ModifiedAt = DateTime.Now.AddDays(-30),
                IsLocked = true,
                LockedReason = "Legacy support - no modifications allowed",
                Tags = new List<string> { "edina", "legacy", "deprecated" }
            },
            new FeatureCollection
            {
                Id = Guid.NewGuid(),
                Name = "Elko Next Gen (T0)",
                Description = "Experimental features for next generation",
                NpiProgramId = elko.Id,
                TargetProgram = elko,
                RequiredLpiVersionId = elko400Lpi.Id,
                RequiredLpiVersion = elko400Lpi,
                RequiredPiccoloVersionId = elko300Piccolo.Id,
                RequiredPiccoloVersion = elko300Piccolo,
                Features = new List<Feature>(),
                Visibility = CollectionVisibility.Private,
                Owner = "john.doe@company.com",
                SharedWith = new List<string>(),
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
                IsLocked = false,
                Tags = new List<string> { "elko", "experimental", "unstable" }
            }
        };
    }

    // Public methods for accessing data
    public List<NpiProgram> GetNpiPrograms() => _context.NpiPrograms.ToList();
    public NpiProgram? GetNpiProgram(Guid id) => _context.NpiPrograms.FirstOrDefault(p => p.Id == id);
    
    public List<FirmwareVersion> GetLpiVersions(Guid? npiProgramId = null)
    {
        var query = _context.FirmwareVersions
            .Include(v => v.NpiProgram)
            .Where(v => v.Parameters.Any()); // LPI versions have parameters
        
        if (npiProgramId.HasValue)
            query = query.Where(v => v.NpiProgramId == npiProgramId.Value);
        
        return query.ToList();
    }
    
    public List<FirmwareVersion> GetPiccoloVersions(Guid? npiProgramId = null)
    {
        var query = _context.FirmwareVersions
            .Include(v => v.NpiProgram)
            .Where(v => v.Commands.Any()); // PICCOLO versions have commands
        
        if (npiProgramId.HasValue)
            query = query.Where(v => v.NpiProgramId == npiProgramId.Value);
        
        return query.ToList();
    }
    
    public FirmwareVersion? GetLpiVersion(Guid id) => _context.FirmwareVersions
        .Include(v => v.NpiProgram)
        .Include(v => v.Parameters)
        .FirstOrDefault(v => v.Id == id);
    
    public FirmwareVersion? GetPiccoloVersion(Guid id) => _context.FirmwareVersions
        .Include(v => v.NpiProgram)
        .Include(v => v.Commands)
        .FirstOrDefault(v => v.Id == id);
    
    public List<Feature> GetFeatures() => _context.Features
        .Include(f => f.Parameters)
        .Include(f => f.Commands)
        .ToList();
    public Feature? GetFeature(Guid id) => _context.Features
        .Include(f => f.Parameters)
        .Include(f => f.Commands)
        .FirstOrDefault(f => f.Id == id);
    
    public List<Feature> GetAllFeatures() => _context.Features
        .Include(f => f.Parameters)
        .Include(f => f.Commands)
        .ToList();
    
    public List<FeatureCollection> GetCollections() => _context.FeatureCollections
        .Include(c => c.TargetProgram)
        .Include(c => c.RequiredLpiVersion)
        .Include(c => c.RequiredPiccoloVersion)
        .Include(c => c.Features)
        .ToList();
    
    public FeatureCollection? GetCollection(Guid id) => _context.FeatureCollections
        .Include(c => c.TargetProgram)
        .Include(c => c.RequiredLpiVersion)
        .Include(c => c.RequiredPiccoloVersion)
        .Include(c => c.Features)
        .FirstOrDefault(c => c.Id == id);
    
    public void AddCollection(FeatureCollection collection)
    {
        _context.FeatureCollections.Add(collection);
        _context.SaveChanges();
    }
    
    public void UpdateCollection(FeatureCollection collection)
    {
        _context.FeatureCollections.Update(collection);
        _context.SaveChanges();
    }

    public OperationalFlow GetDfuFlow()
    {
        // Find the "Eagan Advanced Directionality" collection to link to
        var contextCollection = _context.FeatureCollections.FirstOrDefault(c => c.Name.Contains("Eagan Advanced Directionality")) 
            ?? _context.FeatureCollections.First();
        var linkedFeature = _context.Features.First();

        return new OperationalFlow
        {
            Id = Guid.NewGuid(),
            CollectionId = contextCollection.Id,
            FeatureId = linkedFeature.Id,
            Name = "Device Firmware Update (DFU) Sequence",
            Description = "Standard over-the-air firmware update process with safety checks and rollback.",
            Operations = new List<FlowOperation>
            {
                new FlowOperation
                {
                    Id = Guid.NewGuid(),
                    Name = "Operation 1: Pre-Flight Safety Checks",
                    Type = "Sequential",
                    Steps = new List<FlowStep>
                    {
                        new FlowStep
                        {
                            Id = Guid.NewGuid(),
                            StepNumber = "1.1",
                            Title = "Check Battery Level",
                            Description = "Ensure device has enough power for the update process.",
                            Type = "Command",
                            Attributes = new Dictionary<string, string>
                            {
                                { "Command", "GET_BATTERY_STATUS (0x12)" },
                                { "Timeout", "500ms" },
                                { "Store Result", "var_bat_level" }
                            }
                        },
                        new FlowStep
                        {
                            Id = Guid.NewGuid(),
                            StepNumber = "1.2",
                            Title = "Validate Power Requirement",
                            Description = "Check if battery level is sufficient (> 50%).",
                            Type = "Decision",
                            Attributes = new Dictionary<string, string>
                            {
                                { "Condition", "var_bat_level >= 50" }
                            },
                            Branches = new List<FlowBranch>
                            {
                                new FlowBranch { Label = "Pass", Description = "Battery > 50%", Action = "Continue to Next Operation" },
                                new FlowBranch { Label = "Fail", Description = "Battery < 50%", Action = "Abort Workflow", IsErrorPath = true }
                            }
                        }
                    }
                },
                new FlowOperation
                {
                    Id = Guid.NewGuid(),
                    Name = "Operation 2: DFU Mode Transition",
                    Type = "Sequential",
                    Steps = new List<FlowStep>
                    {
                        new FlowStep
                        {
                            Id = Guid.NewGuid(),
                            StepNumber = "2.1",
                            Title = "Set DFU Mode",
                            Description = "Command device to enter bootloader mode.",
                            Type = "Command",
                            Attributes = new Dictionary<string, string>
                            {
                                { "Command", "SET_SYS_MODE (0xA0)" },
                                { "Payload", "0x01 (DFU)" }
                            }
                        },
                        new FlowStep
                        {
                            Id = Guid.NewGuid(),
                            StepNumber = "2.2",
                            Title = "Wait for Reboot",
                            Description = "Allow device time to restart in bootloader.",
                            Type = "Delay",
                            Attributes = new Dictionary<string, string>
                            {
                                { "Duration", "2000 ms" },
                                { "Reason", "Device Reboot" }
                            }
                        },
                        new FlowStep
                        {
                            Id = Guid.NewGuid(),
                            StepNumber = "2.3",
                            Title = "Verify Bootloader State",
                            Description = "Ping device to confirm it is reachable and in DFU mode.",
                            Type = "Decision",
                            Attributes = new Dictionary<string, string>
                            {
                                { "Command", "PING (0x00)" },
                                { "Retries", "3" }
                            },
                             Branches = new List<FlowBranch>
                            {
                                new FlowBranch { Label = "Ack", Description = "Device Responded", Action = "Continue" },
                                new FlowBranch { Label = "Timeout", Description = "No Response", Action = "Retry (Max 3)", IsErrorPath = true }
                            }
                        }
                    }
                },
                new FlowOperation
                {
                    Id = Guid.NewGuid(),
                    Name = "Operation 3: Image Transfer Loop",
                    Type = "Loop",
                    Steps = new List<FlowStep>
                    {
                        new FlowStep
                        {
                            Id = Guid.NewGuid(),
                            StepNumber = "3.1",
                            Title = "Calculate Transfer Chunks",
                            Description = "Split binary into 256-byte packets.",
                            Type = "Calculation",
                            Attributes = new Dictionary<string, string>
                            {
                                { "Input", "Firmware Binary" },
                                { "Chunk Size", "256 bytes" },
                                { "Set", "var_total_packets" }
                            }
                        },
                         new FlowStep
                        {
                            Id = Guid.NewGuid(),
                            StepNumber = "3.2",
                            Title = "Send Packet",
                            Description = "Transmit current data packet.",
                            Type = "Command",
                            Attributes = new Dictionary<string, string>
                            {
                                { "Command", "DFU_WRITE (0xB1)" },
                                { "Payload", "[Packet Index] [Data...]" }
                            }
                        },
                         new FlowStep
                        {
                            Id = Guid.NewGuid(),
                            StepNumber = "3.3",
                            Title = "Verify Packet Write",
                            Description = "Check device acknowledgment for the packet.",
                            Type = "Decision",
                            Attributes = new Dictionary<string, string>(),
                             Branches = new List<FlowBranch>
                            {
                                new FlowBranch { Label = "Ack", Description = "Write OK", Action = "Next Packet" },
                                new FlowBranch { Label = "Nack", Description = "Write Failed", Action = "Retry Packet (Max 5)", IsErrorPath = true }
                            }
                        }
                    }
                },
                new FlowOperation
                {
                    Id = Guid.NewGuid(),
                    Name = "Operation 4: Validation & Activation",
                    Type = "Sequential",
                    Steps = new List<FlowStep>
                    {
                        new FlowStep
                        {
                            Id = Guid.NewGuid(),
                            StepNumber = "4.1",
                            Title = "Validate Full Image",
                            Description = "Request CRC32 checksum of uploaded image.",
                            Type = "Command",
                            Attributes = new Dictionary<string, string>
                            {
                                { "Command", "DFU_VALIDATE (0xB2)" },
                                { "Expected CRC", "0xA3F192..." }
                            }
                        },
                        new FlowStep
                        {
                            Id = Guid.NewGuid(),
                            StepNumber = "4.2",
                            Title = "Validation Check",
                            Description = "Compare device CRC with calculated CRC.",
                            Type = "Decision",
                            Branches = new List<FlowBranch>
                            {
                                new FlowBranch { Label = "Match", Description = "CRC Valid", Action = "Continue" },
                                new FlowBranch { Label = "Mismatch", Description = "Corrupt Image", Action = "Abort & Rollback", IsErrorPath = true }
                            }
                        },
                        new FlowStep
                        {
                            Id = Guid.NewGuid(),
                            StepNumber = "4.3",
                            Title = "Apply & Boot",
                            Description = "Mark valid and restart device to new firmware.",
                            Type = "Command",
                            Attributes = new Dictionary<string, string>
                            {
                                { "Command", "SYS_RESET (0x01)" }
                            }
                        }
                    }
                }
            }
        };
    }
}
