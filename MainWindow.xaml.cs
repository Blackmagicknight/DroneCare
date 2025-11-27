using DroneCare.Objects;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace DroneCare
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //global definition of drone list and queues
        internal static Queue<Drone> regularService = new Queue<Drone>();
        internal static Queue<Drone> expressService = new Queue<Drone>();
        internal static List<Drone> finishedList = new List<Drone>();

        //runs main logic
        public MainWindow()
        {
            InitializeComponent();
        }

        //only allows numbers to be entered in specified text boxes
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            //gets current text in text box
            TextBox textBox = sender as TextBox;
            string currentText = textBox.Text.Insert(textBox.CaretIndex, e.Text);

            //uses regex to allow any amount of numbers before decimal and 0-2 numbers after decimal
            //stackoverflow.com/questions/5978351
            Regex regex = new Regex(@"^[0-9]*(\.[0-9]{0,2})?$");
            if (!regex.IsMatch(currentText)) e.Handled = true;
        }

        //clears all text boxes and resets radio buttons for new entry
        private void ClearFields()
        {

            TB_ClientName.Clear();
            TB_DroneModel.Clear();
            TB_ServiceProblem.Clear();
            TB_ServiceCost.Text = "0";
            RBT_Regular.IsChecked = true;
        }

        //increments service tag for next entry
        private void IncrementServiceTag() => UD_ServiceTag.Value += 10;

        //returns the value if the priority radio group
        private string GetServicePriority()
        {
            if      (RBT_Regular.IsChecked == true) return "Regular";
            else if (RBT_Express.IsChecked == true) return "Express";

            else return "Unknown";
        }

        //adds a new servce item to the appropriate queue
        private void AddNewItem(object sender, RoutedEventArgs e)
        {
            //retrieves user input from text boxes  
            string clientName = TB_ClientName.Text;
            string droneModel = TB_DroneModel.Text;
            string serviceProblem = TB_ServiceProblem.Text;
            double serviceCost = double.Parse(TB_ServiceCost.Text);
            int serviceTag = int.Parse(UD_ServiceTag.Text);

            //increments service tag for next entry
            IncrementServiceTag();

            //creates new drone object
            Drone newDrone = new Drone(clientName, droneModel, serviceProblem, serviceCost, serviceTag);

            //adds new drone to the appropriate queue based on service type selected
            switch (GetServicePriority())
            {
                case "Regular":
                    regularService.Enqueue(newDrone);
                    break;
                case "Express":
                    //adds 15% to cost for express service
                    newDrone.serviceCost *= 1.15;
                    expressService.Enqueue(newDrone);
                    break;
            }

            //MOVE ME
            DisplayExpressQueue();
            DisplayReqularQueue();
        }

        //display all the elements in the regularService queue
        private void DisplayReqularQueue()
        {
            LVW_RegularQueue.Items.Clear();
            regularService.ToList().ForEach(drone => LVW_RegularQueue.Items.Add(drone));
        }

        //display all the elements in the expressService queue
        private void DisplayExpressQueue()
        {
            LVW_ExpressQueue.Items.Clear();
            expressService.ToList().ForEach(drone => LVW_ExpressQueue.Items.Add(drone));
        }

        //displays selected regular drone details in text boxes
        private void DisplaySelectedRegularDrone(object sender, SelectionChangedEventArgs e)
        {
            //gets selected drone from list view
            Drone? selectedDrone = LVW_RegularQueue.SelectedItem as Drone;

            //clears text boxes
            ClearFields();

            //sets text boxes to selected drone details
            if (selectedDrone != null)
            {
                TB_ClientName.Text = selectedDrone.clientName;
                TB_ServiceProblem.Text = selectedDrone.serviceProblem;
            }
        }
    }
}