namespace Fintranet.Models
{
    public interface IVehicle
    {
        String GetVehicleType();
    }

    public class Vehicle : IVehicle
    {
        private readonly string _name;

        public Vehicle(string name)
        {
            _name = name;
        }
        public string GetVehicleType()
        {
            return _name;
        }
    }
}
