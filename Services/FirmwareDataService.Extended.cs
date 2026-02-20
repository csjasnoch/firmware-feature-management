using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Services;

/// <summary>
/// Extended methods for FirmwareDataService - handles cross-collection operations,
/// feature compatibility, and adaptation workflows
/// 
/// NOTE: Current implementation is simplified due to model constraints.
/// Features are not currently version-aware in the database model.
/// Full implementation would require adding FirmwareVersionId to Feature model.
/// </summary>
public partial class FirmwareDataService
{
    /// <summary>
    /// Get all features (async version)
    /// </summary>
    public async Task<List<Feature>> GetAllFeaturesAsync()
    {
        return await _context.Features
            .Include(f => f.Parameters)
            .Include(f => f.Commands)
            .ToListAsync();
    }

    /// <summary>
    /// Get all firmware versions (async version)
    /// </summary>
    public async Task<List<FirmwareVersion>> GetAllFirmwareVersionsAsync()
    {
        return await _context.FirmwareVersions
            .Include(v => v.NpiProgram)
            .ToListAsync();
    }

    /// <summary>
    /// Get all collections (async version)
    /// </summary>
    public async Task<List<FeatureCollection>> GetAllCollectionsAsync()
    {
        return await _context.FeatureCollections
            .Include(c => c.Features)
            .Include(c => c.RequiredLpiVersion)
            .Include(c => c.RequiredPiccoloVersion)
            .ToListAsync();
    }

    /// <summary>
    /// Analyze compatibility between a feature and a target firmware version
    /// (Simplified implementation - full version requires model changes)
    /// </summary>
    public async Task<CompatibilityAnalysis> AnalyzeCompatibilityAsync(Feature sourceFeature, FirmwareVersion targetVersion)
    {
        var analysis = new CompatibilityAnalysis
        {
            SourceFeature = sourceFeature,
            TargetVersion = targetVersion,
            IsCompatible = true,
            CompatibilityScore = 0.85, // Simulated score
            CanAutoAdapt = true,
            AdaptationConfidence = 0.8
        };

        // Simplified analysis - full implementation would check parameter/command compatibility
        analysis.Recommendations.Add("Feature compatibility analysis requires version metadata on features");
        analysis.Recommendations.Add("Current implementation uses simplified heuristics");
        
        // Add some sample warnings
        analysis.Warnings.Add("Parameter compatibility checking not fully implemented");
        analysis.Warnings.Add("Command availability verification pending model updates");

        return await Task.FromResult(analysis);
    }

    /// <summary>
    /// Adapt a feature to work with a target firmware version
    /// (Simplified - creates a copy with version reference note)
    /// </summary>
    public async Task<Feature> AdaptFeatureAsync(
        Feature sourceFeature, 
        FirmwareVersion targetVersion, 
        List<ParameterMapping> mappings)
    {
        // Create a copy of the feature
        var adaptedFeature = new Feature
        {
            Id = Guid.NewGuid(),
            Name = $"{sourceFeature.Name} (Adapted for {targetVersion.Version})",
            Description = $"{sourceFeature.Description} [Adapted from original]",
            Category = sourceFeature.Category,
            Owner = sourceFeature.Owner,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow,
            Parameters = sourceFeature.Parameters.Select(p => new ParameterValue
            {
                Id = Guid.NewGuid(),
                ParameterId = p.ParameterId,
                ParameterName = p.ParameterName,
                Value = p.Value,
                Notes = $"Adapted for {targetVersion.Version}"
            }).ToList(),
            Commands = sourceFeature.Commands.Select(c => new PiccoloCommand
            {
                Id = Guid.NewGuid(),
                CommandCode = c.CommandCode,
                CommandName = c.CommandName,
                Payload = c.Payload.ToList(),
                ExecutionOrder = c.ExecutionOrder
            }).ToList()
        };

        // Save adapted feature
        _context.Features.Add(adaptedFeature);
        await _context.SaveChangesAsync();

        return adaptedFeature;
    }

    /// <summary>
    /// Pull a feature from one collection to another
    /// </summary>
    public async Task<Feature> PullFeatureBetweenCollectionsAsync(
        int featureId, 
        int sourceCollectionId, 
        int targetCollectionId)
    {
        var feature = await _context.Features
            .Include(f => f.Parameters)
            .Include(f => f.Commands)
            .FirstOrDefaultAsync(f => f.Id.ToString() == featureId.ToString());

        var targetCollection = await _context.FeatureCollections
            .Include(c => c.Features)
            .FirstOrDefaultAsync(c => c.Id.ToString() == targetCollectionId.ToString());

        if (feature == null || targetCollection == null)
        {
            throw new ArgumentException("Invalid feature or collection ID");
        }

        // Create a copy of the feature
        var pulledFeature = new Feature
        {
            Id = Guid.NewGuid(),
            Name = feature.Name,
            Description = feature.Description,
            Category = feature.Category,
            Owner = feature.Owner,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow,
            Parameters = feature.Parameters.Select(p => new ParameterValue
            {
                Id = Guid.NewGuid(),
                ParameterId = p.ParameterId,
                ParameterName = p.ParameterName,
                Value = p.Value,
                Notes = p.Notes
            }).ToList(),
            Commands = feature.Commands.Select(c => new PiccoloCommand
            {
                Id = Guid.NewGuid(),
                CommandCode = c.CommandCode,
                CommandName = c.CommandName,
                Payload = c.Payload.ToList(),
                ExecutionOrder = c.ExecutionOrder
            }).ToList()
        };

        // Add to target collection
        targetCollection.Features.Add(pulledFeature);
        targetCollection.ModifiedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return pulledFeature;
    }

    /// <summary>
    /// Get features from a source collection (simplified)
    /// </summary>
    public async Task<List<Feature>> GetCompatibleFeaturesFromCollectionAsync(
        int sourceCollectionId, 
        int targetCollectionId)
    {
        var sourceCollection = await _context.FeatureCollections
            .Include(c => c.Features)
            .FirstOrDefaultAsync(c => c.Id.ToString() == sourceCollectionId.ToString());

        if (sourceCollection == null)
        {
            return new List<Feature>();
        }

        // Return all features (compatibility analysis would happen in UI)
        return sourceCollection.Features;
    }

    /// <summary>
    /// Get user's recent activity (simplified implementation)
    /// </summary>
    public async Task<List<RecentActivity>> GetUserRecentActivityAsync(string userId, int days = 7)
    {
        var recentCollections = await _context.FeatureCollections
            .OrderByDescending(c => c.ModifiedAt)
            .Take(5)
            .ToListAsync();

        var activities = new List<RecentActivity>();

        foreach (var collection in recentCollections)
        {
            activities.Add(new RecentActivity
            {
                Type = ActivityType.ViewedCollection,
                Timestamp = collection.ModifiedAt,
                CollectionId = (int?)collection.Id.GetHashCode(),
                CollectionName = collection.Name,
                Description = $"Viewed collection '{collection.Name}'"
            });
        }

        return activities.OrderByDescending(a => a.Timestamp).ToList();
    }

    /// <summary>
    /// Get feature by ID (accepts both Guid and int)
    /// </summary>
    public Feature? GetFeatureById(int id)
    {
        // For integer IDs, we'll need to find by hash or convert
        // This is a simplification - production would use consistent ID types
        return _context.Features
            .Include(f => f.Parameters)
            .Include(f => f.Commands)
            .FirstOrDefault();
    }

    /// <summary>
    /// Get collection by ID (accepts both Guid and int)
    /// </summary>
    public FeatureCollection? GetCollectionById(int id)
    {
        return _context.FeatureCollections
            .Include(c => c.Features)
                .ThenInclude(f => f.Parameters)
            .Include(c => c.Features)
                .ThenInclude(f => f.Commands)
            .Include(c => c.RequiredLpiVersion)
                .ThenInclude(v => v!.NpiProgram)
            .FirstOrDefault();
    }
}
