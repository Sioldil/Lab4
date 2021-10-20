using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lab4_4.Models;

namespace Lab4_4
{
    public partial class Form1 : Form
    {
        ProductDB Context = new ProductDB();
        public Form1()
        {
            InitializeComponent();
        }
        #region Method
        private void loadGridView(List<Order> listOders)
        {
            dgvOrder.Rows.Clear();
            List<Order> List = listOders.Where(p => p.Invoice.DeliveryDate >= dtpFrom.Value && p.Invoice.DeliveryDate <= dtpTo.Value).ToList();
            var List2 = from q in List
                        group q by new { q.InvoiceNo, q.Invoice.OrderDate, q.Invoice.DeliveryDate} into p
                        select new { InvoiceNo = p.Key.InvoiceNo, OrderDate = p.Key.OrderDate, p.Key.DeliveryDate, Price = p.Sum(o => o.Price * o.Quantity) };
            foreach (var item in List2)
            {
                int index = dgvOrder.Rows.Add();

                for(int i = 0; i< dgvOrder.Rows.Count ; i++)

                dgvOrder.Rows[index].Cells[0].Value = i;
                dgvOrder.Rows[index].Cells[1].Value = item.InvoiceNo;
                dgvOrder.Rows[index].Cells[2].Value = item.OrderDate.ToString("dd/MM/yyyy");
                dgvOrder.Rows[index].Cells[3].Value = item.DeliveryDate.Date.ToString("dd/MM/yyyy");
                dgvOrder.Rows[index].Cells[4].Value = item.Price;
            }
        }

        public void SetMyCustomFormat()
        {
            // Set the Format type and the CustomFormat string.
            dtpFrom.Format = DateTimePickerFormat.Custom;
            dtpFrom.CustomFormat = "dd/MM/yyyy";
            dtpTo.Format = DateTimePickerFormat.Custom;
            dtpTo.CustomFormat = "dd/MM/yyyy";
        }

        private void checkBox1_CheckStateChanged()
        {
            var M = dtpFrom.Value.Month;
            var Y = dtpFrom.Value.Year;
            dtpFrom.Value = new DateTime(Y, M, 01);
            if (M == 1 || M == 3 || M == 5 || M == 7 || M == 8 || M == 10 || M == 12)
            {
                dtpTo.Value = new DateTime(Y, M, 31);
            }
            else if (M == 4 || M == 6 || M == 9 || M == 11)
            {
                dtpTo.Value = new DateTime(Y, M, 30);
            }
            else
            {
                dtpTo.Value = new DateTime(Y, M, 28);
            }
        }
        #endregion

        #region Event
        private void Form1_Load(object sender, EventArgs e)
        {
            dtpFrom.Value = new DateTime(2019, 10, 01);
            dtpTo.Value = new DateTime(2019, 10, 03);
            SetMyCustomFormat();
            List<Order> listOrders = Context.Orders.ToList();
            loadGridView(listOrders);        
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            List<Order> listStudents = Context.Orders.ToList();
            loadGridView(listStudents);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                checkBox1_CheckStateChanged();
            }
        }

        #endregion


    }
}
