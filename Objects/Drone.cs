using System;
using System.Globalization;

namespace DroneCare.Objects
{
    internal class Drone
    {
        private string? _clientName { get; set; }
        private string? _droneModel { get; set; }
        private string? _serviceProblem { get; set; }
        private int _serviceCost    { get; set; }
        private int _serviceTag     { get; set; }


        TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
        public string? clientName
        {
            get { return _clientName; }
            set
            {
                if (value != null)
                    _clientName = textInfo.ToTitleCase(value.ToLower());
                else
                    _clientName = null;
            }
        }
        public string? serviceProblem
        {
            get { return _serviceProblem; }
            set
            {
                if (value != null)
                    _serviceProblem = textInfo.ToTitleCase(value.ToLower());
                else
                    _serviceProblem = null;
            }
        }
        public string? droneModel { get { return _droneModel; } set { _droneModel = value; } }
        public int serviceCost      { get { return _serviceCost; } set { _serviceCost = value; } }
        public int serviceTag       { get { return _serviceTag; } set { _serviceTag = value; } }


        public string Display()
        {
            return $"Client Name: {clientName}, Service Cost: {serviceCost}";
        }
    }
}