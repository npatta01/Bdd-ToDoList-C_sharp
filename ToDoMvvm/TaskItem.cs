using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using System;

namespace ToDoMvvm
{
    public class TaskItem : ViewModelBase
    {

        #region fields
        //description
        private string _description;
        //task status
        private bool _completed;
        #endregion 

        

       /// <summary>
       /// Create a task with the given id and 
       /// </summary>
       /// <param name="id"></param>
       /// <param name="description"></param>
        public TaskItem(int id, string description)
            : this(id, description, false)
        {
        }

        /// <summary>
        /// Json Deserialization constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        /// <param name="completed"></param>
        [JsonConstructor]
        public TaskItem(int id, string description, bool completed)
        {
            Id = id;
            Description = description;
            Completed = completed;
        }

       
        /// <summary>
        /// Task Completion status
        /// </summary>
        public Boolean Completed
        {
            get { return _completed; }
            set
            {
                _completed = value;
              
                RaisePropertyChanged(() => Completed);
            }
        }

        /// <summary>
        /// Task Id
        /// </summary>
       public int Id { get; private set; }
       

        

        /// <summary>
        /// Task Description
        /// </summary>
        public String Description
        {
            get { return _description; }
            set
            {
                _description = value;
                RaisePropertyChanged(() => Description);
            }
        }

        
    }
}