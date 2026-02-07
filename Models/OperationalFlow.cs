using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace WebApp.Models;

public class OperationalFlow
{
    public Guid Id { get; set; }
    public Guid CollectionId { get; set; } // Link to parent collection context
    public Guid FeatureId { get; set; }    // Link to feature
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<FlowOperation> Operations { get; set; } = new();
}

public class FlowOperation
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = "Sequential"; // "Sequential", "Multi-Command"
    public List<FlowStep> Steps { get; set; } = new();
}

public class FlowStep
{
    public Guid Id { get; set; }
    public string StepNumber { get; set; } = string.Empty; // e.g. "1.1", "1.2"
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Type { get; set; } = "Command"; // "Command", "Decision", "Delay", "Calculation", "Set Value"
    
    // Key details stored as JSON string in database
    private string _attributesJson = "{}";
    
    [NotMapped]
    public Dictionary<string, string> Attributes
    {
        get => string.IsNullOrEmpty(_attributesJson) 
            ? new Dictionary<string, string>() 
            : JsonSerializer.Deserialize<Dictionary<string, string>>(_attributesJson) ?? new Dictionary<string, string>();
        set => _attributesJson = JsonSerializer.Serialize(value);
    }
    
    public string AttributesJson
    {
        get => _attributesJson;
        set => _attributesJson = value;
    }
    
    // For Decision/Forking
    public List<FlowBranch> Branches { get; set; } = new();
}

public class FlowBranch
{
    public string Label { get; set; } = string.Empty; // "Success", "Failure", "True", "False"
    public string Description { get; set; } = string.Empty; // "If battery > 80%"
    public string Action { get; set; } = string.Empty; // "Continue", "Retry", "Go to Step X"
    public bool IsErrorPath { get; set; } = false;
}
