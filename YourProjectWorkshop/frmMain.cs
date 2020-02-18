using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WorkshopData;
using log4net;
using System.Diagnostics;

namespace YourProjectWorkshop
{
    public partial class frmMain : Form
    {
        #region init

        static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
       
        public frmMain()
        {
            InitializeComponent();
            ToolsGrid();
            BorrowersGrid();
            RentalGrid();
            ReportsGrid();
            //log4net.Config.XmlConfigurator.Configure();            
        }

        #endregion 
        
        #region Tools

        private void btnFindTools_Click(object sender, EventArgs e)
        {
            
            var record = new Tools
            {
                Id = Convert.ToInt64(txtIDTools.Text),
            };
            try
            {
                record = new Tools
                {
                    Id = Convert.ToInt64(txtIDTools.Text),
                };
                // reassign record to the found results
                record = Tools.Find(record);               
                txtDescriptionTools.Text = record.Description;
                txtAssetNumberTools.Text = record.AssetNumber.ToString();
                txtBrandTools.Text = record.Brand;
                txtCommentsTools.Text = record.Comments;
                chkActive.Checked = record.Active;
                chkAvailable.Checked = record.Available;
            }
            catch (Exception ex)
            {                
                log.Error($"Find Tool ID {record.Id}", ex);                
                MessageBox.Show($"There is no record ID {txtIDTools.Text}");
            }
        }

        private void btnNewTools_Click(object sender, EventArgs e)
        {
            txtIDTools.Text = "";
            txtDescriptionTools.Text = "";
            txtAssetNumberTools.Text = "";
            txtBrandTools.Text = "";
            txtCommentsTools.Text = "";
            chkActive.Checked = false;
            chkAvailable.Checked = false;
        }

        private void btnDeleteTools_Click(object sender, EventArgs e)
        {
            string confirmationMessage = $"Are you sure you want to delete this record: ID {txtIDTools.Text}? This cannot be undone!";
            DialogResult choice = MessageBox.Show(confirmationMessage, "Confirm Delete", MessageBoxButtons.YesNo);
            if (choice == DialogResult.Yes)
            {
                var record = new Tools
                {
                    Id = Convert.ToInt64(txtIDTools.Text),
                };
                Tools.Delete(record);                
                ToolsGrid();
            }
            log.Info("Tool record deleted");
        }

        private void btnSaveTools_Click(object sender, EventArgs e)
        {
            var record = new Tools
            {
                Description = txtDescriptionTools.Text,
                AssetNumber = Convert.ToInt64(txtAssetNumberTools.Text),
                Brand = txtBrandTools.Text,
                Comments = txtCommentsTools.Text,
                Active = chkActive.Checked,
                Available = chkAvailable.Checked
            };

            if (txtIDTools.Text.Equals(""))
            {
                Tools.Insert(record);
            }
            else
            {
                record.Id = Convert.ToInt64(txtIDTools.Text);
                Tools.Update(record);
            }
            ToolsGrid();
            log.Info("New Tool record created");
        }

        private void ToolsGrid()
        {
            string query = "SELECT * FROM tools";
            var record = Tools.Select(query);
            dgvToolsRecord.DataSource = null;
            dgvToolsRecord.DataSource = record;

            // Set the available ToolID value in Rental
            string query1 = "SELECT * FROM tools WHERE available = 1";
            var record1 = Tools.Select(query1); 
            cboToolIDRental.DataSource = null;
            cboToolIDRental.DataSource = record1;
            cboToolIDRental.ValueMember = "id";
            cboToolIDRental.DisplayMember = "description";            
        }

        #endregion

        #region Borrowers

        private void btnFindBorrowers_Click(object sender, EventArgs e)
        {
            try
            {
                var record = new Borrowers
                {
                    Id = Convert.ToInt64(txtIDBorrowers.Text),
                };
                // reassign record to the found results
                record = Borrowers.Find(record);

                txtFirstNameBorrowers.Text = record.FirstName;
                txtLastNameBorrowers.Text = record.LastName;
                txtPhoneBorrowers.Text = record.Phone;
                txtEmailBorrowers.Text = record.Email;
            }
            catch
            {
                MessageBox.Show($"There is no record ID {txtIDBorrowers.Text}");
            }
        }

        private void btnNewBorrowers_Click(object sender, EventArgs e)
        {
            txtIDBorrowers.Text = "";
            txtFirstNameBorrowers.Text = "";
            txtLastNameBorrowers.Text = "";
            txtPhoneBorrowers.Text = "";
            txtEmailBorrowers.Text = "";
        }

        private void btnDeleteBorrowers_Click(object sender, EventArgs e)
        {
            string confirmationMessage = $"Are you sure you want to delete this record: ID {txtIDBorrowers.Text}? This cannot be undone!";
            DialogResult choice = MessageBox.Show(confirmationMessage, "Confirm Delete", MessageBoxButtons.YesNo);
            if (choice == DialogResult.Yes)
            {
                var record = new Borrowers
                {
                    Id = Convert.ToInt64(txtIDBorrowers.Text),
                };
                Borrowers.Delete(record);
                BorrowersGrid();
            }
        }

        private void btnSaveBorrowers_Click(object sender, EventArgs e)
        {
            var record = new Borrowers
            {
                FirstName = txtFirstNameBorrowers.Text,
                LastName = txtLastNameBorrowers.Text,
                Phone = txtPhoneBorrowers.Text,
                Email = txtEmailBorrowers.Text
            };

            if (txtIDBorrowers.Text.Equals(""))
            {
                Borrowers.Insert(record);
            }
            else
            {
                record.Id = Convert.ToInt64(txtIDBorrowers.Text);
                Borrowers.Update(record);
            }
            BorrowersGrid();
        }

        private void BorrowersGrid()
        {
            var record = Borrowers.SelectAll();
            dgvBorrowersRecord.DataSource = null;
            dgvBorrowersRecord.DataSource = record;

            // Set borrowers in Rental
            cboBorrowerIDRental.DataSource = null;
            cboBorrowerIDRental.DataSource = record;
            cboBorrowerIDRental.ValueMember = "id";
            cboBorrowerIDRental.DisplayMember = "FullName"; // or nothing, let ToString do its work
        }

        #endregion

        #region Rental

        private void btnFindRental_Click(object sender, EventArgs e)
        {
            // set the datetimepickerFormat back to short to display values
            dtpCheckOutRental.Format = DateTimePickerFormat.Short;
            dtpCheckInRental.Format = DateTimePickerFormat.Short;

            try
            {
                var record = new Rental
                {
                    Id = Convert.ToInt64(txtIDRental.Text),
                };
                // Assign record1 to the found results
                var record1 = Rental.FindAll(record);                
                
                // check in record1 if the checkIn time is null
                // Null datatime in sqlite database still has a 01-01-01 value
                // So do not compare record1.Check to null
                if (record1.CheckIn > dtpCheckInRental.MinDate)
                {
                    cboBorrowerIDRental.SelectedValue = record1.Borrower;
                    cboToolIDRental.SelectedValue = record1.Tool;
                    dtpCheckOutRental.Value = record1.CheckOut;
                    dtpCheckInRental.Value = record1.CheckIn;                   
                }
                else
                {
                    var record2 = Rental.FindNull(record);
                    cboBorrowerIDRental.SelectedValue = record2.Borrower;
                    cboToolIDRental.SelectedValue = record2.Tool;
                    dtpCheckOutRental.Value = record2.CheckOut;
                    dtpCheckInRental.Format = DateTimePickerFormat.Custom;
                }                
            }
            catch 
            {
                MessageBox.Show($"There is no record ID {txtIDRental.Text}");
            }
        }

        private void btnNewRental_Click(object sender, EventArgs e)
        {
            txtIDRental.Text = "";           
            dtpCheckOutRental.Format = DateTimePickerFormat.Custom;
            dtpCheckInRental.Format = DateTimePickerFormat.Custom;
        }

        private void btnDeleteRental_Click(object sender, EventArgs e)
        {
            string confirmationMessage = $"Are you sure you want to delete this record: ID {txtIDRental.Text}? This cannot be undone!";
            DialogResult choice = MessageBox.Show(confirmationMessage, "Confirm Delete", MessageBoxButtons.YesNo);
            if (choice == DialogResult.Yes)
            {
                var record = new Rental
                {
                    Id = Convert.ToInt64(txtIDRental.Text),
                };
                Rental.Delete(record);
                RentalGrid();
            }
        }

        private void btnSaveRental_Click(object sender, EventArgs e)
        {            
            // no id provided means it is a new record, use insert method
            if (txtIDRental.Text.Equals(""))
            {
                var record = new Rental
                {
                    CheckOut = dtpCheckOutRental.Value,                    
                    Borrower = Convert.ToInt64(cboBorrowerIDRental.SelectedValue),
                    Tool = Convert.ToInt64(cboToolIDRental.SelectedValue)
                };
                Rental.Insert(record);

                // update the avaiable status in Tools table
                var record1 = new Tools
                {
                    Id = Convert.ToInt64(cboToolIDRental.SelectedValue),
                    Available = false
                };
                Tools.UpdateRental(record1);            
                ToolsGrid();
            }
            // if there is a id, means update the record
            else
            {
                var record = new Rental
                {
                    Id = Convert.ToInt64(txtIDRental.Text),
                    CheckIn = dtpCheckInRental.Value
                };
                Rental.Update(record);

                // update the avaiable status in Tools table
                var record1 = new Tools
                {
                    Id = Convert.ToInt64(cboToolIDRental.SelectedValue),
                    Available = true
                };
                Tools.UpdateRental(record1);
                ToolsGrid();
            }
            RentalGrid();
        }

        private void RentalGrid()
        {
            var record = RentalHistory.Select();
            dgvRentalRecord.DataSource = null;
            dgvRentalRecord.DataSource = record;
        }

        private void dtpCheckOutRental_ValueChanged(object sender, EventArgs e)
        {
            dtpCheckOutRental.Format = DateTimePickerFormat.Short;
        }

        private void dtpCheckInRental_ValueChanged(object sender, EventArgs e)
        {
            dtpCheckInRental.Format = DateTimePickerFormat.Short;
        }
        #endregion

        #region Reports

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (dgvReportPreview.RowCount > 0)
            {
                try
                {
                    Debug.Print("Ready to export");
                    
                    SaveFileDialog dialog = new SaveFileDialog()
                    {
                        DefaultExt = "csv",
                        AddExtension = true,
                        Filter = "CSV Files (*.csv)|*,csv"
                    };

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {                        
                        string data = "";
                        // Column headers are not counted in rows, so need a separate export
                        foreach (DataGridViewColumn column in dgvReportPreview.Columns)
                        {
                         data = data + column.HeaderText + ",";
                        }
                        // Remove the last comma which can be seen in Notepad, although not in Excel
                        data = data.Remove(data.LastIndexOf(",")) + "\r\n";

                        foreach (DataGridViewRow row in dgvReportPreview.Rows)
                        {
                            foreach (DataGridViewCell cell in row.Cells)
                            {
                                data = data + cell.Value.ToString().Replace(",", ":") + ",";
                            }
                            // Another method to delete the last char of a string
                            data = data.Remove(data.Length - 1) + "\r\n";                          
                        }
                     
                        System.IO.File.WriteAllText(dialog.FileName, data);
                        MessageBox.Show("File Saved");
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    MessageBox.Show($"{ex}");
                }
            }
            else
            {
                Debug.Assert(dgvReportPreview.RowCount > 0, "No record to export");
                MessageBox.Show("No record to export");
            }
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "";
                if (cboReportType.SelectedIndex == 0)
                {
                    query = "SELECT * FROM tools WHERE active = 1 AND available = 0";
                }
                else if (cboReportType.SelectedIndex == 1)
                {
                    query = "SELECT * FROM tools WHERE active = 1";
                }
                else if (cboReportType.SelectedIndex == 2)
                {
                    query = "SELECT * FROM tools WHERE active = 1 AND brand=@brand";
                }
                else if (cboReportType.SelectedIndex == 3)
                {
                    query = "SELECT * FROM tools WHERE active = 0";
                }
                else
                {
                    query = "SELECT * FROM tools WHERE active = 0 AND brand=@brand";
                }

                var record = new Report
                {
                    Brand = cboBrand.Text
                };

                var record1 = Report.Select(query, record);
                dgvReportPreview.DataSource = null;
                dgvReportPreview.DataSource = record1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}");
            }            
        }

        private void ReportsGrid()
        {
            // comboBox list for brand
            string query = "SELECT DISTINCT brand FROM tools";
            var record = Tools.Select(query);
            cboBrand.DataSource = null;
            cboBrand.DataSource = record;
            cboBrand.DisplayMember = "brand";

            // comboBox list for ReportsType
            string[] ReportsType = 
            {
                "All currently checked out tools",
                "All currently active tools",
                "All currently active tools, filtered by brand",
                "All currently retired tools",
                "All currently retired tools, filtered by brand"
            };
            cboReportType.DataSource = null;
            cboReportType.DataSource = ReportsType;

            // userSettings color used for the Export button
            btnExport.BackColor = Properties.Settings.Default.myColor;
        }
        
        #endregion        
    }   
}
