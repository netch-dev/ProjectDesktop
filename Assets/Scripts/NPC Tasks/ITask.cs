public interface ITask {
	bool IsAvailable(NPC npc);
	void ExecuteTask(NPC npc);
}