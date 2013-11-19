using NSpec;
using ToDoMvvm;

namespace ToDoSpecs.Specs
{
    class TaskListState_spec : nspec
    {

        /// <summary>
        /// checks basic properties
        /// </summary>
        private void correct_values()
        {
            it["All=0"] = () =>
            {
                const int val = (int)TaskListState.All;
                val.should_be(0);
            };


            it["Active=1"] = () =>
            {
                const int val = (int)TaskListState.Active;
                val.should_be(1);
            };

            it["Completed=2"] = () =>
            {
                const int val = (int)TaskListState.Completed;
                val.should_be(2);
            };

        }
    }
}
