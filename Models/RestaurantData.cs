namespace ResRec.Models
{
    public class RestaurantData
    {
        public string PlaceID { get; set; }
        public string Name { get; set; }
        public string Adress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }

        public string Alcohol { get; set; }
        public string Smoking { get; set; }
        public string DressCode { get; set; }
        public string Price { get; set; }
        public string Area { get; set; }
        public string Other { get; set; }

        public string Cuisine { get; set; }
        public string Payment { get; set; }
        public string OpenDays { get; set; }
        public string OpenHours { get; set; }
        public string Parking { get; set; }
    }
}