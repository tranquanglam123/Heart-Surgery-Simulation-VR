using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VR_Surgery.Scripts.Core
{

    public static class TypeEnums
    {
        public enum OperatingMode
        {
            Surgery,
            Transplant,
            Null
        }

        public enum ModePhase
        {
            Idle,
            Cut,
            Stretch,
            Lining,
            TransplantIdle,
            HeartOut,
            HeartIn
        }
    }
}

