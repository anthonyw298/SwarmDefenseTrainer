using UnrealBuildTool;
using System.Collections.Generic;

public class SwarmDefenseStarterEditorTarget : TargetRules
{
    public SwarmDefenseStarterEditorTarget(TargetInfo Target) : base(Target)
    {
        Type = TargetType.Editor;
        DefaultBuildSettings = BuildSettingsVersion.Latest;
        IncludeOrderVersion = EngineIncludeOrderVersion.Latest;
        ExtraModuleNames.AddRange(new string[] { "SwarmDefenseStarter" });
    }
}
