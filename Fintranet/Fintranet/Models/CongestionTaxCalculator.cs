using Newtonsoft.Json;

namespace Fintranet.Models
{
    using System;
    using System.Collections.Generic;

    namespace CongestionTaxCalculator
    {
        public class CongestionTaxCalculator
        {
            private Dictionary<string, CityTaxRules>? _cityTaxRules;

            public CongestionTaxCalculator()
            {
                LoadTaxRules();
            }

            private void LoadTaxRules()
            {
                string? solutionFolderPath = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.Parent?.Parent?.Parent?.FullName;
                if (solutionFolderPath != null)
                {
                    string dataFolderPath = Path.Combine(solutionFolderPath, "Data");
                    string configFile = Path.Combine(dataFolderPath, "tax_rules.json"); ;
              
                    if (File.Exists(configFile))
                    {
                        string json = File.ReadAllText(configFile);
                        _cityTaxRules = JsonConvert.DeserializeObject<Dictionary<string, CityTaxRules>>(json);
                    }
                    else
                    {
                        // Handle the case when the configuration file is missing or invalid
                        throw new FileNotFoundException("Tax rules configuration file not found.");
                    }
                }
            }

            // Define the toll-free vehicles
            private static readonly HashSet<string> TollFreeVehicles = new HashSet<string>
            {
                "Motorcycle",
                "Tractor",
                "Emergency",
                "Diplomat",
                "Foreign",
                "Military"
            };

            // Calculate the total toll fee for one day
            public int CalculateCongestionTax(string city, IVehicle vehicle, DateTime[] dates)
            {
                if (!_cityTaxRules.ContainsKey(city))
                {
                    // Handle the case when tax rules for the specified city are not found
                    throw new ArgumentException($"Tax rules for city '{city}' are not defined.");
                }

                var taxRules = _cityTaxRules[city];
                int totalFee = 0;
                int intervalStartIndex = 0;
                bool isFirstPass = true;

                foreach (DateTime date in dates.OrderBy(d => d))
                {
                    if (!taxRules.IsExemptDate(date) && !IsTollFreeVehicle(vehicle))
                    {
                        int fee = taxRules.GetTollFee(date);

                        if (isFirstPass)
                        {
                            totalFee += fee;
                            isFirstPass = false;
                        }
                        else
                        {
                            int previousFee = taxRules.GetTollFee(dates[intervalStartIndex]);

                            if (IsWithinTollInterval(date, dates[intervalStartIndex]) && fee > previousFee)
                            {
                                totalFee -= previousFee;
                                totalFee += fee;
                            }
                            else
                            {
                                totalFee += fee;
                                intervalStartIndex++;
                            }
                        }
                    }
                }

                return Math.Min(totalFee, taxRules.MaxFeePerDay);
            }
            // Check if the date is toll-free
            private bool IsTollFreeDate(DateTime date)
            {
                int year = date.Year;
                int month = date.Month;
                int day = date.Day;

                if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                    return true;

                if (year == 2013)
                {
                    if (month == 1 && day == 1 ||
                        month == 3 && (day == 28 || day == 29) ||
                        month == 4 && (day == 1 || day == 30) ||
                        month == 5 && (day == 1 || day == 8 || day == 9) ||
                        month == 6 && (day == 5 || day == 6 || day == 21) ||
                        month == 7 ||
                        month == 11 && day == 1 ||
                        month == 12 && (day == 24 || day == 25 || day == 26 || day == 31))
                    {
                        return true;
                    }
                }

                return false;
            }

            private bool IsTollFreeVehicle(IVehicle vehicle)
            {
                if (vehicle == null)
                    return false;

                string vehicleType = vehicle.GetVehicleType();
                return TollFreeVehicles.Contains(vehicleType);
            }

            private bool IsWithinTollInterval(DateTime currentDate, DateTime intervalStartDate)
            {
                if (currentDate.Date != intervalStartDate.Date)
                    return false;

                TimeSpan currentTime = currentDate.TimeOfDay;
                TimeSpan intervalStartTime = intervalStartDate.TimeOfDay;

                return currentTime >= intervalStartTime;
            }

            //TODO Create an instance of the Vehicle based on the provided vehicle type
            private IVehicle GetVehicleInstance()
            {
                // Implement this method based on your vehicle class implementation
                // Return an instance of the Vehicle class based on the provided vehicle type
                // For example, if vehicleType is "Car", return a new Car instance
                // This allows you to dynamically create different vehicle instances

                return null; // Replace null with the appropriate vehicle instance based on the provided vehicle type
            }
        }
       
    }
}