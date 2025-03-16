using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using Microsoft.VisualBasic;


namespace NGO
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData(); // Load data when form starts
        }

        private void LoadData()
        {
            // Use the correct path for your database file
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\NGO.mdf;Integrated Security=True;Connect Timeout=30";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    string query = "SELECT * FROM Donors"; // Ensure 'Donors' table exists
                    SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt; // Bind the data to DataGridView

                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("No records found in the Donors table.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading data: " + ex.Message);
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Ensure a valid row is clicked
            {
                string donorName = dataGridView1.Rows[e.RowIndex].Cells["Name"].Value.ToString();
                MessageBox.Show($"You clicked on donor: {donorName}");
            }
        }

        private void aDDDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 addDetailsForm = new Form2();
            addDetailsForm.ShowDialog();

            // Refresh DataGridView after adding new details
            LoadData();
        }

        private void dELETEDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Ask the user for the name to delete
            string donorName = Microsoft.VisualBasic.Interaction.InputBox("Enter the name of the donor to delete:",
                                                                           "Delete Donor",
                                                                           "");

            if (string.IsNullOrWhiteSpace(donorName))
            {
                MessageBox.Show("No name entered. Deletion canceled.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\NGO.mdf;Integrated Security=True;Connect Timeout=30";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                // Find matching records
                string query = "SELECT ID, Name, Age, BloodGroup, Contact, City FROM Donors WHERE Name = @Name";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Name", donorName);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("No donor found with this name.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // If only one donor exists, delete directly
                if (dt.Rows.Count == 1)
                {
                    int donorID = Convert.ToInt32(dt.Rows[0]["ID"]);

                    DialogResult confirm = MessageBox.Show($"Are you sure you want to delete {donorName}?",
                                                           "Confirm Deletion",
                                                           MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (confirm == DialogResult.Yes)
                    {
                        DeleteDonor(con, donorID);
                    }
                    return;
                }

                // If multiple donors exist, show selection dialog
                string donorSelection = "Multiple donors found with the same name:\n\n";

                foreach (DataRow row in dt.Rows)
                {
                    donorSelection += $"ID: {row["ID"]}, Age: {row["Age"]}, Blood Group: {row["BloodGroup"]}, Contact: {row["Contact"]}, City: {row["City"]}\n";
                }

                donorSelection += "\nEnter the ID of the donor you want to delete:";
                string inputID = Microsoft.VisualBasic.Interaction.InputBox(donorSelection, "Select Donor to Delete", "");

                if (int.TryParse(inputID, out int selectedID))
                {
                    // Check if the entered ID exists
                    if (dt.AsEnumerable().Any(row => Convert.ToInt32(row["ID"]) == selectedID))
                    {
                        DeleteDonor(con, selectedID);
                    }
                    else
                    {
                        MessageBox.Show("Invalid ID entered. Deletion canceled.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Invalid input. Please enter a valid numeric ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Helper method to delete donor and update table
        private void DeleteDonor(SqlConnection con, int donorID)
        {
            string deleteQuery = "DELETE FROM Donors WHERE ID = @ID";
            SqlCommand deleteCmd = new SqlCommand(deleteQuery, con);
            deleteCmd.Parameters.AddWithValue("@ID", donorID);

            deleteCmd.ExecuteNonQuery();
            MessageBox.Show("Donor details deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Refresh DataGridView if applicable
            LoadDonorData();

        }

        private void LoadDonorData()
        {
            // Code to reload and update DataGridView after deletion
        }
    }
}
