using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore.Models.GameEvents
{
    public class SimpleGameEvent : GameEventBase
    {
        public SimpleGameEvent(string description)
            : base(description) { }
    }
}
