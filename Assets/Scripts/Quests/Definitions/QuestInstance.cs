using System.Collections.Generic;
using System.Linq;
using Quests.Definitions.Tasks;
using Quests.Interfaces;
using UnityEngine;

namespace Quests.Definitions
{
    public class QuestInstance : IQuestInstance 
    {
        private readonly List<ITaskInstance> _tasks = new();
        private int _taskIndex;

        public IQuestDefinition Definition { get; }
        public string Title => Definition.Title;
        public string Description => Definition.Description;
        public IReadOnlyList<ITaskInstanceReadOnly> Tasks => _tasks;
        public QuestStatus Status => _taskIndex >= _tasks.Count ? QuestStatus.Complete : QuestStatus.Ongoing;

        public ITaskInstance ActiveTask 
        {
            get 
            {
                if (_tasks.Count == 0) return null;
                return Status == QuestStatus.Complete ? _tasks[^1] : _tasks[_taskIndex];
            }
        }

        public QuestInstance(IQuestDefinition definition) 
        {
            Definition = definition;
            PopulateTasks(definition.Tasks);
        }

        public void SetTask(ITaskDefinition task) 
        {
            _taskIndex = _tasks.FindIndex((t) => t.Definition == task);

            for (var i = 0; i < Tasks.Count; i++) {
                if (_taskIndex == i) {
                    ActiveTask.Begin();
                    continue;
                }

                if (i < _taskIndex) {
                    _tasks[i].Complete();
                    continue;
                }

                _tasks[i].ClearStatus();
            }
        }

        public void Next() 
        {
            if (Status == QuestStatus.Complete) return;

            var prev = ActiveTask;
            ActiveTask.Complete();
            _taskIndex += 1;

            if (prev != ActiveTask) 
            {
                ActiveTask?.Begin();
            }
        }

        public string Save() 
        {
            var data = new QuestInstanceSave 
            {
                TaskIndex = _taskIndex,
                Tasks = _tasks.Select(t => new QuestInstanceTaskSave 
                {
                    id = t.Id,
                    save = t.Save(),
                }).ToList(),
            };

            return JsonUtility.ToJson(data);
        }

        public void Load(string save) 
        {
            var data = JsonUtility.FromJson<QuestInstanceSave>(save);
            _taskIndex = data.TaskIndex;
            data.Tasks.ForEach(t => {
                var instance = _tasks.Find(i => i.Id == t.id);
                instance.Load(t.save);
            });
        }

        public void Complete() 
        {
            while (Status != QuestStatus.Complete) 
            {
                Next();
            }
        }

        private void PopulateTasks(IReadOnlyCollection<ITaskDefinition> tasks) 
        {
            if (tasks.Count == 0) return;

            foreach (var task in tasks) 
            {
                _tasks.Add(new TaskInstance(task));
            }

            _tasks[0].Begin();
        }
    }
}