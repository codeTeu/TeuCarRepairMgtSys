namespace TeuCarRepairMgtSys
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Cond { get; set; }

        public Vehicle()
        {
        }

        public Vehicle(int id, string make, string model, int year, string cond)
        {
            Id = id;
            Make = make;
            Model = model;
            Year = year;
            Cond = cond;
        }
    }
}
