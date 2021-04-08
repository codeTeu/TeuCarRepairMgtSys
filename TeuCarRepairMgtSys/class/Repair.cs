namespace TeuCarRepairMgtSys
{
    class Repair
    {

        public int Id { get; set; }
        public int VID { get; set; }
        public int Stock { get; set; }
        public double Price { get; set; }
        public double Cost { get; set; }

        public Repair()
        {
        }

        public Repair(int id, int vID, int stock, double price, double cost)
        {
            Id = id;
            VID = vID;
            Stock = stock;
            Price = price;
            Cost = cost;
        }
    }
}
