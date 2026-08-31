using System;
using GameConsoleApp.Models;
using GameCore.ObservableStates;

namespace GameConsoleApp.StateHandlers.Abstractions
{
    internal abstract class StateHandlerBase { }

    internal abstract class StateHandlerBase<TState> : StateHandlerBase
        where TState : ObservableStateBase
    {
        public abstract void RenderState(TState state);

        public abstract GameModeHandlerResponse ProcessKey(ConsoleKeyInfo key);

        private const int FrameContentWidth = 60;

        protected void RenderFrameStart() =>
            Console.WriteLine($"╔═{"".PadRight(FrameContentWidth, '═')}═╗");

        protected void RenderFrameLine(string content = "") =>
            Console.WriteLine($"║ {content.PadRight(FrameContentWidth, ' ')} ║");

        protected void RenderFrameFinish() =>
            Console.WriteLine($"╚═{"".PadRight(FrameContentWidth, '═')}═╝");
    }
}
