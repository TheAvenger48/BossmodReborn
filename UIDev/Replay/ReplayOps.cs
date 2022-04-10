﻿using BossMod;
using System;
using System.Numerics;

namespace UIDev
{
    public static class ReplayOps
    {
        public abstract class Operation
        {
            public DateTime Timestamp;

            public abstract void Redo(WorldState ws);
            public abstract void Undo(WorldState ws);
        }

        public class OpZoneChange : Operation
        {
            public ushort Zone;
            private ushort _prev;

            public override void Redo(WorldState ws)
            {
                _prev = ws.CurrentZone;
                ws.CurrentZone = Zone;
            }

            public override void Undo(WorldState ws)
            {
                ws.CurrentZone = _prev;
            }
        }

        public class OpWaymarkChange : Operation
        {
            public Waymark ID;
            public Vector3? Pos;
            private Vector3? _prev;

            public override void Redo(WorldState ws)
            {
                _prev = ws.Waymarks[ID];
                ws.Waymarks[ID] = Pos;
            }

            public override void Undo(WorldState ws)
            {
                ws.Waymarks[ID] = _prev;
            }
        }

        public class OpActorCreate : Operation
        {
            public uint InstanceID;
            public uint OID;
            public string Name = "";
            public ActorType Type;
            public Class Class;
            public Vector4 PosRot;
            public float HitboxRadius;
            public uint HPCur;
            public uint HPMax;
            public bool IsTargetable;
            public uint OwnerID;

            public override void Redo(WorldState ws)
            {
                ws.Actors.Add(InstanceID, OID, Name, Type, Class, PosRot, HitboxRadius, HPCur, HPMax, IsTargetable, OwnerID);
            }

            public override void Undo(WorldState ws)
            {
                ws.Actors.Remove(InstanceID);
            }
        }

        public class OpActorDestroy : Operation
        {
            public uint InstanceID;
            private uint OID;
            private string Name = "";
            private ActorType Type;
            private Class Class;
            private Vector4 PosRot;
            private float HitboxRadius;
            private uint HPCur;
            private uint HPMax;
            private bool IsTargetable;
            private uint OwnerID;

            public override void Redo(WorldState ws)
            {
                var actor = ws.Actors.Find(InstanceID);
                OID = actor?.OID ?? 0;
                Name = actor?.Name ?? "";
                Type = actor?.Type ?? ActorType.None;
                Class = actor?.Class ?? Class.None;
                PosRot = actor?.PosRot ?? new();
                HitboxRadius = actor?.HitboxRadius ?? 0;
                HPCur = actor?.HPCur ?? 0;
                HPMax = actor?.HPMax ?? 0;
                IsTargetable = actor?.IsTargetable ?? false;
                OwnerID = actor?.OwnerID ?? 0;
                ws.Actors.Remove(InstanceID);
            }

            public override void Undo(WorldState ws)
            {
                ws.Actors.Add(InstanceID, OID, Name, Type, Class, PosRot, HitboxRadius, HPCur, HPMax, IsTargetable, OwnerID);
            }
        }

        public class OpActorRename : Operation
        {
            public uint InstanceID;
            public string Name = "";
            private string _prev = "";

            public override void Redo(WorldState ws)
            {
                var actor = ws.Actors.Find(InstanceID);
                if (actor != null)
                {
                    _prev = actor.Name;
                    ws.Actors.Rename(actor, Name);
                }
            }

            public override void Undo(WorldState ws)
            {
                var actor = ws.Actors.Find(InstanceID);
                if (actor != null)
                {
                    ws.Actors.Rename(actor, _prev);
                }
            }
        }

        public class OpActorClassChange : Operation
        {
            public uint InstanceID;
            public Class Class;
            private Class _prevClass;

            public override void Redo(WorldState ws)
            {
                var actor = ws.Actors.Find(InstanceID);
                if (actor != null)
                {
                    _prevClass = actor.Class;
                    ws.Actors.ChangeClass(actor, Class);
                }
            }

            public override void Undo(WorldState ws)
            {
                var actor = ws.Actors.Find(InstanceID);
                if (actor != null)
                {
                    ws.Actors.ChangeClass(actor, _prevClass);
                }
            }
        }

        public class OpActorMove : Operation
        {
            public uint InstanceID;
            public Vector4 PosRot;
            private Vector4 _prevPosRot;

            public override void Redo(WorldState ws)
            {
                var actor = ws.Actors.Find(InstanceID);
                if (actor != null)
                {
                    _prevPosRot = actor.PosRot;
                    ws.Actors.Move(actor, PosRot);
                }
            }

            public override void Undo(WorldState ws)
            {
                var actor = ws.Actors.Find(InstanceID);
                if (actor != null)
                {
                    ws.Actors.Move(actor, _prevPosRot);
                }
            }
        }

        public class OpActorHP : Operation
        {
            public uint InstanceID;
            public uint Cur;
            public uint Max;
            private uint _prevCur;
            private uint _prevMax;

            public override void Redo(WorldState ws)
            {
                var actor = ws.Actors.Find(InstanceID);
                if (actor != null)
                {
                    _prevCur = actor.HPCur;
                    _prevMax = actor.HPMax;
                    ws.Actors.UpdateHP(actor, Cur, Max);
                }
            }

            public override void Undo(WorldState ws)
            {
                var actor = ws.Actors.Find(InstanceID);
                if (actor != null)
                {
                    ws.Actors.UpdateHP(actor, _prevCur, _prevMax);
                }
            }
        }

        public class OpActorTargetable : Operation
        {
            public uint InstanceID;
            public bool Value;
            private bool _prev;

            public override void Redo(WorldState ws)
            {
                var actor = ws.Actors.Find(InstanceID);
                if (actor != null)
                {
                    _prev = actor.IsTargetable;
                    ws.Actors.ChangeIsTargetable(actor, Value);
                }
            }

            public override void Undo(WorldState ws)
            {
                var actor = ws.Actors.Find(InstanceID);
                if (actor != null)
                {
                    ws.Actors.ChangeIsTargetable(actor, _prev);
                }
            }
        }

        public class OpActorDead : Operation
        {
            public uint InstanceID;
            public bool Value;
            private bool _prev;

            public override void Redo(WorldState ws)
            {
                var actor = ws.Actors.Find(InstanceID);
                if (actor != null)
                {
                    _prev = actor.IsDead;
                    ws.Actors.ChangeIsDead(actor, Value);
                }
            }

            public override void Undo(WorldState ws)
            {
                var actor = ws.Actors.Find(InstanceID);
                if (actor != null)
                {
                    ws.Actors.ChangeIsDead(actor, _prev);
                }
            }
        }

        public class OpActorCombat : Operation
        {
            public uint InstanceID;
            public bool Value;
            private bool _prev;

            public override void Redo(WorldState ws)
            {
                var actor = ws.Actors.Find(InstanceID);
                if (actor != null)
                {
                    _prev = actor.InCombat;
                    ws.Actors.ChangeInCombat(actor, Value);
                }
            }

            public override void Undo(WorldState ws)
            {
                var actor = ws.Actors.Find(InstanceID);
                if (actor != null)
                {
                    ws.Actors.ChangeInCombat(actor, _prev);
                }
            }
        }

        public class OpActorTarget : Operation
        {
            public uint InstanceID;
            public uint Value;
            private uint _prev;

            public override void Redo(WorldState ws)
            {
                var actor = ws.Actors.Find(InstanceID);
                if (actor != null)
                {
                    _prev = actor.TargetID;
                    ws.Actors.ChangeTarget(actor, Value);
                }
            }

            public override void Undo(WorldState ws)
            {
                var actor = ws.Actors.Find(InstanceID);
                if (actor != null)
                {
                    ws.Actors.ChangeTarget(actor, _prev);
                }
            }
        }

        public class OpActorCast : Operation
        {
            public uint InstanceID;
            public ActorCastInfo? Value;
            private ActorCastInfo? _prev;

            public override void Redo(WorldState ws)
            {
                var actor = ws.Actors.Find(InstanceID);
                if (actor != null)
                {
                    _prev = actor.CastInfo;
                    ws.Actors.UpdateCastInfo(actor, Value);
                }
            }

            public override void Undo(WorldState ws)
            {
                var actor = ws.Actors.Find(InstanceID);
                if (actor != null)
                {
                    ws.Actors.UpdateCastInfo(actor, _prev);
                }
            }
        }

        public class OpActorTether : Operation
        {
            public uint InstanceID;
            public ActorTetherInfo Value;
            private ActorTetherInfo _prev;

            public override void Redo(WorldState ws)
            {
                var actor = ws.Actors.Find(InstanceID);
                if (actor != null)
                {
                    _prev = actor.Tether;
                    ws.Actors.UpdateTether(actor, Value);
                }
            }

            public override void Undo(WorldState ws)
            {
                var actor = ws.Actors.Find(InstanceID);
                if (actor != null)
                {
                    ws.Actors.UpdateTether(actor, _prev);
                }
            }
        }

        public class OpActorStatus : Operation
        {
            public uint InstanceID;
            public int Index;
            public ActorStatus Value;
            private ActorStatus _prev;

            public override void Redo(WorldState ws)
            {
                var actor = ws.Actors.Find(InstanceID);
                if (actor != null)
                {
                    _prev = actor.Statuses[Index];
                    ws.Actors.UpdateStatus(actor, Index, Value);
                }
            }

            public override void Undo(WorldState ws)
            {
                var actor = ws.Actors.Find(InstanceID);
                if (actor != null)
                {
                    ws.Actors.UpdateStatus(actor, Index, _prev);
                }
            }
        }

        public class OpPartyJoin : Operation
        {
            public ulong ContentID;
            public uint InstanceID;
            private int _slot;

            public override void Redo(WorldState ws)
            {
                _slot = ws.Party.Add(ContentID, InstanceID, ws.Party.ContentIDs[0] == 0);
            }

            public override void Undo(WorldState ws)
            {
                ws.Party.Remove(_slot);
            }
        }

        public class OpPartyLeave : Operation
        {
            public ulong ContentID;
            public uint InstanceID;

            public override void Redo(WorldState ws)
            {
                var slot = ws.Party.ContentIDs.IndexOf(ContentID);
                ws.Party.Remove(slot);
            }

            public override void Undo(WorldState ws)
            {
                ws.Party.Add(ContentID, InstanceID, ws.Party.ContentIDs[0] == 0);
            }
        }

        public class OpPartyAssign : Operation
        {
            public ulong ContentID;
            public uint InstanceID;
            private uint _prevID;

            public override void Redo(WorldState ws)
            {
                var slot = ws.Party.ContentIDs.IndexOf(ContentID);
                _prevID = ws.Party.ActorIDs[slot];
                ws.Party.AssignActor(slot, ContentID, InstanceID);
            }

            public override void Undo(WorldState ws)
            {
                var slot = ws.Party.ContentIDs.IndexOf(ContentID);
                ws.Party.AssignActor(slot, ContentID, _prevID);
            }
        }

        public class OpEventIcon : Operation
        {
            public uint InstanceID;
            public uint IconID;

            public override void Redo(WorldState ws)
            {
                ws.Events.DispatchIcon((InstanceID, IconID));
            }

            public override void Undo(WorldState ws)
            {
            }
        }

        public class OpEventCast : Operation
        {
            public CastEvent Value = new();

            public override void Redo(WorldState ws)
            {
                ws.Events.DispatchCast(Value);
            }

            public override void Undo(WorldState ws)
            {
            }
        }

        public class OpEventEnvControl : Operation
        {
            public uint FeatureID;
            public byte Index;
            public uint State;

            public override void Redo(WorldState ws)
            {
                ws.Events.DispatchEnvControl((FeatureID, Index, State));
            }

            public override void Undo(WorldState ws)
            {
            }
        }
    }
}
