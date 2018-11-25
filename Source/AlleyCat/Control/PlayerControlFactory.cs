using System.Collections.Generic;
using AlleyCat.Action;
using AlleyCat.Autowire;
using AlleyCat.Character;
using AlleyCat.Common;
using AlleyCat.Event;
using AlleyCat.View;
using Godot;
using LanguageExt;
using Microsoft.Extensions.Logging;
using static LanguageExt.Prelude;

namespace AlleyCat.Control
{
    [AutowireContext]
    public class PlayerControlFactory : GameObjectFactory<PlayerControl>
    {
        [Export]
        public bool Active { get; set; } = true;

        [Export]
        public ProcessMode ProcessMode { get; set; } = ProcessMode.Idle;

        [Node]
        public Option<IHumanoid> Character { get; set; }

        [Node]
        public Option<Camera> Camera { get; set; }

        [Service(local: true)]
        public IEnumerable<IPerspectiveView> Perspectives { get; set; } = Seq<IPerspectiveView>();

        [Service(local: true)]
        public IEnumerable<IAction> Actions { get; set; } = Seq<IAction>();

        [Node("Movement")]
        public Option<IInputBindings> MovementInput { get; set; }

        [Export] private NodePath _characterPath;

        [Export] private NodePath _cameraPath;

        protected override Validation<string, PlayerControl> CreateService(ILoggerFactory loggerFactory)
        {
            return new PlayerControl(
                Camera.IfNone(() => GetViewport().GetCamera()),
                Character | this.FindPlayer<IHumanoid>(),
                Perspectives,
                Actions,
                MovementInput,
                ProcessMode,
                this,
                Active,
                loggerFactory);
        }
    }
}