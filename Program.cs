using ResRec.Services;
using ResRec.Models;

var restaurants = RestaurantDataLoader.LoadPlaces("/Users/hmelihalan/RiderProjects/ResRec/ResRec/Data/geoplaces2.csv");

var calculator = new SimilarityCalculator(restaurants);

var recommendations = calculator.Recommend("134999", 5); // Örnek placeID

Console.WriteLine("Önerilen restoranlar:");
foreach (var r in recommendations)
{
    Console.WriteLine($"{r.Name} - {r.City} - {r.Cuisine} - {r.Payment} - {r.Parking}");
}