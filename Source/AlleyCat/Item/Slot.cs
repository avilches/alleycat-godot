using AlleyCat.Common;
using AlleyCat.Condition.Generic;
using EnsureThat;
using LanguageExt;

namespace AlleyCat.Item
{
    public abstract class Slot : GameObject, ISlot
    {
        public string Key { get; }

        public virtual string DisplayName { get; }

        public Option<ICondition<ISlotItem>> AllowedCondition { get; }

        protected Slot(string key, string displayName, Option<ICondition<ISlotItem>> allowedCondition)
        {
            Ensure.That(key, nameof(key)).IsNotNullOrEmpty();
            Ensure.That(displayName, nameof(displayName)).IsNotNullOrEmpty();

            Key = key;
            DisplayName = displayName;
            AllowedCondition = allowedCondition;
        }

        public virtual bool AllowedFor(ISlotItem context)
        {
            Ensure.That(context, nameof(context)).IsNotNull();

            return !AllowedCondition.Exists(c => !c.Matches(context));
        }

        public bool AllowedFor(object context) => context is ISlotItem item && AllowedFor(item);
    }
}
