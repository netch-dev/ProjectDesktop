using System.Collections.Generic;

public class TaskQueue {
	private List<ITask> tasks = new List<ITask>();

	public void AddTask(ITask task) {
		tasks.Add(task);
	}

	public ITask GetNextTask() {
		foreach (ITask task in tasks) {
			if (task.IsAvailable()) {
				return task;
			}
		}
		return null;
	}
}
