using ResRec.Models;

namespace ResRec.Services
{
    public class SimilarityCalculator
    {
        private readonly List<RestaurantData> _restaurants;
        private readonly RestaurantVectorizer _vectorizer;
        private readonly Dictionary<string, float[]> _vectors;

        public SimilarityCalculator(List<RestaurantData> restaurants)
        {
            _restaurants = restaurants;
            _vectorizer = new RestaurantVectorizer(restaurants);
            _vectors = new Dictionary<string, float[]>();

            foreach (var r in _restaurants)
            {
                if (!string.IsNullOrEmpty(r.PlaceID))
                    _vectors[r.PlaceID] = _vectorizer.Vectorize(r);
            }
        }

        public List<RestaurantData> Recommend(string targetPlaceId, int topN = 5)
        {
            if (!_vectors.ContainsKey(targetPlaceId))
                throw new ArgumentException($"Unknown PlaceID: {targetPlaceId}");

            var targetVector = _vectors[targetPlaceId];

            var similarities = new List<(RestaurantData restaurant, double score)>();

            foreach (var r in _restaurants)
            {
                if (r.PlaceID == targetPlaceId || string.IsNullOrEmpty(r.PlaceID)) continue;

                var vector = _vectors[r.PlaceID];
                var similarity = CosineSimilarity(targetVector, vector);
                similarities.Add((r, similarity));
            }

            return similarities
                .OrderByDescending(x => x.score)
                .Take(topN)
                .Select(x => x.restaurant)
                .ToList();
        }

        private double CosineSimilarity(float[] v1, float[] v2)
        {
            double dot = 0, normA = 0, normB = 0;

            for (int i = 0; i < v1.Length; i++)
            {
                dot += v1[i] * v2[i];
                normA += v1[i] * v1[i];
                normB += v2[i] * v2[i];
            }

            return (normA == 0 || normB == 0) ? 0 : dot / (Math.Sqrt(normA) * Math.Sqrt(normB));
        }
    }
}