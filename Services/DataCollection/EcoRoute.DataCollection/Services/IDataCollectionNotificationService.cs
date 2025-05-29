using EcoRoute.DataCollection.Dtos.WasteBinDtos;

namespace EcoRoute.DataCollection.Services
{
    public interface IDataCollectionNotificationService
    {
        Task SendWasteBinCreatedNotificationAsync(object wasteBinDto);
        Task SendWasteBinUpdatedNotificationAsync(object wasteBinDto);
        Task SendWasteBinDeletedNotificationAsync(object wasteBinDto);
    }
}
