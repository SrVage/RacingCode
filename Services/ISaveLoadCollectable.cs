namespace Code.Services
{
    public interface ISaveLoadCollectable
    {
        void SetList(int count);
        int[] GetList();
        void ChangeCollectableByID(int ID);
        void DeleteList();
        bool IsSaved();
    }
}