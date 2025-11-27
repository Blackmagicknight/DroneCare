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

        //displays error message in error message text box
        private void ErrorMessage(string msg) => TB_ErrorMessage.Text = $"ERROR MESSAGE: {msg}";

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

            //checks for empty fields
            if (clientName == "" || droneModel == "" || serviceProblem == "")
            {
                ErrorMessage("Please fill in all fields before adding a new service item.");
                return;
            }

            //increments service tag for next entry
            IncrementServiceTag();

            //creates new drone object
            Drone newDrone = new Drone(clientName, droneModel, serviceProblem, serviceCost, serviceTag);

            //adds new drone to the appropriate queue based on service type selected
            switch (GetServicePriority())
            {
                case "Regular":
                    regularService.Enqueue(newDrone);
                    DisplayReqularQueue();
                    break;
                case "Express":
                    //adds 15% to cost for express service rounded to 2 decimal places
                    newDrone.serviceCost = Math.Round(newDrone.serviceCost * 1.15, 2);
                    expressService.Enqueue(newDrone);
                    DisplayExpressQueue();
                    break;
            }
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

        //display all the elements in the finishedList
        private void DisplayFinishedList()
        {
            LBX_FinishedList.Items.Clear();
            finishedList.ForEach(drone => LBX_FinishedList.Items.Add(drone));
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
            else ErrorMessage("The selected drone in regular queue is null and cannot be displayed");
        }

        //displays selected express drone details in text boxes
        private void DisplaySelectedExpressDrone(object sender, SelectionChangedEventArgs e)
        {
            //gets selected drone from list view
            Drone? selectedDrone = LVW_ExpressQueue.SelectedItem as Drone;

            //clears text boxes
            ClearFields();

            //sets text boxes to selected drone details
            if (selectedDrone != null)
            {
                TB_ClientName.Text = selectedDrone.clientName;
                TB_ServiceProblem.Text = selectedDrone.serviceProblem;
            }
            else ErrorMessage("The selected drone in express queue is null and cannot be displayed");
        }

        //removes drone from the regular queue and adds it to the finished list
        private void RemoveRegularQueue(object sender, RoutedEventArgs e)
        {
            //checks if regular queue is empty
            if (regularService.Count == 0)
            {
                ErrorMessage("The Regular Service queue is empty.");
                return;
            }

            //removes & displays regular queue
            Drone finishedDrone = regularService.Dequeue();
            DisplayReqularQueue();

            //resets and displays finished list
            finishedList.Add(finishedDrone);
            DisplayFinishedList();
        }

        //removes drone from the express queue and adds it to the finished list
        private void RemoveExpressQueue(object sender, RoutedEventArgs e)
        {
            //checks if express queue is empty
            if (expressService.Count == 0)
            {
                ErrorMessage("The Express Service queue is empty.");
                return;
            }

            //removes & displays express queue
            Drone finishedDrone = expressService.Dequeue();
            DisplayExpressQueue();

            //resets and displays finished list
            finishedList.Add(finishedDrone);
            DisplayFinishedList();
        }

        private void RemoveSelectedFinishedList(object sender, RoutedEventArgs e)
        {
            //gets selected drone from list box
            Drone? selectedDrone = LBX_FinishedList.SelectedItem as Drone;
            //checks if an item is selected
            if (selectedDrone == null)
            {
                ErrorMessage("Please select an item from the Finished List to remove.");
                return;
            }
            //removes selected drone from finished list and updates display
            finishedList.Remove(selectedDrone);
            DisplayFinishedList();
        }
    }
}