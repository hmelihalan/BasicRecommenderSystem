using System.Text;
using ResRec.Models;

namespace ResRec.Services
{
    public class RestaurantVectorizer
    {
        private readonly List<string> _allCuisines = new();
        private readonly List<string> _allPayments = new();
        private readonly List<string> _allParkingTypes = new();
        private readonly List<string> _allAlcoholOptions = new();
        private readonly List<string> _allSmokingOptions = new();
        private readonly List<string> _allPriceOptions = new();

        public RestaurantVectorizer(IEnumerable<RestaurantData> restaurants)
        {
            foreach (var restaurant in restaurants)
            {
                AddToUniqueList(_allCuisines, restaurant.Cuisine);
                AddToUniqueList(_allPayments, restaurant.Payment);
                AddToUniqueList(_allSmokingOptions, restaurant.Smoking);     // ← EKLENDİ
                AddToUniqueList(_allPriceOptions, restaurant.Price);         // ← EKLENDİ
            }
        }

        private void AddToUniqueList(List<string> list, string? values)
        {
            if (string.IsNullOrWhiteSpace(values)) return;

            var items = values.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            foreach (var item in items)
            {
                if (!list.Contains(item))
                    list.Add(item);
            }
        }

        public float[] Vectorize(RestaurantData restaurant)
        {
            var vector = new List<float>();

            // Cuisine (multi-label)
            vector.AddRange(_allCuisines.Select(c => ContainsValue(restaurant.Cuisine, c) ? 1f : 0f));

            // Payment (multi-label)
            vector.AddRange(_allPayments.Select(p => ContainsValue(restaurant.Payment, p) ? 1f : 0f));

            // Parking (single)
            vector.AddRange(_allParkingTypes.Select(p => string.Equals(restaurant.Parking?.Trim(), p, StringComparison.OrdinalIgnoreCase) ? 1f : 0f));

            // Alcohol (single)
            vector.AddRange(_allAlcoholOptions.Select(a => string.Equals(restaurant.Alcohol?.Trim(), a, StringComparison.OrdinalIgnoreCase) ? 1f : 0f));

            // Smoking (single) ← YENİ
            vector.AddRange(_allSmokingOptions.Select(s => string.Equals(restaurant.Smoking?.Trim(), s, StringComparison.OrdinalIgnoreCase) ? 1f : 0f));

            // Price (single) ← YENİ
            vector.AddRange(_allPriceOptions.Select(p => string.Equals(restaurant.Price?.Trim(), p, StringComparison.OrdinalIgnoreCase) ? 1f : 0f));

            return vector.ToArray();
        }

        private bool ContainsValue(string? field, string value)
        {
            if (string.IsNullOrWhiteSpace(field)) return false;
            return field.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                        .Any(x => x.Equals(value, StringComparison.OrdinalIgnoreCase));
        }
    }
}