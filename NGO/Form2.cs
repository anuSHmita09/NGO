using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace NGO
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            cmbBloodGroup.Items.AddRange(new string[] { "A+", "A-", "B+", "B-", "O+", "O-", "AB+", "AB-" });
            cmbBloodGroup.SelectedIndex = 0;
            dtpLastDonation.Value = DateTime.Today;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // VALIDATIONS
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Name cannot be empty!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }

            if (numAge.Value < 18)
            {
                MessageBox.Show("Donor must be at least 18 years old.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numAge.Value = 18;
                return;
            }

            if (cmbBloodGroup.SelectedItem == null)
            {
                MessageBox.Show("Please select a blood group!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbBloodGroup.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtContact.Text) || !IsValidContact(txtContact.Text))
            {
                MessageBox.Show("Invalid contact number! Only numbers, '+', and spaces are allowed.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtContact.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtCity.Text))
            {
                MessageBox.Show("City cannot be empty!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCity.Focus();
                return;
            }

            if (!checkBox1.Checked)
            {
                MessageBox.Show("Please confirm that the details are correct before saving.", "Confirmation Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // DATABASE INSERTION
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\NGO.mdf;Integrated Security=True;Connect Timeout=30";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    string query = "INSERT INTO Donors (Name, Age, BloodGroup, Contact, City, LastDonationDate) VALUES (@Name, @Age, @BloodGroup, @Contact, @City, @LastDonation)";
                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@Name", txtName.Text);
                    cmd.Parameters.AddWithValue("@Age", numAge.Value);
                    cmd.Parameters.AddWithValue("@BloodGroup", cmbBloodGroup.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@Contact", txtContact.Text);
                    cmd.Parameters.AddWithValue("@City", txtCity.Text);
                    cmd.Parameters.AddWithValue("@LastDonation", dtpLastDonation.Value.Date);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Donor details saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // CONTACT VALIDATION METHOD
        private bool IsValidContact(string contact)
        {
            return Regex.IsMatch(contact, @"^[0-9+\s]+$");
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            button1.Enabled = checkBox1.Checked;
        }

        private void cmbBloodGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbBloodGroup.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a valid blood group.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Corrected Contact Number Validation
        private void txtContact_TextChanged(object sender, EventArgs e)
        {
            string validText = Regex.Replace(txtContact.Text, @"[^0-9+\s]", "");
            if (txtContact.Text != validText)
            {
                txtContact.Text = validText;
                txtContact.SelectionStart = txtContact.Text.Length;
            }
        }

        // Corrected Name Validation
        private void txtName_TextChanged(object sender, EventArgs e)
        {
            string validText = Regex.Replace(txtName.Text, @"[^a-zA-Z\s]", "");
            if (txtName.Text != validText)
            {
                txtName.Text = validText;
                txtName.SelectionStart = txtName.Text.Length;
            }
        }

        // Corrected City Validation
        private void txtCity_TextChanged(object sender, EventArgs e)
        {
            string validText = Regex.Replace(txtCity.Text, @"[^a-zA-Z\s]", "");
            if (txtCity.Text != validText)
            {
                txtCity.Text = validText;
                txtCity.SelectionStart = txtCity.Text.Length;
            }
        }

        private void dtpLastDonation_ValueChanged(object sender, EventArgs e)
        {
            if (dtpLastDonation.Value > DateTime.Now)
            {
                MessageBox.Show("Last Donation Date cannot be in the future!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpLastDonation.Value = DateTime.Now;
            }
        }

        private void label2_MouseHover(object sender, EventArgs e)
        {
            label2.ForeColor = Color.Red;
        }

        private void label2_MouseLeave(object sender, EventArgs e)
        {
            label2.ForeColor = Color.Black;
        }

        private void numAge_ValueChanged(object sender, EventArgs e)
        {
            if (numAge.Value < 18)
            {
                MessageBox.Show("Donor must be at least 18 years old.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numAge.Value = 18; // Reset to 18 if the user selects a lower value.
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Label2 was clicked!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {
            //MessageBox.Show("You entered GroupBox!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


    }
}
