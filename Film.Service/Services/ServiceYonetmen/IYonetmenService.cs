using Film.DTOs.Yönetmen;
using Film.Models;

namespace Film.Services.ServiceYonetmen
{
    public interface IYonetmenService
    {
        IEnumerable<Yönetmen> GetAllYonetmen();
        Yönetmen GetByID(int id);
        Yönetmen CreatYonetmen(YönetmenForInsertion yönetmenForInsertion);
        Yönetmen UpdateYonetmen(YönetmenForUpdate yönetmenForUpdate);
        bool DeleteYönetmen(int id);
    }
}
