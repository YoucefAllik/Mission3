using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mission3_GSB
{
    public partial class FrmUpdate : Form
    {
        private gsbrapportsEntities mesDonneesEF;
        public FrmUpdate(gsbrapportsEntities mesDonnees)
        {
            InitializeComponent();
            this.mesDonneesEF = mesDonnees;
            this.bdgMedicament.DataSource = this.mesDonneesEF.Set<medicament>().ToList();
            this.bdgFamille.DataSource = this.mesDonneesEF.Set<famille>().ToList();
        }

    
      

        private void btnValider_Click(object sender, EventArgs e)
        {

            this.bdgMedicament.EndEdit();
            this.mesDonneesEF.SaveChanges();
            MessageBox.Show("Votre médicament a bien été mis à jour");
            this.Close();
        }

        private void btnAnnuler_Click(object sender, EventArgs e)
        {

            this.Close();
            
        }

        
    }
}
