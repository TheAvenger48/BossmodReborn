﻿namespace BossMod.Stormblood.Alliance.A32Agrias;

public enum OID : uint
{
    Boss = 0x261F, // R3.800, x?
    Fran = 0x273E, // R0.595, x?
    Halidom = 0x2626, // R1.000, x?
    Helper = 0x233C, // R0.500, x?, 523 type
    EphemeralKnight = 0x2620, // R3.800, x?
    SwordKnight = 0x2623, // R1.250, x?
    ShieldKnight = 0x2624, // R1.250, x?
    EmblazonedShield = 0x2625, // R1.000, x?
}

public enum AID : uint
{
    AutoAttack = 870, // Boss/SwordKnight->player, no cast, single-target
    SpellAttack = 14431, // ShieldKnight->player, no cast, single-target
    CleansingFlame1 = 14436, // Boss->self, 5.0s cast, single-target
    CleansingFlame2 = 14437, // Helper->players, 5.0s cast, range 6 circle
    CleansingStrike = 14420, // Boss->self, 6.0s cast, single-target
    Consecration = 14416, // Boss->self, 4.0s cast, single-target
    DivineLight = 14439, // Boss->self, 4.0s cast, range 60 circle
    DivineRuination1 = 14424, // Boss->self, 8.0s cast, single-target
    DivineRuination2 = 14425, // Helper->self, no cast, range 60 width 6 rect
    HallowedBolt1 = 14426, // Boss->self, 4.0s cast, single-target
    HallowedBolt2 = 14427, // Helper->self, 4.0s cast, range 15 circle
    HallowedBolt3 = 14428, // Helper->self, no cast, range ?-30 donut
    HeavenlyJudgment1 = 14432, // Boss->self, 4.0s cast, single-target
    HeavenlyJudgment2 = 14433, // Helper->self, 8.0s cast, range 60 circle
    JudgmentBlade = 14423, // Boss->self, 8.0s cast, range 60 circle
    MortalBlow = 14430, // SwordKnight->self, 5.0s cast, range 30 circle
    NorthswainsStrike1 = 14419, // EphemeralKnight->self, 15.0s cast, range 60 width 6 rect
    NorthswainsStrike2 = 14982, // Boss->self, 15.0s cast, single-target
    ThunderSlash = 14438, // Boss->self/player, 4.0s cast, range 60 ?-degree cone
    Unknown1 = 14429, // Boss->self, no cast, single-target
    Unknown2 = 15058, // Helper->self, no cast, range 60 circle
    Unknown3 = 14417, // Halidom->player, no cast, single-target

}

public enum SID : uint
{
    BrinkOfDeath = 44, // none->player, extra=0x0
    DamageUp = 1781, // none->Boss/SwordKnight, extra=0x0
    Doom = 210, // none->player, extra=0x0
    EarthAndWater = 367, // none->player, extra=0x5
    Fetters = 667, // none->player, extra=0xEC4
    Invincibility1 = 1570, // none->player, extra=0x0
    Invincibility2 = 775, // none->ShieldKnight, extra=0x0
    Jackpot = 902, // none->player, extra=0xA
    MeatAndMead = 360, // none->player, extra=0xA
    ReducedRates = 364, // none->player, extra=0x1E
    Shieldbearer = 1733, // none->player, extra=0x0
    Stun = 149, // none->player, extra=0x0
    Swordbearer = 1734, // none->player, extra=0x0
    TheEcho = 42, // none->player, extra=0x0
    TheHeatOfBattle = 365, // none->player, extra=0xA
    Transcendent = 418, // none->player, extra=0x0
    VulnerabilityDown = 350, // none->EmblazonedShield/SwordKnight, extra=0x0
    VulnerabilityUp = 202, // Boss/SwordKnight->player, extra=0x1/0x6/0x5/0x2/0x3/0x7
    Weakness = 43, // none->player, extra=0x0
    FleetFooted = 2932, // none->player, extra=0x32
}

public enum IconID : uint
{
    Icon_8 = 8, // player
    Icon_11 = 11, // player
    Icon_13 = 13, // player
    Icon_23 = 23, // player
    Icon_25 = 25, // player
    Icon_127 = 127, // player
    Icon_139 = 139, // player
    Icon_153 = 153, // player
    Icon_165 = 165, // player
    Icon_4294967292 = 4294967292, // player

}

public enum TetherID : uint
{
    Tether_14 = 14, // EmblazonedShield/SwordKnight->ShieldKnight/SwordKnight
    Tether_2 = 2, // player->Boss
}
