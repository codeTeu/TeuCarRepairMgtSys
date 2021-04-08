namespace TeuCarRepairMgtSys
{
    class Repair
    {

        public int Id { get; set; }
        public int IID { get; set; }
        public string WhatToRepair { get; set; }

        public Repair()
        {
        }

        public Repair(int id, int iID, string whatToRepair)
        {
            Id = id;
            IID = iID;
            WhatToRepair = whatToRepair;
        }
    }
}
