namespace Quests.Saving
{
    public interface ISaveQuests
    {

        object CaptureState();

        void RestoreState(object state);

    }
}
