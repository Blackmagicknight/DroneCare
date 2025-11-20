using System;
using System.Globalization;

namespace DroneCare.Objects
{
    internal class Drone
    {
        //private drone attributes
        private string? _clientName { get; set; }
        private string? _droneModel { get; set; }
        private string? _serviceProblem { get; set; }
        private double _serviceCost { get; set; }
        private int _serviceTag     { get; set; }


        //TextInfo for title casing strings
        TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;

        //public drone attribute accessors/mutators
        public string? clientName
        {
            get { return _clientName; }
            set
            {
                if (value != null)
                    //converts client name to title case
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
                    //converts service problem to title case
                    _serviceProblem = textInfo.ToTitleCase(value.ToLower());
                else
                    _serviceProblem = null;
            }
        }
        public string? droneModel { get { return _droneModel; } set { _droneModel = value; } }
        public double serviceCost { get { return _serviceCost; } set { _serviceCost = value; } }
        public int serviceTag     { get { return _serviceTag; } set { _serviceTag = value; } }


        // drone constuctor that initializes all attributes
        public Drone(string? clientName, string? droneModel, string? serviceProblem,  int serviceCost, int serviceTag)
        {
            this.clientName     = clientName;
            this.droneModel     = droneModel;
            this.serviceProblem = serviceProblem;
            this.serviceCost    = serviceCost;
            this.serviceTag     = serviceTag;
        }

        //returns a string for client name and service cost
        public string Display()
        {
            return $"Client Name: {clientName}, Service Cost: ${serviceCost}";
        }
    }
}