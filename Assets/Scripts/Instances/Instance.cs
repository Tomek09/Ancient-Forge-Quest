using AncientForgeQuest.Models;

namespace AncientForgeQuest.Instances
{
    public class Instance<T> where T : Model 
    {
        public T BaseModel { get; private set; }
        
        public Instance(T baseModel)
        {
            BaseModel = baseModel;
        }
    }
}
