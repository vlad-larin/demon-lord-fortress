using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore.Models.GameEvents
{
    public abstract class GameEventBase
    {
        public string Description { get; set; }

        public GameEventBase(string description)
        {
            Description = description;
        }
    }
}
