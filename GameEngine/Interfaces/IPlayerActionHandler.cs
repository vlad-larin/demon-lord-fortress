using GameCore.Interfaces;
using GameCore.Models;
using GameEngine.Models;

namespace GameEngine.Interfaces
{
    internal interface IPlayerActionHandler
    {
        PlayerActionResult HandlePlayerAction(IPlayerAction playerAction);
    }

    internal interface IPlayerActionHandler<TPlayerAction> : IPlayerActionHandler
        where TPlayerAction : IPlayerAction
    {
        PlayerActionResult HandlePlayerAction(TPlayerAction playerAction);
    }
}
