﻿namespace BossMod.Endwalker.Criterion.C01ASS.C012Gladiator
{
    class RushOfMightFront : Components.SelfTargetedAOEs
    {
        public RushOfMightFront() : base(ActionID.MakeSpell(AID.RushOfMightFront), new AOEShapeCone(60, 90.Degrees())) { }
    }

    class RushOfMightBack : Components.SelfTargetedAOEs
    {
        public RushOfMightBack() : base(ActionID.MakeSpell(AID.RushOfMightBack), new AOEShapeCone(60, 90.Degrees())) { }
    }

    public class C012Gladiator : BossModule
    {
        public C012Gladiator(WorldState ws, Actor primary) : base(ws, primary, new ArenaBoundsSquare(new(-35, -271), 20)) { }
    }
}