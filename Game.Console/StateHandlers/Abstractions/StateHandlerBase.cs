using System;
using System.Collections.Generic;
using System.Text;
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

        private const int FrameContentWidth = 80;

        protected void RenderFrameStart() =>
            Console.WriteLine($"╔═{"".PadRight(FrameContentWidth, '═')}═╗");

        protected void RenderFrameLine(string content = "")
        {
            foreach (var line in WrapToFrameWidth(content))
                Console.WriteLine($"║ {line.PadRight(FrameContentWidth, ' ')} ║");
        }

        /// <summary>
        /// Breaks content that does not fit into the frame over continuation lines, so a long
        /// entry (a wordy action name, a combatant carrying its HP) never breaks the border.
        /// </summary>
        private static IEnumerable<string> WrapToFrameWidth(string content)
        {
            if (content.Length <= FrameContentWidth)
            {
                yield return content;
                yield break;
            }

            const string continuationIndent = "    ";

            var indent = string.Empty;
            var line = new StringBuilder();

            foreach (var word in content.Split(' '))
            {
                var wouldOverflow =
                    indent.Length + line.Length + 1 + word.Length > FrameContentWidth;
                if (line.Length > 0 && wouldOverflow)
                {
                    yield return indent + line;
                    indent = continuationIndent;
                    line.Clear();
                }

                if (line.Length > 0)
                    line.Append(' ');
                line.Append(word);
            }

            if (line.Length > 0)
                yield return indent + line;
        }

        protected void RenderFrameDivider() =>
            Console.WriteLine($"╠═{"".PadRight(FrameContentWidth, '═')}═╣");

        protected void RenderFrameFinish() =>
            Console.WriteLine($"╚═{"".PadRight(FrameContentWidth, '═')}═╝");
    }
}
