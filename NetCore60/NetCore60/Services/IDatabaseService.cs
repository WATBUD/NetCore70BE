using NetCore60.Models;

namespace NetCore60.Services
{
    public interface IDatabaseService
    {
        List<TodoItem> GetItems();
        string Connect();
        //List<YourModel> GetItems();
        //void InsertItem(YourModel item);
        // 其他方法...
    }
}
