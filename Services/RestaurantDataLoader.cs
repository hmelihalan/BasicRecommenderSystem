using System.Globalization;
using CsvHelper;
using ResRec.Models;

namespace ResRec.Services
{
    public static class RestaurantDataLoader
    {
        public static List<RestaurantData> LoadPlaces(string filePath)
        {
            var result = new List<RestaurantData>();

            var cuisineDict = LoadCuisine("/Users/hmelihalan/RiderProjects/ResRec/ResRec/Data/chefmozcuisine.csv");
            var paymentDict = LoadPayment("/Users/hmelihalan/RiderProjects/ResRec/ResRec/Data/chefmozaccepts.csv");
            var parkingDict = LoadParking("/Users/hmelihalan/RiderProjects/ResRec/ResRec/Data/chefmozparking.csv");

            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csv.GetRecords<dynamic>();

            foreach (var record in records)
            {
                var place = new RestaurantData
                {
                    PlaceID = record.placeID,
                    Name = record.name,
                    Adress = record.address,
                    City = record.city,
                    State = record.state,
                    Country = record.country,
                    Alcohol = record.alcohol,
                    Smoking = record.smoking_area,
                    DressCode = record.dress_code,
                    Price = record.price,
                    Area = record.area,
                    Other = record.other_services
                };

                if (cuisineDict.TryGetValue(place.PlaceID, out var cuisine))
                    place.Cuisine = cuisine;

                if (paymentDict.TryGetValue(place.PlaceID, out var payment))
                    place.Payment = payment;

                if (parkingDict.TryGetValue(place.PlaceID, out var parking))
                    place.Parking = parking;

                result.Add(place);
            }

            return result;
        }
        public static Dictionary<string, string> LoadCuisine(string filePath)
        {
            var result = new Dictionary<string, string>();

            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csv.GetRecords<dynamic>();
            

            foreach (var record in records)
            {
                string placeId = record.placeID;
                string cuisine = record.Rcuisine;

                if (!result.ContainsKey(placeId))
                    result[placeId] = cuisine;
                else
                    result[placeId] += ", " + cuisine; // Birden fazla varsa virgülle ayır
            }

            return result;
        }
        public static Dictionary<string, string> LoadPayment(string filePath)
        {
            var result = new Dictionary<string, string>();

            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csv.GetRecords<dynamic>();

            foreach (var record in records)
            {
                string placeId = record.placeID;
                string payment = record.Rpayment;

                if (!result.ContainsKey(placeId))
                    result[placeId] = payment;
                else
                    result[placeId] += ", " + payment;
            }

            return result;
        }

        public static Dictionary<string, string> LoadParking(string filePath)
        {
            var result = new Dictionary<string, string>();

            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csv.GetRecords<dynamic>();

            foreach (var record in records)
            {
                string placeId = record.placeID;
                string parking = record.parking_lot;

                if (!result.ContainsKey(placeId))
                    result[placeId] = parking;
                else
                    result[placeId] += ", " + parking;
            }

            return result;
        }
    }
}