using Newtonsoft.Json;
using NSpec;
using ToDoMvvm;

namespace ToDoSpecs.Specs
{
    internal class TodoItem_spec : nspec
    {
        /// <summary>
        /// checks basic properties
        /// </summary>
        private void basic_properties()
        {
            it["new task is false"] = () =>
                {
                    TaskItem t1 = new TaskItem(1, "description1");
                    t1.Completed.should_be_false();
                };
        }

        /// <summary>
        /// checks json parsing
        /// </summary>
        private void json_parsing_works()
        {
            it["all attributes specified"] = () =>
                {
                    TaskItem t1 = new TaskItem(1, "description1", false);
                    string json = JsonConvert.SerializeObject(t1);
                    TaskItem t2 = JsonConvert.DeserializeObject<TaskItem>(json);
                    t1.Completed.should_be(t2.Completed);
                    t1.Description.should_be(t1.Description);
                    t1.Id.should_be(t2.Id);
                };

            it["missing attributes"] = () =>
            {
                TaskItem t1 = new TaskItem(1, "description1");
                string json = JsonConvert.SerializeObject(t1);
                TaskItem t2 = JsonConvert.DeserializeObject<TaskItem>(json);
                t1.Completed.should_be(t2.Completed);
                t1.Description.should_be(t1.Description);
                t1.Id.should_be(t2.Id);
            };
        }
    }
}