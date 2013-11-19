namespace ToDoMvvm
{
    /// <summary>
    /// Application setting 
    /// </summary>
    public interface IAppSettings
    {
        /// <summary>
        /// Task Database path
        /// </summary>
        string TaskDatabaseName { get; }
    }
}
