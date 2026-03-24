using UnrealBuildTool;
using System.Collections.Generic;

public class SwarmDefenseTrainerTarget : TargetRules
{
    public SwarmDefenseTrainerTarget(TargetInfo Target) : base(Target)
    {
        Type = TargetType.Game;
        DefaultBuildSettings = BuildSettingsVersion.Latest;
        IncludeOrderVersion = EngineIncludeOrderVersion.Latest;
        ExtraModuleNames.Add("SwarmDefenseTrainer");
    }
}
