﻿using System;
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
    public partial class FrmAddFamily : Form
    {
        private gsbrapportsEntities mesDonneesEF;
        public FrmAddFamily(gsbrapportsEntities mesDonnees)
        {
            InitializeComponent();
            this.mesDonneesEF = mesDonnees;
        }

        private void btnFermer_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private famille nvlFamille()
        {
            string nomF = txtNameF.Text;
            string abreviation = txtAbrev.Text;

            famille famille = new famille();
            famille.id = abreviation;
            famille.libelle = nomF;

            return famille;
        }

        private void btnValider_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtNameF.Text == "" || txtAbrev.Text == "")
                {
                    MessageBox.Show("Il faut remplir tous les champs de la nouvelle famille");

                }
                else
                {
                    this.mesDonneesEF.Set<famille>().Add(nvlFamille());
                    this.mesDonneesEF.SaveChanges();
                    MessageBox.Show("Votre famille de médicaments a bien été ajouter :=)");
                    this.Close();
                }

            }
            catch(Exception err)
            {
                MessageBox.Show(err.ToString());
            }

        }
    }
}
