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

        /// <summary>
        /// The label a numbered option is offered under. A prompt can run past nine entries
        /// - the battle plan does as soon as it holds a few steps - and a keypress is a
        /// single character, so the tenth option onwards is a letter.
        /// </summary>
        protected static string RenderOptionLabel(int index) =>
            index < 9 ? (index + 1).ToString() : ((char)('A' + index - 9)).ToString();

        /// <summary>
        /// Reads a keypress as one of the options a prompt offered, matching it against the
        /// very labels that were printed.
        /// </summary>
        protected static bool TryReadOption(ConsoleKeyInfo key, int optionCount, out int index)
        {
            var option = key.KeyChar.ToString().ToUpperInvariant();
            for (index = 0; index < optionCount; index++)
            {
                if (RenderOptionLabel(index) == option)
                    return true;
            }

            index = -1;
            return false;
        }

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

            // Whatever margin the caller indented the entry to is part of the entry - the
            // battle plan nests its intents under their step - so every line of it hangs off
            // that margin and the continuations are indented from there, not from the border.
            var body = content.TrimStart(' ');
            var ownIndent = new string(' ', content.Length - body.Length);

            var indent = ownIndent;
            var line = new StringBuilder();

            foreach (var word in body.Split(' '))
            {
                var wouldOverflow =
                    indent.Length + line.Length + 1 + word.Length > FrameContentWidth;
                if (line.Length > 0 && wouldOverflow)
                {
                    yield return indent + line;
                    indent = ownIndent + continuationIndent;
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
