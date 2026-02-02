using WebApp.Models;

namespace WebApp.Services;

public class FirmwareDataService
{
    private readonly List<NpiProgram> _npiPrograms;
    private readonly List<FirmwareVersion> _lpiVersions;
    private readonly List<FirmwareVersion> _piccoloVersions;
    private readonly List<Feature> _features;
    private readonly List<FeatureCollection> _collections;

    public FirmwareDataService()
    {
        _npiPrograms = InitializeNpiPrograms();
        _lpiVersions = InitializeLpiVersions();
        _piccoloVersions = InitializePiccoloVersions();
        _features = InitializeFeatures();
        _collections = InitializeCollections();
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

    private List<FirmwareVersion> InitializeLpiVersions()
    {
        var edina = _npiPrograms.First(p => p.Name == "Edina");
        var eagan = _npiPrograms.First(p => p.Name == "Eagan");
        var elko = _npiPrograms.First(p => p.Name == "Elko");

        return new List<FirmwareVersion>
        {
            // Edina (Legacy)
            new FirmwareVersion
            {
                Id = Guid.NewGuid(),
                NpiProgramId = edina.Id,
                NpiProgram = edina,
                Version = "2.8.0",
                Stage = FirmwareReleaseStage.T3_Released,
                ReleasedAt = new DateTime(2024, 11, 1),
                ReleasedBy = "Legacy Team",
                ChangeLog = new List<string> { "Final Edina release" },
                Parameters = CreateSampleParameters(20)
            },
            // Eagan
            new FirmwareVersion
            {
                Id = Guid.NewGuid(),
                NpiProgramId = eagan.Id,
                NpiProgram = eagan,
                Version = "3.1.0",
                Stage = FirmwareReleaseStage.T3_Released,
                ReleasedAt = new DateTime(2025, 12, 1),
                ReleasedBy = "Firmware Team",
                ChangeLog = new List<string> { "Eagan initial release", "24 new parameters" },
                Parameters = CreateSampleParameters(24)
            },
            new FirmwareVersion
            {
                Id = Guid.NewGuid(),
                NpiProgramId = eagan.Id,
                NpiProgram = eagan,
                Version = "3.2.0",
                Stage = FirmwareReleaseStage.T3_Released,
                ReleasedAt = new DateTime(2026, 1, 15),
                ReleasedBy = "Firmware Team",
                ChangeLog = new List<string> { "Added 2 new directionality parameters" },
                Parameters = CreateSampleParameters(26)
            },
            new FirmwareVersion
            {
                Id = Guid.NewGuid(),
                NpiProgramId = eagan.Id,
                NpiProgram = eagan,
                Version = "3.2.1",
                Stage = FirmwareReleaseStage.T3_Released,
                ReleasedAt = new DateTime(2026, 1, 28),
                ReleasedBy = "Firmware Team",
                ChangeLog = new List<string> { "Bug fixes only", "Fixed gain calculation" },
                Parameters = CreateSampleParameters(26)
            },
            new FirmwareVersion
            {
                Id = Guid.NewGuid(),
                NpiProgramId = eagan.Id,
                NpiProgram = eagan,
                Version = "3.3.0",
                Stage = FirmwareReleaseStage.T2_Beta,
                ReleasedAt = new DateTime(2026, 2, 1),
                ReleasedBy = "Firmware Team",
                ChangeLog = new List<string> { "Beta: New noise gate feature" },
                Parameters = CreateSampleParameters(28)
            },
            // Elko (Next Gen)
            new FirmwareVersion
            {
                Id = Guid.NewGuid(),
                NpiProgramId = elko.Id,
                NpiProgram = elko,
                Version = "4.0.0",
                Stage = FirmwareReleaseStage.T0_Test,
                ReleasedAt = new DateTime(2026, 2, 2),
                ReleasedBy = "R&D Team",
                ChangeLog = new List<string> { "Experimental: Complete redesign", "IDs may change" },
                Parameters = CreateSampleParameters(32)
            }
        };
    }

    private List<FirmwareVersion> InitializePiccoloVersions()
    {
        var edina = _npiPrograms.First(p => p.Name == "Edina");
        var eagan = _npiPrograms.First(p => p.Name == "Eagan");
        var elko = _npiPrograms.First(p => p.Name == "Elko");

        return new List<FirmwareVersion>
        {
            // Edina (Legacy)
            new FirmwareVersion
            {
                Id = Guid.NewGuid(),
                NpiProgramId = edina.Id,
                NpiProgram = edina,
                Version = "1.9.0",
                Stage = FirmwareReleaseStage.T3_Released,
                ReleasedAt = new DateTime(2024, 10, 20),
                ReleasedBy = "Legacy Team",
                ChangeLog = new List<string> { "Final Edina PICCOLO release" },
                Commands = CreateSampleCommands(14)
            },
            // Eagan
            new FirmwareVersion
            {
                Id = Guid.NewGuid(),
                NpiProgramId = eagan.Id,
                NpiProgram = eagan,
                Version = "2.4.0",
                Stage = FirmwareReleaseStage.T3_Released,
                ReleasedAt = new DateTime(2025, 11, 20),
                ReleasedBy = "Firmware Team",
                ChangeLog = new List<string> { "Eagan initial PICCOLO release" },
                Commands = CreateSampleCommands(16)
            },
            new FirmwareVersion
            {
                Id = Guid.NewGuid(),
                NpiProgramId = eagan.Id,
                NpiProgram = eagan,
                Version = "2.5.0",
                Stage = FirmwareReleaseStage.T3_Released,
                ReleasedAt = new DateTime(2026, 1, 10),
                ReleasedBy = "Firmware Team",
                ChangeLog = new List<string> { "Added advanced audio commands" },
                Commands = CreateSampleCommands(18)
            },
            new FirmwareVersion
            {
                Id = Guid.NewGuid(),
                NpiProgramId = eagan.Id,
                NpiProgram = eagan,
                Version = "2.6.0",
                Stage = FirmwareReleaseStage.T2_Beta,
                ReleasedAt = new DateTime(2026, 2, 1),
                ReleasedBy = "Firmware Team",
                ChangeLog = new List<string> { "Beta: Streaming audio commands" },
                Commands = CreateSampleCommands(20)
            },
            // Elko (Next Gen)
            new FirmwareVersion
            {
                Id = Guid.NewGuid(),
                NpiProgramId = elko.Id,
                NpiProgram = elko,
                Version = "3.0.0",
                Stage = FirmwareReleaseStage.T0_Test,
                ReleasedAt = new DateTime(2026, 2, 2),
                ReleasedBy = "R&D Team",
                ChangeLog = new List<string> { "Experimental: New command protocol" },
                Commands = CreateSampleCommands(24)
            }
        };
    }

    private List<ParameterDefinition> CreateSampleParameters(int count)
    {
        var parameters = new List<ParameterDefinition>();
        var paramNames = new[] { "Gain", "Threshold", "AttackTime", "ReleaseTime", "Directionality", 
                                  "NoiseGate", "Compression", "Frequency", "Bandwidth", "Level" };
        
        for (int i = 0; i < count; i++)
        {
            parameters.Add(new ParameterDefinition
            {
                Id = 1000 + i,
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

    private List<CommandDefinition> CreateSampleCommands(int count)
    {
        var commands = new List<CommandDefinition>();
        var cmdNames = new[] { "Initialize", "SetGain", "SetMode", "StartStream", "StopStream", 
                               "GetStatus", "Reset", "Configure" };
        
        for (int i = 0; i < count; i++)
        {
            commands.Add(new CommandDefinition
            {
                CommandCode = (byte)(0x10 + i),
                Name = $"CMD_{cmdNames[i % cmdNames.Length]}_{i / cmdNames.Length}",
                Description = $"Sample command {i}",
                Parameters = new List<ParameterDefinition>(),
                ExpectedResponseCode = (byte)(0x80 + i),
                IsDeprecated = false
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
            
            // DFU Feature - Device Firmware Update workflow
            new Feature
            {
                Id = Guid.NewGuid(),
                Name = "Device Firmware Update (DFU)",
                Description = "Complete firmware update workflow including package validation, parcel-based download, CRC verification, and device reboot. Supports resume after interruption and rollback to previous firmware.",
                Commands = new List<PiccoloCommand>
                {
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x0A, CommandName = "DfuGetStatus", ExecutionOrder = 1, Payload = new List<byte> { 0x0A, 0x01 } },
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x0B, CommandName = "DfuGetPackageInfo", ExecutionOrder = 2, Payload = new List<byte> { 0x0B, 0x01 } },
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x0C, CommandName = "DfuGetCapabilities", ExecutionOrder = 3, Payload = new List<byte> { 0x0C, 0x01 } },
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x0D, CommandName = "DfuWriteParcel", ExecutionOrder = 4, Payload = new List<byte> { 0x0D, 0x00, 0x00, 0x40 } },
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x0E, CommandName = "DfuHashNvmBlock", ExecutionOrder = 5, Payload = new List<byte> { 0x0E, 0x00, 0x10 } },
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x0F, CommandName = "AfsCheck", ExecutionOrder = 6, Payload = new List<byte> { 0x0F, 0x01, 0x00 } },
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x10, CommandName = "DfuRequest", ExecutionOrder = 7, Payload = new List<byte> { 0x10, 0x02 } }
                },
                Parameters = new List<ParameterValue>
                {
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1100, ParameterName = "PackageValidationMode", Value = 1, Notes = "0=None, 1=CRC, 2=SHA256" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1101, ParameterName = "ParcelSizeBytes", Value = 64, Notes = "Parcel size for chunked download (32-256 bytes)" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1102, ParameterName = "RebootDelayMs", Value = 500, Notes = "Delay before reboot after update" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1103, ParameterName = "EnableRollback", Value = 1, Notes = "0=Disabled, 1=Enabled" }
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

    private List<FeatureCollection> InitializeCollections()
    {
        var eagan = _npiPrograms.First(p => p.Name == "Eagan");
        var edina = _npiPrograms.First(p => p.Name == "Edina");
        var elko = _npiPrograms.First(p => p.Name == "Elko");
        
        var eagan321Lpi = _lpiVersions.First(v => v.Version == "3.2.1" && v.NpiProgram?.Name == "Eagan");
        var eagan250Piccolo = _piccoloVersions.First(v => v.Version == "2.5.0" && v.NpiProgram?.Name == "Eagan");
        
        var eagan330Lpi = _lpiVersions.First(v => v.Version == "3.3.0");
        var eagan260Piccolo = _piccoloVersions.First(v => v.Version == "2.6.0");
        
        var edina280Lpi = _lpiVersions.First(v => v.Version == "2.8.0");
        var edina190Piccolo = _piccoloVersions.First(v => v.Version == "1.9.0");
        
        var elko400Lpi = _lpiVersions.First(v => v.Version == "4.0.0");
        var elko300Piccolo = _piccoloVersions.First(v => v.Version == "3.0.0");

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
                Features = _features.Take(3).ToList(),
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
                Features = _features.Skip(1).Take(2).ToList(),
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
                Features = _features.Take(1).ToList(),
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
    public List<NpiProgram> GetNpiPrograms() => _npiPrograms;
    public NpiProgram? GetNpiProgram(Guid id) => _npiPrograms.FirstOrDefault(p => p.Id == id);
    
    public List<FirmwareVersion> GetLpiVersions(Guid? npiProgramId = null)
    {
        if (npiProgramId.HasValue)
            return _lpiVersions.Where(v => v.NpiProgramId == npiProgramId.Value).ToList();
        return _lpiVersions;
    }
    
    public List<FirmwareVersion> GetPiccoloVersions(Guid? npiProgramId = null)
    {
        if (npiProgramId.HasValue)
            return _piccoloVersions.Where(v => v.NpiProgramId == npiProgramId.Value).ToList();
        return _piccoloVersions;
    }
    
    public FirmwareVersion? GetLpiVersion(Guid id) => _lpiVersions.FirstOrDefault(v => v.Id == id);
    public FirmwareVersion? GetPiccoloVersion(Guid id) => _piccoloVersions.FirstOrDefault(v => v.Id == id);
    
    public List<Feature> GetFeatures() => _features;
    public Feature? GetFeature(Guid id) => _features.FirstOrDefault(f => f.Id == id);
    
    public List<FeatureCollection> GetCollections() => _collections;
    public FeatureCollection? GetCollection(Guid id) => _collections.FirstOrDefault(c => c.Id == id);
    
    public void AddCollection(FeatureCollection collection) => _collections.Add(collection);
    public void UpdateCollection(FeatureCollection collection)
    {
        var existing = _collections.FirstOrDefault(c => c.Id == collection.Id);
        if (existing != null)
        {
            var index = _collections.IndexOf(existing);
            _collections[index] = collection;
        }
    }
}
