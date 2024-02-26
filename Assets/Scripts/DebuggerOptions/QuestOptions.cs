using System;
using System.ComponentModel;
using Quests.Core;
using Zenject;

namespace DebuggerOptions
{
    public class QuestOptions : IInitializable, IDisposable
    {
        private readonly IQuestSequence _questSequence;

        public QuestOptions(IQuestSequence questSequence)
        {
            _questSequence = questSequence;
        }

        [Category("Quests")]
        public void Initialize()
        {
            Dispose();
            SRDebug.Instance.AddOptionContainer(this);
        }

        [Category("Quests")]
        public void Dispose() => SRDebug.Instance.RemoveOptionContainer(this);

        [Category("Quests")]
        public void CompleteCurrentQuest() => _questSequence.CurrentQuest.Value?.MarkAsCompleted();
    }
}