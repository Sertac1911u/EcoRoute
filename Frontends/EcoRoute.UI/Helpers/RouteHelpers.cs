using EcoRoute.DtoLayer.RouteOptimizationDtos;

namespace EcoRoute.UI.Helpers
{
    public static class RouteHelpers
    {
        public static string GetStatusText(RouteStatus status)
        {
            return status switch
            {
                RouteStatus.Scheduled => "Planlanmış",
                RouteStatus.Active => "Aktif",
                RouteStatus.Completed => "Tamamlanmış",
                _ => status.ToString()
            };
        }

        public static string GetStatusBadgeClass(RouteStatus status)
        {
            return status switch
            {
                RouteStatus.Scheduled => "bg-blue-100 text-blue-800 dark:bg-blue-900/20 dark:text-blue-300",
                RouteStatus.Active => "bg-green-100 text-green-800 dark:bg-green-900/20 dark:text-green-300",
                RouteStatus.Completed => "bg-gray-100 text-gray-800 dark:bg-gray-900/20 dark:text-gray-300",
                _ => "bg-gray-100 text-gray-800 dark:bg-gray-700 dark:text-gray-300"
            };
        }

        public static string GetOptimizationTypeText(OptimizationType type)
        {
            return type switch
            {
                OptimizationType.EnKisaMesafe => "En Kısa Mesafe",
                OptimizationType.DolulukOncelikli => "Doluluk Öncelikli",
                _ => type.ToString()
            };
        }

        public static string GetWasteTypeText(WasteType type)
        {
            return type switch
            {
                WasteType.Cop => "Çöp",
                WasteType.GeriDonusum => "Geri Dönüşüm",
                WasteType.Organik => "Organik Atık",
                WasteType.Cam => "Cam",
                WasteType.Metal => "Metal",
                WasteType.Elektronik => "Elektronik Atık",
                WasteType.Tehlikeli => "Tehlikeli Atık",
                _ => type.ToString()
            };
        }

        public static string GetProgressColor(double percentage)
        {
            return percentage switch
            {
                >= 100 => "text-green-500",
                >= 75 => "text-blue-500",
                >= 50 => "text-yellow-500",
                >= 25 => "text-orange-500",
                _ => "text-red-500"
            };
        }

        public static string GetFillLevelBadgeClass(double? fillLevel)
        {
            if (!fillLevel.HasValue) return "bg-gray-100 text-gray-800";

            return fillLevel switch
            {
                >= 90 => "bg-red-100 text-red-800 dark:bg-red-900/20 dark:text-red-400",
                >= 70 => "bg-orange-100 text-orange-800 dark:bg-orange-900/20 dark:text-orange-400",
                >= 50 => "bg-yellow-100 text-yellow-800 dark:bg-yellow-900/20 dark:text-yellow-400",
                _ => "bg-green-100 text-green-800 dark:bg-green-900/20 dark:text-green-400"
            };
        }

        public static string FormatDateTime(DateTime dateTime)
        {
            return dateTime.ToString("dd.MM.yyyy HH:mm");
        }

        public static string GetStepStatusClass(RouteStepDto step, bool isSimulating)
        {
            if (step.IsCompleted)
                return "bg-green-100 text-green-800 dark:bg-green-900/20 dark:text-green-300";

            if (isSimulating)
                return "bg-blue-100 text-blue-800 dark:bg-blue-900/20 dark:text-blue-300 animate-pulse";

            return "bg-yellow-100 text-yellow-800 dark:bg-yellow-900/20 dark:text-yellow-300";
        }

        public static string GetStepStatusText(RouteStepDto step, bool isSimulating)
        {
            if (step.IsCompleted)
                return "Tamamlandı";

            if (isSimulating)
                return "İşlemede";

            return "Beklemede";
        }

        public static string GetStepStatusIcon(RouteStepDto step, bool isSimulating)
        {
            if (step.IsCompleted)
                return "✅";

            if (isSimulating)
                return "🔄";

            return "⏳";
        }

        public static (int startPage, int endPage) GetPaginationRange(int currentPage, int totalPages, int maxVisiblePages = 5)
        {
            int startPage = Math.Max(1, currentPage - maxVisiblePages / 2);
            int endPage = Math.Min(totalPages, startPage + maxVisiblePages - 1);

            if (endPage - startPage < maxVisiblePages - 1)
            {
                startPage = Math.Max(1, endPage - maxVisiblePages + 1);
            }

            return (startPage, endPage);
        }
    }
}