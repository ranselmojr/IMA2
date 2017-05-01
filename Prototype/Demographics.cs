﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Prototype
{
    public partial class Demographics : Form
    {
        private string msgMissing = "The student ID entered does not match any student in the system; please verify." +
            "\nValid ids include: 123456, 223344, 555555, and 987654";
        private string[] studentArray;
        private List<Student> students;
        Student candidate = null;

        public Demographics()
        {
            InitializeComponent();
        }

        private void Demographics_Load(object sender, EventArgs e)
        {
            btnCancel_Click(null, null);
            // read input file and fill in State, Country; load a set of 3 to 5 valid students
            cmbState.Items.Add("California");
            cmbState.Items.Add("Arizona");
            cmbState.Items.Add("North Carolina");
            cmbState.Items.Add("Illinois");
            cmbState.Items.Add("Indiana");
            cmbState.Items.Add("Massachusetts");
            cmbState.Sorted = true;
            cmbCountry.Items.Add("United States");
            cmbCountry.Items.Add("Mexico");
            cmbCountry.Items.Add("Bahamas");
            cmbCountry.Items.Add("Canada");
            cmbCountry.Sorted = true;
            students = Student.InitializeFromFile();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            btnCancel.Enabled = false;
            btnEdit.Enabled = false;
            btnSave.Enabled = false;
            btnView.Enabled = false;
            txtStudent.Clear();
            txtLast.Clear();
            txtFirst.Clear();
            txtAddress1.Clear();
            txtAddress1.ReadOnly = true;
            txtAddress2.Clear();
            txtAddress2.ReadOnly = true;
            txtCity.Clear();
            txtCity.ReadOnly = true;
            cmbState.Text = "";
            cmbState.Enabled = false;
            cmbCountry.Text = "";
            cmbCountry.Enabled = false;
            txtZip.Clear();
            txtZip.ReadOnly = true;
            txtYear.Clear();
            txtCredits.Clear();
            txtGPA.Clear();
            txtDeans.Clear();
            txtNotes.Clear();
            txtNotes.Enabled = false;
            ClearRadioButtons();
        }

        private void ClearRadioButtons()
        {
            btnMale.Checked = false;
            btnFemale.Checked = false;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            txtAddress1.ReadOnly = false;
            txtAddress2.ReadOnly = false;
            txtCity.ReadOnly = false;
            cmbState.Enabled = true;
            cmbCountry.Enabled = true;
            txtZip.ReadOnly = false;
            btnSave.Enabled = true;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {      
            candidate.Addr1 = txtAddress1.Text;
            candidate.Addr2 = txtAddress2.Text;              
            candidate.City = txtCity.Text;              
            candidate.State = cmbState.Text;            
            candidate.Country = cmbCountry.Text;
            candidate.Zip = Int32.Parse(txtZip.Text);
            btnSave.Enabled = false;
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            String id = txtStudent.Text;
            Int32 target = Int32.Parse(id);
            candidate = null;
            btnCancel_Click(null, null);
            foreach (Student student in students)
            {
                if (student.Id == target)
                {
                    candidate = student;
                    break;
                }
            }
            if (candidate == null)
            {
                MessageBox.Show(msgMissing);
                return;
            }
            txtStudent.Text = id;
            txtLast.Text = candidate.Last;
            txtFirst.Text = candidate.First;
            txtAddress1.Text = candidate.Addr1;
            txtAddress2.Text = candidate.Addr2;
            txtCity.Text = candidate.City;
            cmbState.Text = candidate.State;
            cmbCountry.Text = candidate.Country;
            txtZip.Text = candidate.Zip.ToString();
            txtYear.Text = candidate.Year;
            txtCredits.Text = candidate.Credits.ToString();
            txtGPA.Text = candidate.Gpa.ToString();
            txtDeans.Text = candidate.Deans.ToString();
            txtNotes.Text = candidate.Notes;
            ClearRadioButtons();
            if (candidate.Gender == 0)
                btnMale.Checked = true;
            else
                btnFemale.Checked = true;
            btnEdit.Enabled = true;
        }

        private void txtStudent_TextChanged(object sender, EventArgs e)
        {
            if (txtStudent.Text.Length == 6)
            {
                btnCancel.Enabled = true;
                btnView.Enabled = true;
            } else
            {
                btnCancel.Enabled = false;
                btnView.Enabled = false;
            }
        }
    }

    // a DAO object to hold configured values of programs and minimum requirements
    class Student
    {
        private static string fileName = "students.txt";

        public Int32 Id { get; set; }
        public string Last { get; set; }
        public string First { get; set; }
        public string Addr1 { get; set; }
        public string Addr2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public Int32 Zip { get; set; }
        public string Country { get; set; }
        public string Year { get; set; }
        public Int32 Credits { get; set; }
        public Double Gpa { get; set; }
        public Boolean Deans { get; set; }
        public Int32 Gender { get; set; }
        public string Notes { get; set; }

        public Student(string values)
        {
            string[] valueList = values.Split('\t');
            Id = Int32.Parse(valueList[0]);
            Last = valueList[1];
            First = valueList[2];
            Addr1 = valueList[3];
            Addr2 = valueList[4];
            City = valueList[5];
            State = valueList[6];
            Zip = Int32.Parse(valueList[7]);
            Country = valueList[8];
            Year = valueList[9];
            Credits = Int32.Parse(valueList[10]);
            Gpa = Double.Parse(valueList[11]);
            Deans = Boolean.Parse(valueList[12]);
            Gender = Int32.Parse(valueList[13]);
            Notes = valueList[14];
        }

        public static List<Student> InitializeFromFile()
        {
            string[] lines = null;
            try
            {
                lines = File.ReadAllLines("..\\..\\students.txt");
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show("Ensure that the file '" + fileName + "' is in the project directory.");
                return null;
            }
            if (lines.Length <= 0) {
                MessageBox.Show("There were no data in the input file '" + fileName + "'.  Please ensure it is valid.");
                return null;
            }

            List<Student> students = new List<Student>();
            foreach (string line in lines)
            {
                if (line.StartsWith("#") || line.Length == 0)  // ignore comments
                    continue;
                students.Add(new Student(line));
            }
            return students;
        }
    }
}



