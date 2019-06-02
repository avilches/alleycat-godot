using System.Collections.Generic;
using AlleyCat.Action;
using AlleyCat.Character;
using AlleyCat.View;

namespace AlleyCat.Control
{
    public interface IPlayerControl : ICharacterAware<IHumanoid>, IPerspectiveSwitcher, IActor
    {
        IEnumerable<IInput> Inputs { get; }
    }
}
