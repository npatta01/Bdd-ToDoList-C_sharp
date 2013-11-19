namespace ToDoMvvm
{
   public interface ICollectionViewSourceFactory
   {
       /// <summary>
       /// Create a TaskItem CollectionView
       /// </summary>
       /// <returns></returns>
       IWrappedCollectionViewSource<TaskItem> CreateTaskListViewSource();
   }
}
