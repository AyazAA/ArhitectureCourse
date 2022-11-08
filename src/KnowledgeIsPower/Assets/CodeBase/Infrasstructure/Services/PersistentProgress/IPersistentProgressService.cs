using CodeBase.Data;

namespace CodeBase.Infrasstructure.Services.PersistentProgress
{
    public interface IPersistentProgressService : IService
    { 
        PlayerProgress Progress { get; set; }
    }
}