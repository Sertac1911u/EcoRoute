using EcoRoute.DtoLayer.WasteBinDtos;

namespace EcoRoute.UI.Helpers
{
    public static class BinUIHelpers
    {
        // Doluluk hesaplaması için sabitler
        private const double DAILY_FILL_RATE = 5.0; // Günlük ortalama doluluk artışı yüzdesi

        #region Device Status Helpers
        public static string GetDeviceStatusColor(string status)
        {
            return status switch
            {
                "Active" => "bg-green-100 text-green-800 dark:bg-green-900/20 dark:text-green-300",
                "Inactive" => "bg-gray-100 text-gray-800 dark:bg-gray-900/20 dark:text-gray-300",
                "Maintenance" => "bg-yellow-100 text-yellow-800 dark:bg-yellow-900/20 dark:text-yellow-300",
                "Faulty" => "bg-red-100 text-red-800 dark:bg-red-900/20 dark:text-red-300",
                _ => "bg-gray-100 text-gray-800 dark:bg-gray-700 dark:text-gray-300"
            };
        }

        public static string GetDeviceStatusTextColor(string status)
        {
            return status switch
            {
                "Active" => "text-green-600 dark:text-green-400",
                "Inactive" => "text-gray-600 dark:text-gray-400",
                "Maintenance" => "text-yellow-600 dark:text-yellow-400",
                "Faulty" => "text-red-600 dark:text-red-400",
                _ => "text-gray-600 dark:text-gray-400"
            };
        }

        public static string GetDeviceStatusText(string status)
        {
            return status switch
            {
                "Active" => "Aktif",
                "Inactive" => "Pasif",
                "Maintenance" => "Bakımda",
                "Faulty" => "Arızalı",
                _ => status
            };
        }
        #endregion

        #region Fill Level Helpers
        public static string GetFillLevelColor(double? fillLevel)
        {
            if (!fillLevel.HasValue) return "bg-gray-500";

            return fillLevel switch
            {
                >= 90 => "bg-red-500",
                >= 70 => "bg-orange-500",
                >= 50 => "bg-yellow-500",
                >= 30 => "bg-blue-500",
                _ => "bg-green-500"
            };
        }

        public static string GetFillLevelBadge(double? fillLevel)
        {
            if (!fillLevel.HasValue) return "bg-gray-100 text-gray-800";

            return fillLevel switch
            {
                >= 90 => "bg-red-100 text-red-800 dark:bg-red-900/20 dark:text-red-400",
                >= 70 => "bg-orange-100 text-orange-800 dark:bg-orange-900/20 dark:text-orange-400",
                >= 50 => "bg-yellow-100 text-yellow-800 dark:bg-yellow-900/20 dark:text-yellow-400",
                >= 30 => "bg-blue-100 text-blue-800 dark:bg-blue-900/20 dark:text-blue-400",
                _ => "bg-green-100 text-green-800 dark:bg-green-900/20 dark:text-green-400"
            };
        }

        public static string GetFillStatusBadge(double? fillLevel)
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

        public static string GetFillStatusText(double? fillLevel)
        {
            if (!fillLevel.HasValue) return "Durum Bilinmiyor";

            return fillLevel switch
            {
                >= 90 => "Acil Boşaltılmalı",
                >= 70 => "Yakında Boşaltılmalı",
                >= 50 => "Orta Doluluk",
                _ => "Boşaltma Gerekmiyor"
            };
        }

        public static string GetTextColorByFillLevel(double? fillLevel)
        {
            if (!fillLevel.HasValue) return "text-gray-500";

            return fillLevel switch
            {
                >= 90 => "text-red-500",
                >= 70 => "text-orange-500",
                >= 50 => "text-yellow-500",
                >= 30 => "text-blue-500",
                _ => "text-green-500"
            };
        }

        public static string GetStrokeFillColor(double? fillLevel)
        {
            if (!fillLevel.HasValue) return "text-gray-500";

            return fillLevel switch
            {
                >= 90 => "text-red-500",
                >= 70 => "text-orange-500",
                >= 50 => "text-yellow-500",
                >= 30 => "text-blue-500",
                _ => "text-green-500"
            };
        }
        #endregion

        #region Bin Icon and Sensor Helpers
        public static string GetBinIconColor(ResultWasteBinDto bin)
        {
            return bin.DeviceStatus switch
            {
                "Active" => "text-green-500",
                "Inactive" => "text-gray-500",
                "Maintenance" => "text-yellow-500",
                "Faulty" => "text-red-500",
                _ => "text-gray-500"
            };
        }

        public static string GetSensorBorderColor(bool isActive)
        {
            return isActive ? "border-green-500" : "border-red-500";
        }
        #endregion

        #region Fill Rate Predictions
        public static double GetEstimatedFillRate(double? currentFillLevel, int daysFromNow)
        {
            if (!currentFillLevel.HasValue)
                return 0;

            double estimatedFill = currentFillLevel.Value + (DAILY_FILL_RATE * daysFromNow);
            return Math.Min(estimatedFill, 100);
        }

        public static string GetEstimatedFillDate(double? fillLevel)
        {
            if (!fillLevel.HasValue || fillLevel >= 100)
                return "Bilinmiyor";

            double remainingCapacity = 100 - fillLevel.Value;
            int daysUntilFull = (int)Math.Ceiling(remainingCapacity / DAILY_FILL_RATE);
            DateTime estimatedDate = DateTime.Now.AddDays(daysUntilFull);

            return estimatedDate.ToString("dd.MM.yyyy");
        }
        #endregion
    }

}
