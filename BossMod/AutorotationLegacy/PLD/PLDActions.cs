﻿//namespace BossMod.PLD;

//class Actions : TankActions
//{
//    public const int AutoActionST = AutoActionFirstCustom + 0;
//    public const int AutoActionAOE = AutoActionFirstCustom + 1;

//    private bool _aoe;
//    private readonly Rotation.State _state;
//    private readonly Rotation.Strategy _strategy;
//    private readonly ConfigListener<PLDConfig> _config;

//    public Actions(Autorotation autorot, Actor player) : base(autorot, player, Definitions.UnlockQuests)
//    {
//        _state = new(autorot.WorldState);
//        _strategy = new();

//        SupportedSpell(AID.Reprisal).Condition = _ => Autorot.Hints.PotentialTargets.Any(e => e.Actor.Position.InCircle(Player.Position, 5 + e.Actor.HitboxRadius)); // TODO: consider checking only target?..
//        SupportedSpell(AID.Interject).Condition = target => target?.CastInfo?.Interruptible ?? false;

//        _config = Service.Config.GetAndSubscribe<PLDConfig>(OnConfigModified);
//    }

//    protected override void Dispose(bool disposing)
//    {
//        _config.Dispose();
//        base.Dispose(disposing);
//    }

//    public override CommonRotation.PlayerState GetState() => _state;
//    public override CommonRotation.Strategy GetStrategy() => _strategy;

//    protected override void UpdateInternalState(int autoAction)
//    {
//        base.UpdateInternalState(autoAction);
//        _aoe = autoAction switch
//        {
//            AutoActionST => false,
//            AutoActionAOE => true, // TODO: consider making AI-like check
//            AutoActionAIFight => NumTargetsHitByAOE() >= 3,
//            _ => false, // irrelevant...
//        };
//        UpdatePlayerState();
//        FillCommonStrategy(_strategy, ActionDefinitions.IDPotionStr);
//    }

//    protected override void QueueAIActions()
//    {
//        if (_state.Unlocked(AID.Interject))
//        {
//            var interruptibleEnemy = Autorot.Hints.PotentialTargets.Find(e => e.ShouldBeInterrupted && (e.Actor.CastInfo?.Interruptible ?? false) && e.Actor.Position.InCircle(Player.Position, 3 + e.Actor.HitboxRadius + Player.HitboxRadius));
//            SimulateManualActionForAI(ActionID.MakeSpell(AID.Interject), interruptibleEnemy?.Actor, interruptibleEnemy != null);
//        }
//        if (_state.Unlocked(AID.IronWill))
//            SimulateManualActionForAI(ActionID.MakeSpell(AID.IronWill), Player, ShouldSwapStance());
//        if (_state.Unlocked(AID.Provoke))
//        {
//            var provokeEnemy = Autorot.Hints.PotentialTargets.Find(e => e.ShouldBeTanked && e.PreferProvoking && e.Actor.TargetID != Player.InstanceID && e.Actor.Position.InCircle(Player.Position, 25 + e.Actor.HitboxRadius + Player.HitboxRadius));
//            SimulateManualActionForAI(ActionID.MakeSpell(AID.Provoke), provokeEnemy?.Actor, provokeEnemy != null);
//        }
//    }

//    protected override ActionQueue.Entry CalculateAutomaticGCD()
//    {
//        if (Autorot.PrimaryTarget == null || AutoAction < AutoActionAIFight)
//            return default;
//        if (AutoAction == AutoActionAIFight && !Autorot.PrimaryTarget.Position.InCircle(Player.Position, 3 + Autorot.PrimaryTarget.HitboxRadius + Player.HitboxRadius) && _state.Unlocked(AID.ShieldLob))
//            return MakeResult(AID.ShieldLob, Autorot.PrimaryTarget); // TODO: reconsider...
//        var aid = Rotation.GetNextBestGCD(_state, _strategy, _aoe);
//        return MakeResult(aid, Autorot.PrimaryTarget);
//    }

//    protected override ActionQueue.Entry CalculateAutomaticOGCD(float deadline)
//    {
//        if (Autorot.PrimaryTarget == null || AutoAction < AutoActionAIFight)
//            return default;

//        ActionID res = new();
//        if (_state.CanWeave(deadline - _state.OGCDSlotLength)) // first ogcd slot
//            res = Rotation.GetNextBestOGCD(_state, _strategy, deadline - _state.OGCDSlotLength, _aoe);
//        if (!res && _state.CanWeave(deadline)) // second/only ogcd slot
//            res = Rotation.GetNextBestOGCD(_state, _strategy, deadline, _aoe);
//        return MakeResult(res, Autorot.PrimaryTarget);
//    }

//    private void UpdatePlayerState()
//    {
//        FillCommonPlayerState(_state);
//        _state.HaveTankStance = Player.FindStatus(SID.IronWill) != null;

//        //s.Gauge = Service.JobGauges.Get<PLDGauge>().OathGauge;

//        _state.FightOrFlightLeft = StatusDetails(Player, SID.FightOrFlight, Player.InstanceID).Left;
//    }

//    private void OnConfigModified(PLDConfig config)
//    {
//        // placeholders
//        SupportedSpell(AID.FastBlade).PlaceholderForAuto = config.FullRotation ? AutoActionST : AutoActionNone;
//        SupportedSpell(AID.TotalEclipse).PlaceholderForAuto = config.FullRotation ? AutoActionAOE : AutoActionNone;

//        // combo replacement
//        SupportedSpell(AID.RiotBlade).TransformAction = config.STCombos ? () => ActionID.MakeSpell(Rotation.GetNextRiotBladeComboAction(ComboLastMove)) : null;
//        SupportedSpell(AID.RageOfHalone).TransformAction = config.STCombos ? () => ActionID.MakeSpell(Rotation.GetNextSTComboAction(ComboLastMove, AID.RageOfHalone)) : null;
//        SupportedSpell(AID.GoringBlade).TransformAction = config.STCombos ? () => ActionID.MakeSpell(Rotation.GetNextSTComboAction(ComboLastMove, AID.GoringBlade)) : null;

//        // smart targets
//        SupportedSpell(AID.Shirk).TransformTarget = config.SmartShirkTarget ? SmartTargetCoTank : null;
//        SupportedSpell(AID.Provoke).TransformTarget = config.ProvokeMouseover ? SmartTargetHostile : null; // TODO: also interject/low-blow
//    }

//    private AID ComboLastMove => (AID)Autorot.ActionManager.ComboLastMove;

//    private int NumTargetsHitByAOE() => Autorot.Hints.NumPriorityTargetsInAOECircle(Player.Position, 5);
//}
