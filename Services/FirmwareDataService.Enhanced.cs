using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Services;

/// <summary>
/// Enhanced FirmwareDataService with realistic PICCOLO-based features
/// Based on actual PiccoloInterface.xml and PiccoloProtocol.xml structures
/// </summary>
public partial class FirmwareDataService
{
    /// <summary>
    /// Initialize features based on actual firmware LPI parameter groups and PICCOLO commands
    /// Features map to real contexts like Memory1-5, ParameterGroups (Directionality, NoiseReduction, etc.)
    /// </summary>
    private List<Feature> InitializeFeaturesEnhanced()
    {
        return new List<Feature>
        {
            // ================================================================================
            // DIRECTIONALITY FEATURE
            // Maps to: DirectionalityPerMemory, DirectionalityControls, DirectionalityMonitor,
            //          DirectionalitySpatialAwareness, DirectionalityCalibration parameter groups
            // LPI Context: Memory1-5 (each memory can have different directionality settings)
            // ================================================================================
            new Feature
            {
                Id = Guid.NewGuid(),
                Name = "Adaptive Directionality",
                Description = "Multi-setting directional microphone system with spatial awareness. Maps to DirectionalityPerMemory, DirectionalityControls, DirectionalityMonitor, and DirectionalitySpatialAwareness parameter groups from LPI Context (Memory1-5). Supports 4 settings for different listening environments: Omnidirectional, Fixed Cardioid, Adaptive Moderate, and Full Adaptive with null steering.",
                Parameters = new List<ParameterValue>
                {
                    // ===== SETTING 1: Omnidirectional (Quiet Environments) =====
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1000, ParameterName = "Setting1_DirectionalityMode", Value = 0, Notes = "0=Omni, 1=Fixed, 2=Adaptive, 3=FullAdaptive | DirectionalityPerMemory.Mode" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1001, ParameterName = "Setting1_FrontProcessingGain", Value = 0, Notes = "Front hemisphere gain in dB (0dB = unity) | DirectionalityPerMemory.FrontGain" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1002, ParameterName = "Setting1_RearProcessingGain", Value = 0, Notes = "Rear hemisphere gain in dB (0dB = unity) | DirectionalityPerMemory.RearGain" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1003, ParameterName = "Setting1_NullDepth", Value = 0, Notes = "Directional null depth 0-20dB | DirectionalityPerMemory.NullDepth" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1004, ParameterName = "Setting1_AdaptationSpeed", Value = 0, Notes = "Speed of directional adaptation 0-100 | DirectionalityPerMemory.AdaptSpeed" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1005, ParameterName = "Setting1_PolarPattern", Value = 0, Notes = "0=Omni, 1=Cardioid, 2=Hypercardioid, 3=Bidirectional | DirectionalityPerMemory.Pattern" },
                    
                    // ===== SETTING 2: Fixed Cardioid (Moderate Noise) =====
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1010, ParameterName = "Setting2_DirectionalityMode", Value = 1, Notes = "Fixed directional mode" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1011, ParameterName = "Setting2_FrontProcessingGain", Value = 6, Notes = "+6dB front gain" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1012, ParameterName = "Setting2_RearProcessingGain", Value = -12, Notes = "-12dB rear attenuation" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1013, ParameterName = "Setting2_NullDepth", Value = 15, Notes = "15dB null depth at rear" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1014, ParameterName = "Setting2_AdaptationSpeed", Value = 0, Notes = "Fixed (no adaptation)" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1015, ParameterName = "Setting2_PolarPattern", Value = 1, Notes = "Cardioid pattern" },
                    
                    // ===== SETTING 3: Adaptive Moderate (Restaurant/Party) =====
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1020, ParameterName = "Setting3_DirectionalityMode", Value = 2, Notes = "Adaptive moderate mode" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1021, ParameterName = "Setting3_FrontProcessingGain", Value = 8, Notes = "+8dB front gain" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1022, ParameterName = "Setting3_RearProcessingGain", Value = -15, Notes = "-15dB rear attenuation" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1023, ParameterName = "Setting3_NullDepth", Value = 18, Notes = "18dB adaptive null depth" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1024, ParameterName = "Setting3_AdaptationSpeed", Value = 50, Notes = "Moderate adaptation speed" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1025, ParameterName = "Setting3_PolarPattern", Value = 2, Notes = "Hypercardioid pattern" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1026, ParameterName = "Setting3_NullSteeringEnable", Value = 1, Notes = "0=Disabled, 1=Enabled | DirectionalityPerMemory.NullSteering" },
                    
                    // ===== SETTING 4: Full Adaptive (High Noise/Cocktail Party) =====
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1030, ParameterName = "Setting4_DirectionalityMode", Value = 3, Notes = "Full adaptive with beam steering" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1031, ParameterName = "Setting4_FrontProcessingGain", Value = 10, Notes = "+10dB front gain" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1032, ParameterName = "Setting4_RearProcessingGain", Value = -18, Notes = "-18dB rear attenuation" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1033, ParameterName = "Setting4_NullDepth", Value = 20, Notes = "20dB maximum null depth" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1034, ParameterName = "Setting4_AdaptationSpeed", Value = 80, Notes = "Fast adaptation" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1035, ParameterName = "Setting4_PolarPattern", Value = 3, Notes = "Bidirectional with null steering" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1036, ParameterName = "Setting4_NullSteeringEnable", Value = 1, Notes = "Enabled with tracking" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1037, ParameterName = "Setting4_BeamWidth", Value = 60, Notes = "Narrow beam (degrees) | DirectionalityPerMemory.BeamWidth" },
                    
                    // ===== SPATIAL AWARENESS (Shared across all settings - DirectionalitySpatialAwareness group) =====
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1040, ParameterName = "SpatialAwareness_Enable", Value = 1, Notes = "0=Disabled, 1=Enabled | DirectionalitySpatialAwareness.Enable" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1041, ParameterName = "SpatialAwareness_Sensitivity", Value = 70, Notes = "Environmental change detection 0-100 | DirectionalitySpatialAwareness.Sensitivity" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1042, ParameterName = "WindNoiseDetection", Value = 1, Notes = "Auto-disable directionality in wind | DirectionalityControls.WindDetect" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1043, ParameterName = "TransitionSmoothingMs", Value = 500, Notes = "Smooth transitions between settings (ms) | DirectionalityControls.TransitionTime" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1044, ParameterName = "AutomaticSwitching", Value = 1, Notes = "Auto switch based on environment | DirectionalityControls.AutoSwitch" }
                },
                Commands = new List<PiccoloCommand>
                {
                    // LPI Context operations (Memory1-5 contexts contain directionality parameter groups)
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x40, CommandName = "LpiContextRead", ExecutionOrder = 1, Payload = new List<byte> { 0x40, 0x01, 0x00 }, Notes = "Read Memory1 context (contains DirectionalityPerMemory group) | AFS Service" },
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x41, CommandName = "LpiContextWrite", ExecutionOrder = 2, Payload = new List<byte> { 0x41, 0x01, 0x00 }, Notes = "Write updated directionality settings to Memory1 context | AFS Service" },
                    
                    // Parameter Group operations (DirectionalityPerMemory specific)
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x44, CommandName = "LpiParameterGroupRead", ExecutionOrder = 3, Payload = new List<byte> { 0x44, 0x15 }, Notes = "Read DirectionalityPerMemory parameter group (GroupId=0x15) | LPI Service" },
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x45, CommandName = "LpiParameterGroupWrite", ExecutionOrder = 4, Payload = new List<byte> { 0x45, 0x15 }, Notes = "Write DirectionalityPerMemory parameter group | LPI Service" },
                    
                    // Monitor operations (DirectionalityMonitor group - read-only status)
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x46, CommandName = "LpiMonitorRead_Directionality", ExecutionOrder = 5, Payload = new List<byte> { 0x46, 0x20 }, Notes = "Read DirectionalityMonitor status (current mode, SNR, null angle) | Monitor Service" }
                },
                CreatedAt = DateTime.Now.AddDays(-10),
                ModifiedAt = DateTime.Now.AddDays(-2),
                Owner = "alice.williams@company.com"
            },

            // ================================================================================
            // NOISE REDUCTION FEATURE
            // Maps to: NoiseReduction, BinauralNoiseReduction parameter groups
            // LPI Context: Memory1-5, MemoryGlobal
            // ================================================================================
            new Feature
            {
                Id = Guid.NewGuid(),
                Name = "Advanced Noise Reduction",
                Description = "Multi-band spectral noise reduction with speech preservation. Maps to NoiseReduction and BinauralNoiseReduction parameter groups. Uses 6-channel frequency-specific gain control with modulation detection for speech vs noise classification. Supports both monaural and binaural coordination.",
                Parameters = new List<ParameterValue>
                {
                    // Overall Control
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1050, ParameterName = "NoiseReductionStrength", Value = 65, Notes = "Overall NR aggressiveness 0-100 | NoiseReduction.Strength" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1051, ParameterName = "SpeechPreservation", Value = 80, Notes = "Speech protection level 0-100 | NoiseReduction.SpeechPreserve" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1052, ParameterName = "ModulationDetectionThreshold", Value = 45, Notes = "Threshold for speech vs noise (0-100) | NoiseReduction.ModThreshold" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1053, ParameterName = "AttackTimeMs", Value = 20, Notes = "Attack time 10-100ms | NoiseReduction.AttackTime" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1054, ParameterName = "ReleaseTimeMs", Value = 200, Notes = "Release time 50-500ms | NoiseReduction.ReleaseTime" },
                    
                    // 6-Channel Frequency-Specific Gains
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1055, ParameterName = "Channel1Gain_250_500Hz", Value = 50, Notes = "250-500 Hz band gain 0-100 | NoiseReduction.Ch1Gain" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1056, ParameterName = "Channel2Gain_500_1kHz", Value = 55, Notes = "500-1k Hz band gain | NoiseReduction.Ch2Gain" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1057, ParameterName = "Channel3Gain_1k_2kHz", Value = 60, Notes = "1k-2k Hz band (speech) | NoiseReduction.Ch3Gain" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1058, ParameterName = "Channel4Gain_2k_4kHz", Value = 65, Notes = "2k-4k Hz band (speech) | NoiseReduction.Ch4Gain" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1059, ParameterName = "Channel5Gain_4k_6kHz", Value = 60, Notes = "4k-6k Hz band | NoiseReduction.Ch5Gain" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1060, ParameterName = "Channel6Gain_6k_8kHz", Value = 55, Notes = "6k-8k Hz band | NoiseReduction.Ch6Gain" },
                    
                    // Binaural Coordination (BinauralNoiseReduction group)
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1061, ParameterName = "BinauralCoordination_Enable", Value = 1, Notes = "0=Monaural only, 1=Binaural | BinauralNoiseReduction.Enable" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1062, ParameterName = "BinauralSyncMode", Value = 2, Notes = "0=Independent, 1=Master-Slave, 2=Cooperative | BinauralNoiseReduction.SyncMode" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1063, ParameterName = "InterauralCoherence", Value = 70, Notes = "Binaural coherence threshold 0-100 | BinauralNoiseReduction.Coherence" }
                },
                Commands = new List<PiccoloCommand>
                {
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x44, CommandName = "LpiParameterGroupRead_NoiseReduction", ExecutionOrder = 1, Payload = new List<byte> { 0x44, 0x18 }, Notes = "Read NoiseReduction parameter group (GroupId=0x18)" },
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x45, CommandName = "LpiParameterGroupWrite_NoiseReduction", ExecutionOrder = 2, Payload = new List<byte> { 0x45, 0x18 }, Notes = "Write NoiseReduction parameter group" },
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x44, CommandName = "LpiParameterGroupRead_BinauralNR", ExecutionOrder = 3, Payload = new List<byte> { 0x44, 0x19 }, Notes = "Read BinauralNoiseReduction parameter group (GroupId=0x19)" },
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x45, CommandName = "LpiParameterGroupWrite_BinauralNR", ExecutionOrder = 4, Payload = new List<byte> { 0x45, 0x19 }, Notes = "Write BinauralNoiseReduction parameter group" }
                },
                CreatedAt = DateTime.Now.AddDays(-8),
                ModifiedAt = DateTime.Now.AddDays(-1),
                Owner = "john.doe@company.com"
            },

            // ================================================================================
            // TINNITUS THERAPY FEATURE
            // Maps to: TinnitusTherapyPerMemory parameter group
            // LPI Context: Memory1-5
            // ================================================================================
            new Feature
            {
                Id = Guid.NewGuid(),
                Name = "Tinnitus Therapy (Sound Generator)",
                Description = "Configurable tinnitus masking sound generator. Maps to TinnitusTherapyPerMemory parameter group in Memory contexts. Supports multiple noise types (white, pink, brown), customizable frequency shaping, and volume control for tinnitus relief.",
                Parameters = new List<ParameterValue>
                {
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1100, ParameterName = "TinnitusTherapy_Enable", Value = 1, Notes = "0=Off, 1=On | TinnitusTherapyPerMemory.Enable" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1101, ParameterName = "NoiseType", Value = 1, Notes = "0=White, 1=Pink, 2=Brown, 3=Custom | TinnitusTherapyPerMemory.NoiseType" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1102, ParameterName = "Volume", Value = 45, Notes = "Therapy volume 0-100 | TinnitusTherapyPerMemory.Volume" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1103, ParameterName = "LowCutFrequencyHz", Value = 500, Notes = "High-pass filter cutoff | TinnitusTherapyPerMemory.LowCut" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1104, ParameterName = "HighCutFrequencyHz", Value = 6000, Notes = "Low-pass filter cutoff | TinnitusTherapyPerMemory.HighCut" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1105, ParameterName = "ModulationDepth", Value = 15, Notes = "Amplitude modulation 0-100 | TinnitusTherapyPerMemory.ModDepth" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1106, ParameterName = "ModulationFrequencyHz", Value = 4, Notes = "Modulation rate in Hz (0-20) | TinnitusTherapyPerMemory.ModFreq" }
                },
                Commands = new List<PiccoloCommand>
                {
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x44, CommandName = "LpiParameterGroupRead_TinnitusTherapy", ExecutionOrder = 1, Payload = new List<byte> { 0x44, 0x10 }, Notes = "Read TinnitusTherapyPerMemory group (GroupId=0x10)" },
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x45, CommandName = "LpiParameterGroupWrite_TinnitusTherapy", ExecutionOrder = 2, Payload = new List<byte> { 0x45, 0x10 }, Notes = "Write TinnitusTherapyPerMemory group" }
                },
                CreatedAt = DateTime.Now.AddDays(-15),
                ModifiedAt = DateTime.Now.AddDays(-5),
                Owner = "jane.smith@company.com"
            },

            // ================================================================================
            // DEVICE FIRMWARE UPDATE (DFU) FEATURE
            // Maps to: PICCOLO DFU Service commands
            // Not LPI-based, uses dedicated PICCOLO DFU protocol
            // ================================================================================
            new Feature
            {
                Id = Guid.NewGuid(),
                Name = "Device Firmware Update (DFU)",
                Description = "Complete over-the-air firmware update workflow using PICCOLO DFU Service. State machine: GetStatus → Reset/Prepare → Download Image Package (parcels) → Validate Image → Download Script Package → Validate Script → Update Firmware → Reboot → Acknowledge/Rollback. Supports background download, pause/resume, and automatic rollback on failure.",
                Commands = new List<PiccoloCommand>
                {
                    // Step 1: Check current DFU state
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x06, CommandName = "DfuGetStatus", ExecutionOrder = 1, Payload = new List<byte> { 0x06, 0x0A }, Notes = "ServiceId=0x0A (DFU) | Query state: Ready, Downloading, Validated, UpdateComplete, UpdateFailed, etc." },
                    
                    // Step 2: Prepare for update
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x10, CommandName = "DfuRequest_Reset", ExecutionOrder = 2, Payload = new List<byte> { 0x10, 0x0A, 0x00 }, Notes = "RequestId=0x00 | Reset DFU agent to Ready state" },
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x10, CommandName = "DfuRequest_Prepare", ExecutionOrder = 3, Payload = new List<byte> { 0x10, 0x0A, 0x01 }, Notes = "RequestId=0x01 | Prepare for download (Ready → ReadyToDownload)" },
                    
                    // Step 3: Download firmware image in parcels (loop)
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x0B, CommandName = "DfuWriteParcel", ExecutionOrder = 4, Payload = new List<byte> { 0x0B, 0x0A, 0x00, 0x00, 0x40, 0x00 }, Notes = "Write 64-byte parcel | PackageType=0x00 (Image), ParcelIndex, ParcelSize=0x40, [Data...] | Loop until complete" },
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x06, CommandName = "DfuGetStatus_DownloadProgress", ExecutionOrder = 5, Payload = new List<byte> { 0x06, 0x0A }, Notes = "Check download progress (can pause/resume)" },
                    
                    // Step 4: Validate image package
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x10, CommandName = "DfuRequest_ValidateImage", ExecutionOrder = 6, Payload = new List<byte> { 0x10, 0x0A, 0x03 }, Notes = "RequestId=0x03 | Validate image CRC/hash (Downloading → ImagePackageValidated)" },
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x06, CommandName = "DfuGetStatus_ValidationResult", ExecutionOrder = 7, Payload = new List<byte> { 0x06, 0x0A }, Notes = "Check validation result" },
                    
                    // Step 5: Download script package for parameter preservation
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x0B, CommandName = "DfuWriteParcel_Script", ExecutionOrder = 8, Payload = new List<byte> { 0x0B, 0x0A, 0x01, 0x00, 0x20, 0x00 }, Notes = "PackageType=0x01 (Script) | Write LPI preservation script parcels" },
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x10, CommandName = "DfuRequest_ValidateScript", ExecutionOrder = 9, Payload = new List<byte> { 0x10, 0x0A, 0x05 }, Notes = "RequestId=0x05 | Validate script package" },
                    
                    // Step 6: Initiate firmware update (device will reboot)
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x10, CommandName = "DfuRequest_UpdateFirmware", ExecutionOrder = 10, Payload = new List<byte> { 0x10, 0x0A, 0x06 }, Notes = "RequestId=0x06 | Initiate update (30-90sec switchover), device reboots to new firmware" },
                    
                    // Step 7: After reboot, reconnect and check status
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x06, CommandName = "DfuGetStatus_AfterReboot", ExecutionOrder = 11, Payload = new List<byte> { 0x06, 0x0A }, Notes = "Reconnect and check: UpdateComplete or UpdateFailed" },
                    
                    // Step 8: Acknowledge successful update OR rollback
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x10, CommandName = "DfuRequest_AcknowledgeUpdate", ExecutionOrder = 12, Payload = new List<byte> { 0x10, 0x0A, 0x07 }, Notes = "RequestId=0x07 | Finalize new firmware" },
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x10, CommandName = "DfuRequest_Rollback", ExecutionOrder = 13, Payload = new List<byte> { 0x10, 0x0A, 0x08 }, Notes = "RequestId=0x08 | Rollback to previous firmware (failsafe)" },
                    
                    // Utility commands
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x09, CommandName = "DfuGetPackageInfo", ExecutionOrder = 14, Payload = new List<byte> { 0x09, 0x0A }, Notes = "Get package metadata (version, size, type)" },
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x0E, CommandName = "DfuHashNvmBlock", ExecutionOrder = 15, Payload = new List<byte> { 0x0E, 0x0A, 0x00, 0x10 }, Notes = "Hash verification of NVM blocks" }
                },
                Parameters = new List<ParameterValue>
                {
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1150, ParameterName = "DfuMode", Value = 0, Notes = "0=Background DFU (slow, maintains function), 1=Foreground DFU (fast, minimal function)" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1151, ParameterName = "ParcelSizeBytes", Value = 64, Notes = "Parcel size 16-256 bytes (typically 64)" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1152, ParameterName = "ValidationMethod", Value = 1, Notes = "0=None, 1=CRC32, 2=SHA256" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1153, ParameterName = "EnableAutoRollback", Value = 1, Notes = "0=Manual, 1=Auto rollback on boot failure" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1154, ParameterName = "PreserveSettings", Value = 1, Notes = "0=Reset defaults, 1=Preserve LPI parameters via script" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1155, ParameterName = "MaxSwitchoverTimeSec", Value = 60, Notes = "Maximum firmware switchover time 30-90sec" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1156, ParameterName = "EnablePauseResume", Value = 1, Notes = "Allow pause/resume on power cycle" }
                },
                CreatedAt = DateTime.Now.AddDays(-15),
                ModifiedAt = DateTime.Now.AddDays(-3),
                Owner = "firmware.team@company.com"
            },

            // ================================================================================
            // FEEDBACK CANCELLATION FEATURE
            // Maps to: FeedbackCanceller parameter group
            // LPI Context: Memory1-5, MemoryGlobal
            // ================================================================================
            new Feature
            {
                Id = Guid.NewGuid(),
                Name = "Adaptive Feedback Cancellation",
                Description = "Real-time acoustic feedback suppression using adaptive filtering. Maps to FeedbackCanceller parameter group. Monitors feedback paths and applies inverse filtering to prevent whistling while preserving prescribed gain.",
                Parameters = new List<ParameterValue>
                {
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1200, ParameterName = "FeedbackCancellation_Enable", Value = 1, Notes = "0=Disabled, 1=Enabled | FeedbackCanceller.Enable" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1201, ParameterName = "AdaptationRate", Value = 50, Notes = "Filter adaptation speed 0-100 | FeedbackCanceller.AdaptRate" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1202, ParameterName = "MaxGainReductionDb", Value = 12, Notes = "Max gain reduction to prevent feedback | FeedbackCanceller.MaxGainReduction" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1203, ParameterName = "DetectionThresholdDb", Value = 3, Notes = "Feedback detection sensitivity | FeedbackCanceller.DetectThreshold" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1204, ParameterName = "FilterLength", Value = 64, Notes = "Adaptive filter taps 32-128 | FeedbackCanceller.FilterTaps" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1205, ParameterName = "CancellationDepthDb", Value = 20, Notes = "Target feedback cancellation depth | FeedbackCanceller.CancelDepth" }
                },
                Commands = new List<PiccoloCommand>
                {
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x44, CommandName = "LpiParameterGroupRead_FeedbackCanceller", ExecutionOrder = 1, Payload = new List<byte> { 0x44, 0x1C }, Notes = "Read FeedbackCanceller group (GroupId=0x1C)" },
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x45, CommandName = "LpiParameterGroupWrite_FeedbackCanceller", ExecutionOrder = 2, Payload = new List<byte> { 0x45, 0x1C }, Notes = "Write FeedbackCanceller group" }
                },
                CreatedAt = DateTime.Now.AddDays(-25),
                ModifiedAt = DateTime.Now.AddDays(-10),
                Owner = "bob.johnson@company.com"
            },

            // ================================================================================
            // BLUETOOTH STREAMING FEATURE
            // Maps to: StreamingControls parameter group
            // LPI Context: BleConfiguration, BleSystem
            // ================================================================================
            new Feature
            {
                Id = Guid.NewGuid(),
                Name = "Bluetooth Audio Streaming",
                Description = "Wireless audio streaming from smartphones and accessories. Maps to StreamingControls parameter group in BLE contexts. Includes audio routing, latency compensation, volume control, and codec preference for streamed content.",
                Commands = new List<PiccoloCommand>
                {
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x50, CommandName = "BleStreamStart", ExecutionOrder = 1, Payload = new List<byte> { 0x50, 0x0C, 0x01 }, Notes = "ServiceId=0x0C (BLE) | Start streaming session" },
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x51, CommandName = "BleStreamSetVolume", ExecutionOrder = 2, Payload = new List<byte> { 0x51, 0x0C, 0x50 }, Notes = "Set streaming volume (0x00-0x64 / 0-100)" },
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x52, CommandName = "BleStreamStop", ExecutionOrder = 3, Payload = new List<byte> { 0x52, 0x0C }, Notes = "Stop streaming session" },
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x53, CommandName = "BleStreamGetStatus", ExecutionOrder = 4, Payload = new List<byte> { 0x53, 0x0C }, Notes = "Get streaming status (active, paused, codec, RSSI)" }
                },
                Parameters = new List<ParameterValue>
                {
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1250, ParameterName = "StreamingVolume", Value = 70, Notes = "Streaming audio volume 0-100 | StreamingControls.Volume" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1251, ParameterName = "LatencyCompensationMs", Value = 40, Notes = "A/V sync delay 0-200ms | StreamingControls.LatencyComp" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1252, ParameterName = "MixWithMicrophoneDb", Value = 6, Notes = "Mix ratio: streamed vs mic audio (-20 to +20dB) | StreamingControls.MixRatio" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1253, ParameterName = "CodecPreference", Value = 1, Notes = "0=SBC, 1=AAC, 2=aptX, 3=aptX-HD | StreamingControls.CodecPref" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1254, ParameterName = "AutoDucking_Enable", Value = 1, Notes = "Auto-reduce streaming when speech detected | StreamingControls.AutoDuck" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1255, ParameterName = "DuckingDepthDb", Value = -12, Notes = "Ducking attenuation when speech present | StreamingControls.DuckDepth" }
                },
                CreatedAt = DateTime.Now.AddDays(-18),
                ModifiedAt = DateTime.Now.AddDays(-7),
                Owner = "alice.williams@company.com"
            },

            // ================================================================================
            // OUTPUT COMPRESSION LIMITER (OCL) FEATURE
            // Maps to: Ocl (Output Compression Limiter) parameter group
            // LPI Context: Memory1-5
            // ================================================================================
            new Feature
            {
                Id = Guid.NewGuid(),
                Name = "Output Compression Limiter (OCL)",
                Description = "Multi-channel output compression and limiting for hearing protection. Maps to Ocl parameter group in Memory contexts. Provides per-channel compression ratios, knee points, attack/release times, and maximum output limiting to ensure user comfort and safety.",
                Parameters = new List<ParameterValue>
                {
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1300, ParameterName = "Ocl_Enable", Value = 1, Notes = "0=Disabled, 1=Enabled | Ocl.Enable" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1301, ParameterName = "CompressionRatio", Value = 30, Notes = "Compression ratio 1:1 to 10:1 (30 = 3:1) | Ocl.Ratio" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1302, ParameterName = "KneePointDb", Value = 55, Notes = "Compression threshold in dB SPL | Ocl.KneePoint" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1303, ParameterName = "AttackTimeMs", Value = 5, Notes = "Attack time 1-50ms | Ocl.AttackTime" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1304, ParameterName = "ReleaseTimeMs", Value = 50, Notes = "Release time 10-500ms | Ocl.ReleaseTime" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1305, ParameterName = "OutputLimitingDb", Value = 105, Notes = "Maximum output level (MPO) | Ocl.MaxOutput" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1306, ParameterName = "ExpansionRatio", Value = 15, Notes = "Low-level expansion 1:1 to 3:1 (15 = 1.5:1) | Ocl.ExpansionRatio" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1307, ParameterName = "ExpansionThresholdDb", Value = 30, Notes = "Expansion knee point in dB SPL | Ocl.ExpansionThreshold" }
                },
                Commands = new List<PiccoloCommand>
                {
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x44, CommandName = "LpiParameterGroupRead_Ocl", ExecutionOrder = 1, Payload = new List<byte> { 0x44, 0x12 }, Notes = "Read Ocl parameter group (GroupId=0x12)" },
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x45, CommandName = "LpiParameterGroupWrite_Ocl", ExecutionOrder = 2, Payload = new List<byte> { 0x45, 0x12 }, Notes = "Write Ocl parameter group" }
                },
                CreatedAt = DateTime.Now.AddDays(-12),
                ModifiedAt = DateTime.Now.AddDays(-4),
                Owner = "jane.smith@company.com"
            },

            // ================================================================================
            // FREQUENCY TRANSLATION/LOWERING FEATURE
            // Maps to: FrequencyTranslation parameter group
            // LPI Context: Memory1-5
            // ================================================================================
            new Feature
            {
                Id = Guid.NewGuid(),
                Name = "Frequency Translation (Frequency Lowering)",
                Description = "Frequency lowering technology to make high-frequency speech sounds audible by transposing them to lower frequencies. Maps to FrequencyTranslation parameter group. Essential for patients with severe high-frequency hearing loss who cannot benefit from conventional amplification in higher frequencies.",
                Parameters = new List<ParameterValue>
                {
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1400, ParameterName = "FreqTranslation_Enable", Value = 1, Notes = "0=Off, 1=On | FrequencyTranslation.Enable" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1401, ParameterName = "StartFrequencyHz", Value = 2500, Notes = "Frequency above which lowering begins (1500-6000Hz) | FrequencyTranslation.StartFreq" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1402, ParameterName = "DestinationFrequencyHz", Value = 1800, Notes = "Target frequency for transposed content | FrequencyTranslation.DestFreq" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1403, ParameterName = "CompressionRatio", Value = 25, Notes = "Frequency compression ratio (10-50, 25=2.5:1) | FrequencyTranslation.CompRatio" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1404, ParameterName = "Audibility", Value = 70, Notes = "Audibility vs naturalness balance 0-100 | FrequencyTranslation.Audibility" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1405, ParameterName = "BandwidthHz", Value = 2000, Notes = "Bandwidth of affected frequencies | FrequencyTranslation.Bandwidth" }
                },
                Commands = new List<PiccoloCommand>
                {
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x44, CommandName = "LpiParameterGroupRead_FrequencyTranslation", ExecutionOrder = 1, Payload = new List<byte> { 0x44, 0x18 }, Notes = "Read FrequencyTranslation group" },
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x45, CommandName = "LpiParameterGroupWrite_FrequencyTranslation", ExecutionOrder = 2, Payload = new List<byte> { 0x45, 0x18 }, Notes = "Write FrequencyTranslation group" }
                },
                CreatedAt = DateTime.Now.AddDays(-20),
                ModifiedAt = DateTime.Now.AddDays(-3),
                Owner = "john.doe@company.com"
            },

            // ================================================================================
            // ADVANCED ENVIRONMENT CLASSIFICATION FEATURE
            // Maps to: AdvancedEnvironmentClassification, AdaptiveTuningPerAcousticEnvironment groups
            // LPI Context: Multiple acoustic environment contexts
            // ================================================================================
            new Feature
            {
                Id = Guid.NewGuid(),
                Name = "Advanced Environment Classification (AEC)",
                Description = "AI-powered acoustic environment detection and automatic program adjustment. Maps to AdvancedEnvironmentClassification and AdaptiveTuningPerAcousticEnvironment parameter groups. Classifies listening environments (speech, noise, music, quiet, wind) and automatically optimizes audio processing parameters for each scenario.",
                Parameters = new List<ParameterValue>
                {
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1500, ParameterName = "AEC_Enable", Value = 1, Notes = "Enable automatic environment classification | AEC.Enable" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1501, ParameterName = "SensitivityLevel", Value = 60, Notes = "Classification sensitivity 0-100 | AEC.Sensitivity" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1502, ParameterName = "TransitionTimeMs", Value = 2000, Notes = "Smoothing time between environments (500-5000ms) | AEC.TransitionTime" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1503, ParameterName = "QuietEnvironment_Enabled", Value = 1, Notes = "Detect and adapt to quiet environments | AEC.Quiet.Enable" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1504, ParameterName = "SpeechInNoise_Enabled", Value = 1, Notes = "Detect speech in background noise | AEC.SpeechInNoise.Enable" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1505, ParameterName = "MusicDetection_Enabled", Value = 1, Notes = "Detect and optimize for music | AEC.Music.Enable" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1506, ParameterName = "WindNoiseDetection_Enabled", Value = 1, Notes = "Detect wind noise | AEC.WindNoise.Enable" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1507, ParameterName = "LargeRoomSpeech_Enabled", Value = 1, Notes = "Detect large room acoustics | AEC.LargeRoom.Enable" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1508, ParameterName = "Restaurant_Enabled", Value = 1, Notes = "Detect restaurant/cafeteria | AEC.Restaurant.Enable" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1509, ParameterName = "Transportation_Enabled", Value = 1, Notes = "Detect car/bus/train noise | AEC.Transportation.Enable" }
                },
                Commands = new List<PiccoloCommand>
                {
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x44, CommandName = "LpiParameterGroupRead_AEC", ExecutionOrder = 1, Payload = new List<byte> { 0x44, 0x22 }, Notes = "Read AEC parameter group" },
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x45, CommandName = "LpiParameterGroupWrite_AEC", ExecutionOrder = 2, Payload = new List<byte> { 0x45, 0x22 }, Notes = "Write AEC parameters" },
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x46, CommandName = "LpiMonitorRead_AEC_Status", ExecutionOrder = 3, Payload = new List<byte> { 0x46, 0x22 }, Notes = "Get current environment classification" }
                },
                CreatedAt = DateTime.Now.AddDays(-25),
                ModifiedAt = DateTime.Now.AddDays(-1),
                Owner = "alice.williams@company.com"
            },

            // ================================================================================
            // OWN VOICE DETECTION FEATURE
            // Maps to: OwnVoiceDetection parameter group
            // LPI Context: Memory1-5, monitoring contexts
            // ================================================================================
            new Feature
            {
                Id = Guid.NewGuid(),
                Name = "Own Voice Detection & Processing",
                Description = "Detects user's own voice and applies specialized processing to reduce occlusion effect and improve naturalness. Maps to OwnVoiceDetection parameter group. Uses advanced signal processing to differentiate own voice from external speech and apply custom gain/EQ adjustments.",
                Parameters = new List<ParameterValue>
                {
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1600, ParameterName = "OVD_Enable", Value = 1, Notes = "Enable own voice detection | OwnVoiceDetection.Enable" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1601, ParameterName = "DetectionSensitivity", Value = 75, Notes = "Detection threshold 0-100 | OwnVoiceDetection.Sensitivity" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1602, ParameterName = "GainReductionDb", Value = -6, Notes = "Gain reduction when own voice detected (-20 to 0dB) | OwnVoiceDetection.GainReduction" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1603, ParameterName = "LowFreqBoostDb", Value = 3, Notes = "Low-freq boost to compensate occlusion (0-10dB) | OwnVoiceDetection.LowFreqBoost" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1604, ParameterName = "TransitionTimeMs", Value = 50, Notes = "Attack/release time for processing changes | OwnVoiceDetection.TransitionTime" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1605, ParameterName = "OcclusionReduction_Enable", Value = 1, Notes = "Active occlusion reduction | OwnVoiceDetection.OcclusionReduction" }
                },
                Commands = new List<PiccoloCommand>
                {
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x44, CommandName = "LpiParameterGroupRead_OVD", ExecutionOrder = 1, Payload = new List<byte> { 0x44, 0x25 }, Notes = "Read OwnVoiceDetection group" },
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x45, CommandName = "LpiParameterGroupWrite_OVD", ExecutionOrder = 2, Payload = new List<byte> { 0x45, 0x25 }, Notes = "Write OVD parameters" },
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x46, CommandName = "LpiMonitorRead_OVD_Status", ExecutionOrder = 3, Payload = new List<byte> { 0x46, 0x25 }, Notes = "Get own voice detection status (active/inactive)" }
                },
                CreatedAt = DateTime.Now.AddDays(-15),
                ModifiedAt = DateTime.Now.AddDays(-2),
                Owner = "bob.johnson@company.com"
            },

            // ================================================================================
            // WIND NOISE ADAPTATION FEATURE
            // Maps to: WindNoiseAdaptation parameter group
            // LPI Context: Memory1-5, environment contexts
            // ================================================================================
            new Feature
            {
                Id = Guid.NewGuid(),
                Name = "Wind Noise Adaptation",
                Description = "Specialized processing to detect and suppress wind noise artifacts in outdoor environments. Maps to WindNoiseAdaptation parameter group. Automatically reduces directional microphone sensitivity and applies low-pass filtering when wind is detected to maintain speech intelligibility.",
                Parameters = new List<ParameterValue>
                {
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1700, ParameterName = "WindNoise_Enable", Value = 1, Notes = "Enable automatic wind noise detection/reduction | WindNoiseAdaptation.Enable" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1701, ParameterName = "DetectionThreshold", Value = 65, Notes = "Wind detection sensitivity 0-100 | WindNoiseAdaptation.Threshold" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1702, ParameterName = "AttenuationDepthDb", Value = -15, Notes = "Gain reduction in wind (-30 to 0dB) | WindNoiseAdaptation.Attenuation" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1703, ParameterName = "LowPassFilterHz", Value = 3000, Notes = "Low-pass cutoff when wind detected (1000-6000Hz) | WindNoiseAdaptation.LPF" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1704, ParameterName = "DisableDirectionality", Value = 1, Notes = "Switch to omni mode in wind | WindNoiseAdaptation.OmniSwitch" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1705, ParameterName = "AdaptationSpeedMs", Value = 500, Notes = "Response time to wind detection (100-2000ms) | WindNoiseAdaptation.Speed" }
                },
                Commands = new List<PiccoloCommand>
                {
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x44, CommandName = "LpiParameterGroupRead_WindNoise", ExecutionOrder = 1, Payload = new List<byte> { 0x44, 0x28 }, Notes = "Read WindNoiseAdaptation group" },
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x45, CommandName = "LpiParameterGroupWrite_WindNoise", ExecutionOrder = 2, Payload = new List<byte> { 0x45, 0x28 }, Notes = "Write wind noise parameters" }
                },
                CreatedAt = DateTime.Now.AddDays(-22),
                ModifiedAt = DateTime.Now.AddDays(-5),
                Owner = "alice.williams@company.com"
            },

            // ================================================================================
            // MULTI-CHANNEL EQUALIZER FEATURE
            // Maps to: EqualizerSlider parameter group
            // LPI Context: Memory1-5
            // ================================================================================
            new Feature
            {
                Id = Guid.NewGuid(),
                Name = "Multi-Channel Equalizer",
                Description = "Professional 8-band parametric equalizer for precise frequency shaping. Maps to EqualizerSlider parameter group. Provides user and audiologist control over gain adjustments across the full audible spectrum with configurable center frequencies, bandwidths, and gain ranges.",
                Parameters = new List<ParameterValue>
                {
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1800, ParameterName = "EQ_Enable", Value = 1, Notes = "Enable equalizer | EqualizerSlider.Enable" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1801, ParameterName = "Band1_250Hz_GainDb", Value = 0, Notes = "250Hz band gain (-20 to +20dB) | EqualizerSlider.Band1" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1802, ParameterName = "Band2_500Hz_GainDb", Value = 2, Notes = "500Hz band gain | EqualizerSlider.Band2" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1803, ParameterName = "Band3_1000Hz_GainDb", Value = 3, Notes = "1kHz band gain | EqualizerSlider.Band3" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1804, ParameterName = "Band4_2000Hz_GainDb", Value = 4, Notes = "2kHz band gain | EqualizerSlider.Band4" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1805, ParameterName = "Band5_3000Hz_GainDb", Value = 5, Notes = "3kHz band gain | EqualizerSlider.Band5" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1806, ParameterName = "Band6_4000Hz_GainDb", Value = 6, Notes = "4kHz band gain | EqualizerSlider.Band6" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1807, ParameterName = "Band7_6000Hz_GainDb", Value = 4, Notes = "6kHz band gain | EqualizerSlider.Band7" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1808, ParameterName = "Band8_8000Hz_GainDb", Value = 2, Notes = "8kHz band gain | EqualizerSlider.Band8" }
                },
                Commands = new List<PiccoloCommand>
                {
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x44, CommandName = "LpiParameterGroupRead_Equalizer", ExecutionOrder = 1, Payload = new List<byte> { 0x44, 0x30 }, Notes = "Read EqualizerSlider group" },
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x45, CommandName = "LpiParameterGroupWrite_Equalizer", ExecutionOrder = 2, Payload = new List<byte> { 0x45, 0x30 }, Notes = "Write EQ parameters" }
                },
                CreatedAt = DateTime.Now.AddDays(-28),
                ModifiedAt = DateTime.Now.AddDays(-1),
                Owner = "john.doe@company.com"
            },

            // ================================================================================
            // MOTION ADAPTATION FEATURE
            // Maps to: MotionDetectionControls, ImuFeatures parameter groups
            // LPI Context: Motion/IMU contexts
            // ================================================================================
            new Feature
            {
                Id = Guid.NewGuid(),
                Name = "Motion-Based Audio Adaptation",
                Description = "Uses inertial measurement unit (IMU) to detect user motion and activity, automatically adjusting audio processing parameters. Maps to MotionDetectionControls and ImuFeatures groups. Distinguishes stationary, walking, running, and head movement states to optimize directionality and noise reduction.",
                Parameters = new List<ParameterValue>
                {
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1900, ParameterName = "MotionAdaptation_Enable", Value = 1, Notes = "Enable motion-based adaptation | MotionDetectionControls.Enable" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1901, ParameterName = "Walking_DirectionalityBoost", Value = 1, Notes = "Increase directionality when walking | MotionDetectionControls.WalkingDir" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1902, ParameterName = "Running_NoiseReduction", Value = 2, Notes = "Enhanced NR when running (0-3) | MotionDetectionControls.RunningNR" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1903, ParameterName = "Stationary_SensitivityBoost", Value = 3, Notes = "Boost sensitivity when stationary (+3dB) | MotionDetectionControls.StationaryGain" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1904, ParameterName = "HeadMovement_Tracking", Value = 1, Notes = "Track head position for spatial audio | ImuFeatures.HeadTracking" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 1905, ParameterName = "ActivityMonitoring_Enable", Value = 1, Notes = "Log steps and activity | ImuFeatures.ActivityLog" }
                },
                Commands = new List<PiccoloCommand>
                {
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x44, CommandName = "LpiParameterGroupRead_Motion", ExecutionOrder = 1, Payload = new List<byte> { 0x44, 0x35 }, Notes = "Read MotionDetectionControls group" },
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x45, CommandName = "LpiParameterGroupWrite_Motion", ExecutionOrder = 2, Payload = new List<byte> { 0x45, 0x35 }, Notes = "Write motion parameters" },
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x46, CommandName = "LpiMonitorRead_IMU_Status", ExecutionOrder = 3, Payload = new List<byte> { 0x46, 0x35 }, Notes = "Get current activity state (stationary/walking/running)" }
                },
                CreatedAt = DateTime.Now.AddDays(-30),
                ModifiedAt = DateTime.Now.AddDays(-4),
                Owner = "bob.johnson@company.com"
            },

            // ================================================================================
            // TELECOIL (T-COIL) FEATURE
            // Maps to: Telecoil parameter group
            // LPI Context: Memory1-5 (T-coil memories)
            // ================================================================================
            new Feature
            {
                Id = Guid.NewGuid(),
                Name = "Telecoil (T-Coil) System",
                Description = "Electromagnetic induction receiver for hearing loops, landline telephones, and assistive listening systems. Maps to Telecoil parameter group. Provides gain control, equalization, and mixing options for T-coil input, with automatic phone detection and switching capabilities.",
                Parameters = new List<ParameterValue>
                {
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 2000, ParameterName = "Telecoil_Enable", Value = 1, Notes = "Enable T-coil functionality | Telecoil.Enable" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 2001, ParameterName = "TcoilGainDb", Value = 15, Notes = "T-coil input gain 0-30dB | Telecoil.Gain" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 2002, ParameterName = "TcoilMixMode", Value = 1, Notes = "0=T only, 1=T+Mic, 2=Auto | Telecoil.MixMode" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 2003, ParameterName = "MicMixRatio", Value = 50, Notes = "Mic mix percentage when T+Mic mode (0-100) | Telecoil.MicMix" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 2004, ParameterName = "LowFreqBoostDb", Value = 3, Notes = "Low-frequency emphasis for phone | Telecoil.LowBoost" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 2005, ParameterName = "AutoPhoneDetection", Value = 1, Notes = "Auto-switch to T-coil near phone | Telecoil.AutoPhone" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 2006, ParameterName = "LoopDetectionSensitivity", Value = 70, Notes = "Hearing loop detection threshold | Telecoil.LoopDetect" }
                },
                Commands = new List<PiccoloCommand>
                {
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x44, CommandName = "LpiParameterGroupRead_Telecoil", ExecutionOrder = 1, Payload = new List<byte> { 0x44, 0x40 }, Notes = "Read Telecoil group" },
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x45, CommandName = "LpiParameterGroupWrite_Telecoil", ExecutionOrder = 2, Payload = new List<byte> { 0x45, 0x40 }, Notes = "Write T-coil parameters" }
                },
                CreatedAt = DateTime.Now.AddDays(-35),
                ModifiedAt = DateTime.Now.AddDays(-8),
                Owner = "jane.smith@company.com"
            },

            // ================================================================================
            // GOOGLE FAST PAIR FEATURE
            // Maps to: BleGoogleFastPairConfiguration, BleGoogleFastPairInformation groups
            // LPI Context: BLE configuration contexts
            // ================================================================================
            new Feature
            {
                Id = Guid.NewGuid(),
                Name = "Google Fast Pair",
                Description = "Google Fast Pair enables quick, seamless Bluetooth pairing with Android devices using proximity-based detection. Maps to BleGoogleFastPairConfiguration and BleGoogleFastPairInformation parameter groups. Provides one-tap pairing, automatic device detection, and battery level sharing with Android ecosystem.",
                Parameters = new List<ParameterValue>
                {
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 2100, ParameterName = "FastPair_Enable", Value = 1, Notes = "Enable Google Fast Pair | BleGoogleFastPairConfiguration.Enable" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 2101, ParameterName = "ModelId", Value = 0x2C7F29, Notes = "Fast Pair model ID (24-bit) | BleGoogleFastPairInformation.ModelId" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 2102, ParameterName = "AccountKeyFilter", Value = 1, Notes = "Enable account key filtering | BleGoogleFastPairConfiguration.AccountKeyFilter" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 2103, ParameterName = "PersonalizedName_Enable", Value = 1, Notes = "Allow personalized device name | BleGoogleFastPairConfiguration.PersonalizedName" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 2104, ParameterName = "BatteryNotification", Value = 1, Notes = "Send battery level to Android | BleGoogleFastPairConfiguration.BatteryNotif" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 2105, ParameterName = "SilenceModeSupport", Value = 1, Notes = "Support silence/ring mode | BleGoogleFastPairConfiguration.SilenceMode" }
                },
                Commands = new List<PiccoloCommand>
                {
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x44, CommandName = "LpiParameterGroupRead_FastPair", ExecutionOrder = 1, Payload = new List<byte> { 0x44, 0x50 }, Notes = "Read FastPair configuration" },
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x45, CommandName = "LpiParameterGroupWrite_FastPair", ExecutionOrder = 2, Payload = new List<byte> { 0x45, 0x50 }, Notes = "Write FastPair settings" }
                },
                CreatedAt = DateTime.Now.AddDays(-19),
                ModifiedAt = DateTime.Now.AddDays(-6),
                Owner = "alice.williams@company.com"
            },

            // ================================================================================
            // AUTO-PHONE DETECTION FEATURE
            // Maps to: PhoneLeftBehind, DtmfRemoteControl parameter groups
            // LPI Context: BLE/connectivity contexts
            // ================================================================================
            new Feature
            {
                Id = Guid.NewGuid(),
                Name = "Auto-Phone Detection & Alerts",
                Description = "Proximity-based phone detection using Bluetooth signal strength to alert users when phone is left behind. Maps to PhoneLeftBehind and related parameter groups. Monitors RSSI to determine phone distance and triggers audio/haptic alerts when connection weakens beyond threshold.",
                Parameters = new List<ParameterValue>
                {
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 2200, ParameterName = "PhoneDetection_Enable", Value = 1, Notes = "Enable phone left behind detection | PhoneLeftBehind.Enable" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 2201, ParameterName = "ProximityThresholdRssi", Value = -80, Notes = "RSSI threshold for alert (-100 to -40dBm) | PhoneLeftBehind.RssiThreshold" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 2202, ParameterName = "AlertDelaySeconds", Value = 30, Notes = "Delay before triggering alert (10-120s) | PhoneLeftBehind.AlertDelay" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 2203, ParameterName = "AlertType", Value = 2, Notes = "0=None, 1=Audio, 2=Haptic, 3=Both | PhoneLeftBehind.AlertType" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 2204, ParameterName = "SnoozeTimeMinutes", Value = 15, Notes = "Snooze duration (5-60 min) | PhoneLeftBehind.SnoozeTime" }
                },
                Commands = new List<PiccoloCommand>
                {
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x44, CommandName = "LpiParameterGroupRead_PhoneDetect", ExecutionOrder = 1, Payload = new List<byte> { 0x44, 0x55 }, Notes = "Read phone detection config" },
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x45, CommandName = "LpiParameterGroupWrite_PhoneDetect", ExecutionOrder = 2, Payload = new List<byte> { 0x45, 0x55 }, Notes = "Write phone detection settings" }
                },
                CreatedAt = DateTime.Now.AddDays(-14),
                ModifiedAt = DateTime.Now.AddDays(-3),
                Owner = "bob.johnson@company.com"
            },

            // ================================================================================
            // SLEEP & HEALTH MONITORING FEATURE
            // Maps to: SleepMonitorControl, SleepMonitorStage, RespiratoryRateControls groups
            // LPI Context: Health monitoring contexts
            // ================================================================================
            new Feature
            {
                Id = Guid.NewGuid(),
                Name = "Sleep & Health Monitoring",
                Description = "Advanced health monitoring using IMU and audio sensors to track sleep quality, respiratory rate, and activity patterns. Maps to SleepMonitorControl, SleepMonitorStage, and RespiratoryRateControls parameter groups. Provides insights into sleep stages (light/deep/REM), posture, and respiratory patterns for holistic health tracking.",
                Parameters = new List<ParameterValue>
                {
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 2300, ParameterName = "SleepMonitoring_Enable", Value = 1, Notes = "Enable sleep tracking | SleepMonitorControl.Enable" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 2301, ParameterName = "SleepStageDetection", Value = 1, Notes = "Detect light/deep/REM stages | SleepMonitorStage.Detection" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 2302, ParameterName = "PostureTracking", Value = 1, Notes = "Monitor sleep posture | SleepMonitorStage.PostureTrack" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 2303, ParameterName = "RespiratoryRate_Enable", Value = 1, Notes = "Monitor breathing rate | RespiratoryRateControls.Enable" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 2304, ParameterName = "ApneaDetection", Value = 1, Notes = "Detect breathing irregularities | RespiratoryRateControls.ApneaDetect" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 2305, ParameterName = "ActivitySummary", Value = 1, Notes = "Daily activity summary | SleepMonitorControl.ActivitySummary" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 2306, ParameterName = "SmartAlarm_Enable", Value = 1, Notes = "Optimal wake time detection | SleepMonitorControl.SmartAlarm" }
                },
                Commands = new List<PiccoloCommand>
                {
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x44, CommandName = "LpiParameterGroupRead_SleepMonitor", ExecutionOrder = 1, Payload = new List<byte> { 0x44, 0x60 }, Notes = "Read sleep monitoring config" },
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x45, CommandName = "LpiParameterGroupWrite_SleepMonitor", ExecutionOrder = 2, Payload = new List<byte> { 0x45, 0x60 }, Notes = "Write sleep settings" },
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x46, CommandName = "LpiMonitorRead_SleepData", ExecutionOrder = 3, Payload = new List<byte> { 0x46, 0x60 }, Notes = "Retrieve sleep stage and respiratory data" }
                },
                CreatedAt = DateTime.Now.AddDays(-40),
                ModifiedAt = DateTime.Now.AddDays(-10),
                Owner = "jane.smith@company.com"
            },

            // ================================================================================
            // MULTI-MEMORY MANAGEMENT FEATURE
            // Maps to: Memory1-5 contexts, UiControls, LocalUiFeatureMap groups
            // LPI Context: Memory contexts, UI control contexts
            // ================================================================================
            new Feature
            {
                Id = Guid.NewGuid(),
                Name = "Multi-Memory Program Management",
                Description = "Comprehensive management of up to 5 customizable memory programs for different listening situations. Maps to Memory1-5 contexts and UiControls groups. Each memory stores complete audio processing configurations (compression, NR, directionality) optimized for specific environments like quiet, restaurant, music, outdoor, and streaming.",
                Parameters = new List<ParameterValue>
                {
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 2400, ParameterName = "Memory1_Name", Value = 0, Notes = "Program name index: 0=Automatic, 1=Quiet, 2=Restaurant | UiControls.Mem1Name" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 2401, ParameterName = "Memory2_Name", Value = 2, Notes = "Restaurant/Noisy" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 2402, ParameterName = "Memory3_Name", Value = 5, Notes = "Music" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 2403, ParameterName = "Memory4_Name", Value = 3, Notes = "Outdoor" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 2404, ParameterName = "Memory5_Name", Value = 7, Notes = "Streaming" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 2405, ParameterName = "AutoMemorySwitching", Value = 1, Notes = "Enable automatic memory switching | UiControls.AutoMemSwitch" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 2406, ParameterName = "MemorySwitchTone", Value = 1, Notes = "Play tone on memory change | UiControls.MemTone" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 2407, ParameterName = "ButtonAssignment", Value = 1, Notes = "0=Volume, 1=Memory, 2=Streaming | LocalUiFeatureMap.ButtonFunc" }
                },
                Commands = new List<PiccoloCommand>
                {
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x40, CommandName = "LpiContextRead_Memory1", ExecutionOrder = 1, Payload = new List<byte> { 0x40, 0x01 }, Notes = "Read Memory1 context" },
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x41, CommandName = "LpiContextWrite_Memory1", ExecutionOrder = 2, Payload = new List<byte> { 0x41, 0x01 }, Notes = "Write Memory1 configuration" },
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x48, CommandName = "SetActiveMemory", ExecutionOrder = 3, Payload = new List<byte> { 0x48, 0x02 }, Notes = "Switch to memory program (1-5)" }
                },
                CreatedAt = DateTime.Now.AddDays(-50),
                ModifiedAt = DateTime.Now.AddDays(-15),
                Owner = "john.doe@company.com"
            },

            // ================================================================================
            // AURACAST BROADCAST AUDIO FEATURE
            // Maps to: AuracastTransmitterInfo, BroadcastDeviceList, BleWirelessConfiguration groups
            // LPI Context: BLE/Broadcast contexts
            // ================================================================================
            new Feature
            {
                Id = Guid.NewGuid(),
                Name = "Auracast Broadcast Audio Reception",
                Description = "Bluetooth LE Audio Auracast technology for receiving broadcast audio in public venues (theaters, airports, gyms). Maps to AuracastTransmitterInfo and BroadcastDeviceList parameter groups. Enables one-to-many audio broadcasting with low latency, multiple language support, and automatic venue detection.",
                Parameters = new List<ParameterValue>
                {
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 2500, ParameterName = "Auracast_Enable", Value = 1, Notes = "Enable Auracast broadcast reception | AuracastTransmitterInfo.Enable" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 2501, ParameterName = "AutoScanForBroadcasts", Value = 1, Notes = "Auto-scan for nearby broadcasts | BroadcastDeviceList.AutoScan" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 2502, ParameterName = "PreferredLanguage", Value = 1, Notes = "0=English, 1=Spanish, 2=French, etc. | AuracastTransmitterInfo.Language" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 2503, ParameterName = "BroadcastVolumeDb", Value = 75, Notes = "Volume for broadcast audio (0-100) | AuracastTransmitterInfo.Volume" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 2504, ParameterName = "MixWithEnvironment", Value = 50, Notes = "Mix broadcast with ambient mic (0-100%) | AuracastTransmitterInfo.MixRatio" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 2505, ParameterName = "MaxBroadcastsStored", Value = 10, Notes = "Remember recent broadcasts (1-20) | BroadcastDeviceList.MaxStored" }
                },
                Commands = new List<PiccoloCommand>
                {
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x44, CommandName = "LpiParameterGroupRead_Auracast", ExecutionOrder = 1, Payload = new List<byte> { 0x44, 0x70 }, Notes = "Read Auracast configuration" },
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x45, CommandName = "LpiParameterGroupWrite_Auracast", ExecutionOrder = 2, Payload = new List<byte> { 0x45, 0x70 }, Notes = "Write Auracast settings" },
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x72, CommandName = "ScanForBroadcasts", ExecutionOrder = 3, Payload = new List<byte> { 0x72 }, Notes = "Initiate broadcast scan" },
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x73, CommandName = "ConnectToBroadcast", ExecutionOrder = 4, Payload = new List<byte> { 0x73, 0x00 }, Notes = "Connect to selected broadcast ID" }
                },
                CreatedAt = DateTime.Now.AddDays(-8),
                ModifiedAt = DateTime.Now.AddHours(-12),
                Owner = "alice.williams@company.com"
            },

            // ================================================================================
            // DNN SPEECH ENHANCEMENT FEATURE
            // Maps to: DnnSpeechEnhancement parameter group
            // LPI Context: Memory1-5
            // ================================================================================
            new Feature
            {
                Id = Guid.NewGuid(),
                Name = "DNN Speech Enhancement",
                Description = "Deep Neural Network-powered speech enhancement using AI/ML models to separate speech from noise. Maps to DnnSpeechEnhancement parameter group. Provides superior noise suppression and speech clarity compared to traditional algorithms, with adjustable strength and preservation of natural speech quality.",
                Parameters = new List<ParameterValue>
                {
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 2600, ParameterName = "DNN_Enable", Value = 1, Notes = "Enable DNN speech enhancement | DnnSpeechEnhancement.Enable" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 2601, ParameterName = "EnhancementStrength", Value = 75, Notes = "DNN processing strength 0-100 | DnnSpeechEnhancement.Strength" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 2602, ParameterName = "ModelVersion", Value = 3, Notes = "DNN model version (1-5, higher=newer) | DnnSpeechEnhancement.ModelVer" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 2603, ParameterName = "SpeechPreservation", Value = 85, Notes = "Naturalness vs clarity balance 0-100 | DnnSpeechEnhancement.Preservation" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 2604, ParameterName = "MusicModeProtection", Value = 1, Notes = "Reduce DNN in music environments | DnnSpeechEnhancement.MusicProtect" },
                    new ParameterValue { Id = Guid.NewGuid(), ParameterId = 2605, ParameterName = "LatencyMs", Value = 12, Notes = "DNN processing latency (8-20ms) | DnnSpeechEnhancement.Latency" }
                },
                Commands = new List<PiccoloCommand>
                {
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x44, CommandName = "LpiParameterGroupRead_DNN", ExecutionOrder = 1, Payload = new List<byte> { 0x44, 0x75 }, Notes = "Read DNN configuration" },
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x45, CommandName = "LpiParameterGroupWrite_DNN", ExecutionOrder = 2, Payload = new List<byte> { 0x45, 0x75 }, Notes = "Write DNN parameters" },
                    new PiccoloCommand { Id = Guid.NewGuid(), CommandCode = 0x76, CommandName = "DNN_LoadModel", ExecutionOrder = 3, Payload = new List<byte> { 0x76, 0x03 }, Notes = "Load specific DNN model version" }
                },
                CreatedAt = DateTime.Now.AddDays(-5),
                ModifiedAt = DateTime.Now.AddDays(-1),
                Owner = "bob.johnson@company.com"
            }
        };
    }
}
