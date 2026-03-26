using UnrealBuildTool;
using System.Collections.Generic;

public class SwarmDefenseStarterTarget : TargetRules
{
    public SwarmDefenseStarterTarget(TargetInfo Target) : base(Target)
    {
        Type = TargetType.Game;
        DefaultBuildSettings = BuildSettingsVersion.Latest;
        IncludeOrderVersion = EngineIncludeOrderVersion.Latest;
        ExtraModuleNames.AddRange(new string[] { "SwarmDefenseStarter" });
    }
}
