using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using ToDoMvvm;

namespace ToDoWpfView
{
    /// <summary>
    /// Class to wrap round collection view source
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class WrappedCollectionViewSource<T> : IWrappedCollectionViewSource<T>
    {
        private readonly CollectionViewSource _source = new CollectionViewSource();
        private Predicate<object> _filter;

        public WrappedCollectionViewSource()
        {
            _filter = t => true;
            _source.Filter += ApplyFilter;
        }

        public object View
        {
            get
            {
                return _source.View;
            }
        }

        public void SetSource(IEnumerable<T> source)
        {
            _source.Source = source;
        }

        public void ChangeFilter(Predicate<object> predicate)
        {
            _filter = predicate;
            Refresh();
        }

        public IEnumerable<T> Items
        {
            get
            {
                return _source.View.Cast<T>();
                
            }
        }

        public void Refresh()
        {
            if (_source.View != null)
            {
                _source.View.Refresh();
            }
        }

        public bool IsEmpty()
        {
            
            return _source.View.IsEmpty;
        }

        private void ApplyFilter(object sender, FilterEventArgs e)
        {
            e.Accepted = _filter(e.Item);
        }
    }
}