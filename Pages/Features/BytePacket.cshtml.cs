using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Pages.Features;

public class BytePacketModel : PageModel
{
    private readonly FirmwareDataService _dataService;

    public BytePacketModel(FirmwareDataService dataService)
    {
        _dataService = dataService;
    }

    public Feature? Feature { get; set; }
    public byte[] BytePacket { get; set; } = Array.Empty<byte>();
    public int SelectedSetting { get; set; } = 1;
    public bool HasMultipleSettings { get; set; }
    
    public FirmwareVersion? CurrentLpiVersion { get; set; }
    public FirmwareVersion? CurrentPiccoloVersion { get; set; }

    public IActionResult OnGet(Guid id, int setting = 1)
    {
        Feature = _dataService.GetFeature(id);
        if (Feature == null)
        {
            return NotFound();
        }

        SelectedSetting = setting;
        HasMultipleSettings = DetectMultipleSettings();
        
        LoadFirmwareContext();
        GenerateBytePacket();
        
        return Page();
    }

    private bool DetectMultipleSettings()
    {
        if (Feature == null) return false;
        
        // Check if feature has Setting1_, Setting2_, etc. parameters
        return Feature.Parameters.Any(p => p.ParameterName.Contains("Setting1_")) &&
               Feature.Parameters.Any(p => p.ParameterName.Contains("Setting2_"));
    }

    private void LoadFirmwareContext()
    {
        // Get default Eagan firmware versions
        var eagan = _dataService.GetNpiPrograms().FirstOrDefault(p => p.Name == "Eagan");
        if (eagan != null)
        {
            CurrentLpiVersion = _dataService.GetLpiVersions(eagan.Id)
                .FirstOrDefault(v => v.Version == "3.2.1");
            CurrentPiccoloVersion = _dataService.GetPiccoloVersions(eagan.Id)
                .FirstOrDefault(v => v.Version == "2.5.0");
        }
    }

    private void GenerateBytePacket()
    {
        if (Feature == null)
        {
            BytePacket = Array.Empty<byte>();
            return;
        }

        List<byte> packet = new List<byte>();
        
        // Byte 0: Command header (mock value)
        packet.Add(0x5A);
        
        // Get parameters for the selected setting
        var settingParams = Feature.Parameters
            .Where(p => !HasMultipleSettings || p.ParameterName.Contains($"Setting{SelectedSetting}_"))
            .OrderBy(p => p.ParameterId)
            .ToList();
        
        // Add parameter values as bytes
        foreach (var param in settingParams.Take(25)) // Limit to 25 parameters
        {
            packet.Add((byte)(param.Value & 0xFF)); // Take low byte of value
        }
        
        // Pad to at least 26 bytes
        while (packet.Count < 26)
        {
            packet.Add(0x00);
        }
        
        // Calculate checksum (XOR of all bytes)
        byte checksum = 0;
        foreach (var b in packet)
        {
            checksum ^= b;
        }
        packet.Add(checksum);
        
        BytePacket = packet.ToArray();
    }

    public ParameterValue? GetParameterForByte(int byteIndex)
    {
        if (Feature == null || byteIndex == 0 || byteIndex >= BytePacket.Length - 1)
        {
            return null;
        }

        var settingParams = Feature.Parameters
            .Where(p => !HasMultipleSettings || p.ParameterName.Contains($"Setting{SelectedSetting}_"))
            .OrderBy(p => p.ParameterId)
            .ToList();

        int paramIndex = byteIndex - 1; // Skip header byte
        return paramIndex < settingParams.Count ? settingParams[paramIndex] : null;
    }

    public string GetParameterCategory(string parameterName)
    {
        if (parameterName.Contains("Directionality")) return "Directionality";
        if (parameterName.Contains("Noise")) return "Noise Reduction";
        if (parameterName.Contains("Tinnitus")) return "Tinnitus Therapy";
        if (parameterName.Contains("Feedback")) return "Feedback Cancellation";
        if (parameterName.Contains("Compression") || parameterName.Contains("Ocl")) return "Compression/OCL";
        if (parameterName.Contains("Stream") || parameterName.Contains("Ble")) return "Streaming";
        if (parameterName.Contains("Front")) return "Front Processing";
        if (parameterName.Contains("Rear")) return "Rear Processing";
        if (parameterName.Contains("Spatial")) return "Spatial Awareness";
        return "Other";
    }
}
