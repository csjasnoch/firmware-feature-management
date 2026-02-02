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
            new Feature
            {
                Id = Guid.NewGuid(),
                Name = "Directionality",
                Description = "24-parameter directionality feature with 4 settings",
                Parameters = new List<ParameterValue>
                {
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1000, ParameterName = "PARAM_Directionality_0", Value = 75 },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1001, ParameterName = "PARAM_Directionality_1", Value = 50 }
                },
                CreatedAt = DateTime.Now.AddDays(-10),
                ModifiedAt = DateTime.Now.AddDays(-2),
                Owner = "john.doe@company.com"
            },
            new Feature
            {
                Id = Guid.NewGuid(),
                Name = "NoiseReduction",
                Description = "Advanced noise reduction with adaptive thresholds",
                Parameters = new List<ParameterValue>
                {
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1005, ParameterName = "PARAM_NoiseGate_0", Value = 30 },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1006, ParameterName = "PARAM_Threshold_0", Value = 60 }
                },
                CreatedAt = DateTime.Now.AddDays(-8),
                ModifiedAt = DateTime.Now.AddDays(-1),
                Owner = "john.doe@company.com"
            },
            new Feature
            {
                Id = Guid.NewGuid(),
                Name = "DeviceInitialization",
                Description = "Standard device startup sequence",
                Commands = new List<PiccoloCommand>
                {
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x10, CommandName = "CMD_Initialize_0", ExecutionOrder = 1 },
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x13, CommandName = "CMD_Configure_0", ExecutionOrder = 2 }
                },
                CreatedAt = DateTime.Now.AddDays(-15),
                ModifiedAt = DateTime.Now.AddDays(-15),
                Owner = "system"
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
