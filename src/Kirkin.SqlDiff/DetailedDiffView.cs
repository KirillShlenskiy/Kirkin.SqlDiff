using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using DiffPlex;

using Kirkin.Data;

namespace Kirkin.SqlDiff
{
    public partial class DetailedDiffView : Form
    {
        private readonly DataTableLite LeftTable;
        private readonly DataTableLite RightTable;

        public DetailedDiffView(DataTableLite leftTable, DataTableLite rightTable)
        {
            LeftTable = leftTable;
            RightTable = rightTable;

            InitializeComponent();
        }

        // Designer support.
        public DetailedDiffView()
        {
            InitializeComponent();
        }
    }
}