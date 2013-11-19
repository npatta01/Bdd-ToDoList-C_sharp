using System;
using System.Collections.Generic;
using Microsoft.Practices.ServiceLocation;

namespace ToDoMvvm
{
    /// <summary>
    /// A wrapper around collectionv view source
    /// Suggestion from
    /// https://gist.github.com/onovotny/5008551
    /// </summary>
    public interface IWrappedCollectionViewSource<T>
    {
        /// <summary>
        /// Collection View object
        /// </summary>
        object View { get; }

        /// <summary>
        /// set collection view sourcce
        /// </summary>
        /// <param name="source"></param>
        void SetSource(IEnumerable<T> source);
        //change the current filter
        void ChangeFilter(Predicate<object> predicate);
        //get the visuble items in the view
        IEnumerable<T> Items { get; }
        //refrest collection
        void Refresh();
        /// <summary>
        /// Is empty
        /// </summary>
        /// <returns></returns>
        bool IsEmpty();
    }

    /// <summary>
    /// CollectionView FActory
    /// </summary>
    public class CollectionViewSourceFactory : ICollectionViewSourceFactory
    {
        /// <summary>
        /// Create a TaskItem CollectionView
        /// </summary>
        /// <returns></returns>
        public IWrappedCollectionViewSource<TaskItem> CreateTaskListViewSource()
        {
            return ServiceLocator.Current.GetInstance<IWrappedCollectionViewSource<TaskItem>>();
        }
    }
}